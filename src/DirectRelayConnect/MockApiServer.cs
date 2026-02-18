using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DirectRelayConnect;

/// <summary>
/// Minimal HTTP API server that mimics the ReadyMP master server endpoints
/// required by the in-game WukongMp.Coop mod (CLIENT side).
///
/// The game mod's HttpBlobClient makes raw HTTP/1.1 requests (via BouncyCastle TcpClient)
/// to whatever API_BASE_URL is in the handshake file. Our mock handles these 4 routes:
///
///   GET  /api/server/{id}/files/{filename}          → returns { "downloadUrl": "..." }
///   GET  /api/server/{id}/files/upload-sas?...      → returns "http://...upload-url..."
///   GET  /blobs/{filename}                          → returns raw save bytes
///   PUT  /blobs/{filename}                          → stores uploaded save bytes
///
/// This is the MISSING PIECE that prevented the game from completing its init & connecting
/// to the relay. Without these responses, the mod's initialization would silently fail
/// and never call RequestConnect() on the relay.
/// </summary>
class MockApiServer
{
    readonly HttpListener _listener;
    readonly int _port;
    readonly Action<string, string, ConsoleColor> _log;
    
    // In-memory blob storage (save files)
    readonly ConcurrentDictionary<string, byte[]> _blobs = new();
    
    // Seed save data path (optional — from mods folder)
    string? _seedSavePath;

    public int Port => _port;
    public string BaseUrl => $"http://localhost:{_port}";

    public MockApiServer(int port, Action<string, string, ConsoleColor> log)
    {
        _port = port;
        _log = log;
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://+:{port}/");
    }

    /// <summary>
    /// Pre-load a save file so it's available for the first download request.
    /// Call this before StartAsync() with the seed .sav file content.
    /// </summary>
    public void PreSeedBlob(string name, byte[] data)
    {
        _blobs[name] = data;
        _log("API-MOCK", $"Pre-seeded blob '{name}' ({data.Length / 1024.0:F1} KB)", ConsoleColor.Cyan);
    }

    /// <summary>
    /// Set path to the mods folder containing ArchiveSaveFile.1.sav for seed data.
    /// </summary>
    public void SetSeedSavePath(string path)
    {
        _seedSavePath = path;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        try
        {
            _listener.Start();
            _log("API-MOCK", $"Mock API server listening on port {_port}", ConsoleColor.Green);
            _log("API-MOCK", $"Base URL: {BaseUrl}", ConsoleColor.Cyan);
        }
        catch (HttpListenerException ex) when (ex.ErrorCode == 5)
        {
            _log("API-MOCK", $"Cannot bind to port {_port} (access denied). Trying netsh reservation...", ConsoleColor.Yellow);
            TryAddUrlReservation();
            try
            {
                _listener.Start();
                _log("API-MOCK", $"Mock API server listening on port {_port} (after reservation)", ConsoleColor.Green);
            }
            catch (Exception ex2)
            {
                _log("API-MOCK", $"FAILED to start mock API server: {ex2.Message}", ConsoleColor.Red);
                _log("API-MOCK", $"The game mod WILL NOT be able to load save files.", ConsoleColor.Red);
                _log("API-MOCK", $"Try running as Administrator, or manually run:", ConsoleColor.Yellow);
                _log("API-MOCK", $"  netsh http add urlacl url=http://+:{_port}/ user=Everyone", ConsoleColor.Yellow);
                return;
            }
        }
        catch (Exception ex)
        {
            _log("API-MOCK", $"FAILED to start mock API server: {ex.Message}", ConsoleColor.Red);
            return;
        }

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var context = await _listener.GetContextAsync().WaitAsync(ct);
                _ = Task.Run(() => HandleRequest(context), ct);
            }
            catch (OperationCanceledException) { break; }
            catch (ObjectDisposedException) { break; }
            catch (Exception ex)
            {
                _log("API-MOCK", $"Listener error: {ex.Message}", ConsoleColor.Red);
            }
        }

        try { _listener.Stop(); } catch { }
        _log("API-MOCK", "Mock API server stopped", ConsoleColor.Yellow);
    }

    async Task HandleRequest(HttpListenerContext context)
    {
        var req = context.Request;
        var resp = context.Response;
        string method = req.HttpMethod;
        string path = req.Url?.AbsolutePath ?? "/";
        string query = req.Url?.Query ?? "";

        _log("API-MOCK", $"{method} {path}{query}", ConsoleColor.DarkCyan);

        try
        {
            // Route: GET /api/server/{id}/files/{filename}
            if (method == "GET" && path.StartsWith("/api/server/") && path.Contains("/files/") && !query.Contains("upload-sas"))
            {
                await HandleDownloadMetadata(path, resp);
                return;
            }

            // Route: GET /api/server/{id}/files/upload-sas?kind=...&userGuid=...&serverId=...
            if (method == "GET" && path.Contains("/files/upload-sas"))
            {
                await HandleUploadSas(path, query, resp);
                return;
            }

            // Route: GET /blobs/{filename}
            if (method == "GET" && path.StartsWith("/blobs/"))
            {
                await HandleBlobDownload(path, resp);
                return;
            }

            // Route: PUT /blobs/{filename}
            if (method == "PUT" && path.StartsWith("/blobs/"))
            {
                await HandleBlobUpload(path, req, resp);
                return;
            }

            // Catch-all: return 200 OK
            _log("API-MOCK", $"  Unhandled route, returning 200 OK", ConsoleColor.DarkYellow);
            resp.StatusCode = 200;
            resp.ContentType = "application/json";
            byte[] body = Encoding.UTF8.GetBytes("{}");
            resp.ContentLength64 = body.Length;
            await resp.OutputStream.WriteAsync(body);
        }
        catch (Exception ex)
        {
            _log("API-MOCK", $"  Error handling {method} {path}: {ex.Message}", ConsoleColor.Red);
            try
            {
                resp.StatusCode = 500;
                byte[] errBody = Encoding.UTF8.GetBytes($"{{\"error\":\"{ex.Message}\"}}");
                resp.ContentLength64 = errBody.Length;
                await resp.OutputStream.WriteAsync(errBody);
            }
            catch { }
        }
        finally
        {
            try { resp.Close(); } catch { }
        }
    }

    async Task HandleDownloadMetadata(string path, HttpListenerResponse resp)
    {
        string[] segments = path.Split('/');
        string filename = Uri.UnescapeDataString(segments[^1]);

        _log("API-MOCK", $"  Download metadata request for: '{filename}'", ConsoleColor.DarkCyan);

        bool hasBlob = _blobs.ContainsKey(filename);

        if (!hasBlob && _seedSavePath != null)
        {
            string seedFile = Path.Combine(_seedSavePath, "ArchiveSaveFile.1.sav");
            if (File.Exists(seedFile))
            {
                byte[] seedData = await File.ReadAllBytesAsync(seedFile);
                _blobs[filename] = seedData;
                hasBlob = true;
                _log("API-MOCK", $"  Auto-seeded '{filename}' from ArchiveSaveFile.1.sav ({seedData.Length / 1024.0:F1} KB)", ConsoleColor.Green);
            }
        }

        string json;
        if (hasBlob)
        {
            string downloadUrl = $"{BaseUrl}/blobs/{Uri.EscapeDataString(filename)}";
            json = JsonSerializer.Serialize(new { downloadUrl });
            _log("API-MOCK", $"  -> downloadUrl: {downloadUrl}", ConsoleColor.Green);
        }
        else
        {
            json = JsonSerializer.Serialize(new { downloadUrl = (string?)null });
            _log("API-MOCK", $"  -> no save file found, returning null (game will start fresh)", ConsoleColor.DarkYellow);
        }

        resp.StatusCode = 200;
        resp.ContentType = "application/json";
        resp.Headers.Add("Connection", "close");
        byte[] body = Encoding.UTF8.GetBytes(json);
        resp.ContentLength64 = body.Length;
        await resp.OutputStream.WriteAsync(body);
    }

    async Task HandleUploadSas(string path, string query, HttpListenerResponse resp)
    {
        // Parse query params (manual parsing to avoid System.Web dependency)
        var queryParams = ParseQueryString(query);
        queryParams.TryGetValue("kind", out string? kind);
        queryParams.TryGetValue("userGuid", out string? userGuid);

        string filename;
        if (kind == "WorldSave" || kind == "worldsave")
        {
            filename = "world.sav";
        }
        else if (!string.IsNullOrEmpty(userGuid) && userGuid != "null" && Guid.TryParse(userGuid, out var guid))
        {
            filename = $"player_{guid:N}.sav";
        }
        else
        {
            filename = $"upload_{DateTime.Now:yyyyMMdd_HHmmss}.sav";
        }

        string uploadUrl = $"{BaseUrl}/blobs/{Uri.EscapeDataString(filename)}";
        _log("API-MOCK", $"  Upload SAS for kind={kind}: -> {uploadUrl}", ConsoleColor.DarkCyan);

        string json = JsonSerializer.Serialize(uploadUrl);

        resp.StatusCode = 200;
        resp.ContentType = "application/json";
        resp.Headers.Add("Connection", "close");
        byte[] body = Encoding.UTF8.GetBytes(json);
        resp.ContentLength64 = body.Length;
        await resp.OutputStream.WriteAsync(body);
    }

    async Task HandleBlobDownload(string path, HttpListenerResponse resp)
    {
        string filename = Uri.UnescapeDataString(path.Substring("/blobs/".Length));

        if (_blobs.TryGetValue(filename, out byte[]? data))
        {
            _log("API-MOCK", $"  Serving blob '{filename}' ({data.Length / 1024.0:F1} KB)", ConsoleColor.Green);
            resp.StatusCode = 200;
            resp.ContentType = "application/octet-stream";
            resp.Headers.Add("Connection", "close");
            resp.ContentLength64 = data.Length;
            await resp.OutputStream.WriteAsync(data);
        }
        else
        {
            _log("API-MOCK", $"  Blob '{filename}' not found -> 404", ConsoleColor.DarkYellow);
            resp.StatusCode = 404;
            resp.Headers.Add("Connection", "close");
            byte[] body = Encoding.UTF8.GetBytes("{\"error\":\"Not found\"}");
            resp.ContentLength64 = body.Length;
            await resp.OutputStream.WriteAsync(body);
        }
    }

    async Task HandleBlobUpload(string path, HttpListenerRequest req, HttpListenerResponse resp)
    {
        string filename = Uri.UnescapeDataString(path.Substring("/blobs/".Length));

        using var ms = new MemoryStream();
        await req.InputStream.CopyToAsync(ms);
        byte[] rawData = ms.ToArray();

        byte[] saveData;
        try
        {
            using var gzStream = new GZipStream(new MemoryStream(rawData), CompressionMode.Decompress);
            using var decompressed = new MemoryStream();
            await gzStream.CopyToAsync(decompressed);
            saveData = decompressed.ToArray();
            _log("API-MOCK", $"  Received blob '{filename}': {rawData.Length / 1024.0:F1} KB compressed -> {saveData.Length / 1024.0:F1} KB", ConsoleColor.Green);
        }
        catch
        {
            saveData = rawData;
            _log("API-MOCK", $"  Received blob '{filename}': {saveData.Length / 1024.0:F1} KB (raw)", ConsoleColor.Green);
        }

        _blobs[filename] = saveData;

        resp.StatusCode = 201;
        resp.Headers.Add("Connection", "close");
        resp.ContentLength64 = 0;
    }

    /// <summary>
    /// Simple query string parser (avoids System.Web dependency).
    /// Input: "?kind=WorldSave&userGuid=abc" or "kind=WorldSave&userGuid=abc"
    /// </summary>
    static Dictionary<string, string> ParseQueryString(string query)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrEmpty(query)) return result;
        if (query.StartsWith('?')) query = query.Substring(1);
        foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var kv = pair.Split('=', 2);
            if (kv.Length == 2)
                result[Uri.UnescapeDataString(kv[0])] = Uri.UnescapeDataString(kv[1]);
            else if (kv.Length == 1)
                result[Uri.UnescapeDataString(kv[0])] = "";
        }
        return result;
    }

    void TryAddUrlReservation()
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"http add urlacl url=http://+:{_port}/ user=Everyone",
                UseShellExecute = true,
                Verb = "runas",
                CreateNoWindow = true
            };
            var proc = System.Diagnostics.Process.Start(psi);
            proc?.WaitForExit(10000);
            if (proc?.ExitCode == 0)
                _log("API-MOCK", $"URL reservation added for port {_port}", ConsoleColor.Green);
            else
                _log("API-MOCK", $"URL reservation failed (exit code {proc?.ExitCode})", ConsoleColor.Yellow);
        }
        catch (Exception ex)
        {
            _log("API-MOCK", $"URL reservation attempt failed: {ex.Message}", ConsoleColor.Yellow);
        }
    }
}
