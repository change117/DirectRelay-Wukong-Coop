using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Win32;
using Open.Nat;

namespace DirectRelay;

// === Configuration Classes ===
class RelayConfig
{
    public NetworkConfig Network { get; set; } = new();
    public PerformanceConfig Performance { get; set; } = new();
    public DiagnosticsConfig Diagnostics { get; set; } = new();
}

class NetworkConfig
{
    public int Port { get; set; } = 7777;
    public int UpdateTimeMs { get; set; } = 5;          // ReadyMP: Constants.ServerNetworkTickRateMs = 5
    public int DisconnectTimeoutMs { get; set; } = 5000;  // ReadyMP: Constants.ClientConnectionTimeoutMs = 5000
    public int SendBufferSizeKB { get; set; } = 1024;
    public int ReceiveBufferSizeKB { get; set; } = 1024;
    public int MTU { get; set; } = 1400;
}

class PerformanceConfig
{
    public bool HighPriorityThread { get; set; } = true;
    public bool HighPriorityProcess { get; set; } = true;
    public bool EnableStatistics { get; set; } = true;   // ReadyMP: EnableStatistics = true always
}

class DiagnosticsConfig
{
    public bool LogPackets { get; set; } = false;
    public bool LogConnections { get; set; } = true;
    public bool LogErrors { get; set; } = true;
    public string LogFilePath { get; set; } = "relay_diagnostics.log";
}

// === Object Pool for NetDataWriter (eliminates allocations) ===
static class NetDataWriterPool
{
    private static readonly ConcurrentBag<NetDataWriter> _pool = new();
    
    public static NetDataWriter Rent()
    {
        return _pool.TryTake(out var writer) ? writer : new NetDataWriter();
    }
    
    public static void Return(NetDataWriter writer)
    {
        writer.Reset();
        _pool.Add(writer);
    }
}

class Program
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AttachConsole(int dwProcessId);

    // File-based diagnostic log — every session writes to a file you can share
    static StreamWriter? _logFile;
    static readonly object _logLock = new();

    [STAThread]
    static async Task Main(string[] args)
    {
        // Ensure console is visible
        if (!AttachConsole(-1))
        {
            AllocConsole();
        }

        // Open diagnostic log file
        try
        {
            string logPath = Path.Combine(AppContext.BaseDirectory, $"directrelay_{DateTime.Now:yyyyMMdd_HHmmss}.log");
            _logFile = new StreamWriter(logPath, append: false) { AutoFlush = true };
            _logFile.WriteLine($"=== DirectRelay Diagnostic Log - {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
            _logFile.WriteLine($"Machine: {Environment.MachineName} | User: {Environment.UserName} | OS: {Environment.OSVersion}");
            _logFile.WriteLine($"LiteNetLib version: 1.3.1 (exact match with game mod assembly reference)");
            _logFile.WriteLine();
        }
        catch { }

        int port = 7777;
        if (args.Length > 0 && int.TryParse(args[0], out int p))
            port = p;

        // === Auto-create Windows Firewall rule ===
        EnsureFirewallRule(port);

        // === UPnP automatic port forwarding (essential for internet play) ===
        await TryUPnPPortForward(port);

        Console.Title = $"DirectRelay - Port {port}";
        Console.WriteLine("========================================================");
#if DIAGNOSTIC_MODE
        Console.WriteLine("     DirectRelay for ReadyMP (Wukong Co-op)  [DIAG]      ");
#else
        Console.WriteLine("     DirectRelay for ReadyMP (Wukong Co-op)  [PROD]      ");
#endif
        Console.WriteLine("========================================================");
        Console.WriteLine($"  Listening on port : {port}");
        Console.WriteLine($"  Machine           : {Environment.MachineName}");
        Console.WriteLine($"  Time started      : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
#if DIAGNOSTIC_MODE
        Console.WriteLine("  Mode              : DIAGNOSTIC (Full Logging)");
#else
        Console.WriteLine("  Mode              : PRODUCTION (Optimized)");
#endif
        Console.WriteLine("========================================================");
        Console.WriteLine();

        // === Show network info for the HOST ===
        Log("NETWORK", "Detecting network addresses for your friend to connect...", ConsoleColor.Cyan);
        try
        {
            var hostName = System.Net.Dns.GetHostName();
            var addresses = System.Net.Dns.GetHostAddresses(hostName);
            foreach (var addr in addresses)
            {
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(addr))
                {
                    Log("NETWORK", $"  LAN IP: {addr} (only works if friend is on your same local network)", ConsoleColor.DarkGray);
                }
            }
        }
        catch (Exception ex)
        {
            Log("NETWORK", $"  Could not detect local IPs: {ex.Message}", ConsoleColor.Yellow);
        }

        // Try to get public IP — this is the one your friend needs for internet play
        try
        {
            using var httpClient = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            string publicIp = (await httpClient.GetStringAsync("https://api.ipify.org")).Trim();
            Log("NETWORK", $"  *** PUBLIC IP: {publicIp} *** <- GIVE THIS TO YOUR FRIEND", ConsoleColor.Green);
            Log("NETWORK", $"  Your friend enters this IP in DirectRelayConnect to join.", ConsoleColor.Cyan);
        }
        catch
        {
            Log("NETWORK", $"  Could not determine public IP (no internet or API down)", ConsoleColor.DarkYellow);
        }
        Console.WriteLine();

        // === Launch GUI Control Panel ===
        Log("GUI", "Starting control panel...", ConsoleColor.Cyan);
        var launchSignal = new ManualResetEvent(false);
        
        var guiThread = new Thread(() =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var controlPanel = new ControlPanelWindow(launchSignal, Log);
            Application.Run(controlPanel);
        });
        guiThread.SetApartmentState(ApartmentState.STA);
        guiThread.Start();

        Log("GUI", "Control panel ready", ConsoleColor.Green);
        Log("GUI", "Waiting for user to click 'Launch Game'...", ConsoleColor.Yellow);
        Console.WriteLine();

        // Block until user clicks "Launch Game"
        launchSignal.WaitOne();
        
        Log("GUI", "Launch signal received from control panel", ConsoleColor.Green);
        Console.WriteLine();

        // === START MOCK API SERVER ===
        // The in-game WukongMp.Coop mod makes HTTP requests to API_BASE_URL during initialization.
        // Without a server responding, the mod silently fails and never calls RequestConnect().
        // This mock handles the 4 endpoints the mod needs: file download metadata, upload SAS,
        // blob download, and blob upload.
        const int mockApiPort = 8769;
        var mockApi = new MockApiServer(mockApiPort, Log);
        var mockApiCts = new CancellationTokenSource();
        _ = Task.Run(() => mockApi.StartAsync(mockApiCts.Token));
        // Give the mock server a moment to bind the port
        await Task.Delay(500);
        Log("SETUP", $"Mock API server ready at {mockApi.BaseUrl}", ConsoleColor.Green);
        Console.WriteLine();

        // === AUTO-SETUP: Install mods + write handshake for the HOST ===
        var (setupModDir, setupUserGuid) = await AutoSetupHost(port, mockApi.BaseUrl);

        var server = new DirectRelayServer(port);

        // Pre-seed relay blob storage with the HOST's seed save data.
        // This matches the MP Launcher's flow: the launcher downloads world.sav from the API
        // and makes it available before the CLIENT's game tries to load it.
        // Without this, the CLIENT's OnLoadArchive blob download returns null and falls
        // back to a generic slot-1 save, potentially loading a different world state.
        if (setupModDir != null)
        {
            string seedSavePath = Path.Combine(setupModDir, "ArchiveSaveFile.1.sav");
            if (File.Exists(seedSavePath))
            {
                byte[] saveData = File.ReadAllBytes(seedSavePath);
                server.PreSeedBlob("world.sav", saveData);
                server.PreSeedBlob($"player_{setupUserGuid:N}.sav", saveData);
                // Also pre-seed the mock API server so the game mod can download saves via HTTP
                mockApi.PreSeedBlob("world.sav", saveData);
                mockApi.PreSeedBlob($"player_{setupUserGuid:N}.sav", saveData);
                mockApi.SetSeedSavePath(setupModDir);
                Log("SETUP", $"Pre-seeded relay + API blobs from seed save ({saveData.Length / 1024.0:F1} KB)", ConsoleColor.Green);
            }
            else
            {
                Log("SETUP", "WARNING: Seed save not found — CLIENT won't be able to download world save", ConsoleColor.Red);
                // Still set seed path so mock can try to find it later
                mockApi.SetSeedSavePath(setupModDir);
            }
        }
        // === Monitor handshake file consumption ===
        // The in-game mod reads & DELETES the handshake file. If it's still there after
        // the game has been running for a while, it means the mod isn't loading.
        string handshakeMonitorPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ReadyM.Launcher", "wukong_handshake.env");
        _ = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(90)); // Give the game time to load
            if (File.Exists(handshakeMonitorPath))
            {
                Log("WARNING", "*** HANDSHAKE FILE WAS NOT CONSUMED AFTER 90 SECONDS ***", ConsoleColor.Red);
                Log("WARNING", $"  File still exists: {handshakeMonitorPath}", ConsoleColor.Red);
                Log("WARNING", "  This means the in-game ReadyMP mod did NOT load.", ConsoleColor.Red);
                Log("WARNING", "  Possible causes:", ConsoleColor.Yellow);
                Log("WARNING", "    1. CSharpLoader (version.dll or dwmapi.dll) is NOT installed in Win64/", ConsoleColor.Yellow);
                Log("WARNING", "       -> DirectRelay should auto-install it. If it failed, see the log above.", ConsoleColor.Yellow);
                Log("WARNING", "    2. CSharpLoader is installed but failed to find/load mods", ConsoleColor.Yellow);
                Log("WARNING", "       -> Check Win64/CSharpLoader/ for log files", ConsoleColor.Yellow);
                Log("WARNING", "    3. Mod DLLs missing from Win64/CSharpLoader/Mods/WukongMp.Coop/", ConsoleColor.Yellow);
                Log("WARNING", "    4. The game was launched through Steam instead of directly", ConsoleColor.Yellow);
            }
            else
            {
                Log("HANDSHAKE", "Handshake file was consumed by the game mod (good!)", ConsoleColor.Green);
            }
        });

        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
            Log("SHUTDOWN", "Ctrl+C received, shutting down...", ConsoleColor.Yellow);
        };

        try
        {
            await server.RunAsync(cts.Token);
        }
        catch (OperationCanceledException) { }

        // Clean up mock API server
        mockApiCts.Cancel();
        Log("SHUTDOWN", "Server stopped.", ConsoleColor.Yellow);
    }

    static async Task<(string? ModDir, Guid UserGuid)> AutoSetupHost(int port, string apiBaseUrl)
    {
        Log("SETUP", "=== Auto-Setup: Preparing host machine ===", ConsoleColor.Cyan);

        // --- Find the game ---
        string? gamePath = FindGameExe();
        if (gamePath == null)
        {
            Log("SETUP", "Could not find Black Myth: Wukong installation.", ConsoleColor.DarkYellow);
            Log("SETUP", "Mod auto-install skipped. Server will run relay-only.", ConsoleColor.DarkYellow);
            Console.WriteLine();
            return (null, Guid.Empty);
        }

        // Path layout:  {install}/b1/Binaries/Win64/b1-Win64-Shipping.exe
        string win64Dir = Path.GetDirectoryName(gamePath)!;
        // Navigate up to game install root (Win64 -> Binaries -> b1 -> root)
        string gameInstallRoot = Path.GetFullPath(Path.Combine(win64Dir, "..", "..", ".."));
        // b1 directory (CSharpLoader zip extracts relative to this)
        string b1Dir = Path.GetFullPath(Path.Combine(win64Dir, "..", ".."));
        // CSharpLoader mod directory: Win64/CSharpLoader/Mods/WukongMp.Coop/
        string csLoaderModsDir = Path.Combine(win64Dir, "CSharpLoader", "Mods");
        string coopModDir = Path.Combine(csLoaderModsDir, "WukongMp.Coop");

        Log("SETUP", $"Game found: {gamePath}", ConsoleColor.Green);
        Log("SETUP", $"  Win64 dir      : {win64Dir}", ConsoleColor.DarkCyan);
        Log("SETUP", $"  Install root   : {gameInstallRoot}", ConsoleColor.DarkCyan);
        Log("SETUP", $"  Mod target dir : {coopModDir}", ConsoleColor.DarkCyan);

        // --- Check for CSharpLoader (the mod framework) ---
        // CSharpLoader can use either version.dll (open-source) or dwmapi.dll (ReadyMP fork) as proxy
        string versionDllPath = Path.Combine(win64Dir, "version.dll");
        string dwmapiPath = Path.Combine(win64Dir, "dwmapi.dll");
        string csLoaderDir = Path.Combine(win64Dir, "CSharpLoader");
        bool hasProxyDll = File.Exists(versionDllPath) || File.Exists(dwmapiPath);
        bool hasLoaderDir = Directory.Exists(csLoaderDir) && File.Exists(Path.Combine(csLoaderDir, "CSharpModBase.dll"));

        if (!hasProxyDll || !hasLoaderDir)
        {
            Log("SETUP", "", ConsoleColor.Yellow);
            Log("SETUP", "*** CSharpLoader NOT FOUND — attempting automatic download... ***", ConsoleColor.Yellow);
            Log("SETUP", "  CSharpLoader is the mod framework that loads WukongMp.Coop into the game.", ConsoleColor.Cyan);

            bool installed = await TryDownloadCSharpLoader(b1Dir, win64Dir);
            if (installed)
            {
                // Re-check after install
                hasProxyDll = File.Exists(versionDllPath) || File.Exists(dwmapiPath);
                hasLoaderDir = Directory.Exists(csLoaderDir) && File.Exists(Path.Combine(csLoaderDir, "CSharpModBase.dll"));
            }

            if (!hasProxyDll || !hasLoaderDir)
            {
                Log("SETUP", "", ConsoleColor.Red);
                Log("SETUP", "*** CSharpLoader auto-install FAILED — manual install required ***", ConsoleColor.Red);
                Log("SETUP", "  TO FIX — Install CSharpLoader for Black Myth: Wukong:", ConsoleColor.Yellow);
                Log("SETUP", "    Option A: Run the ReadyMP Launcher ONCE (free download at readymp.com)", ConsoleColor.Yellow);
                Log("SETUP", "             It auto-installs CSharpLoader, then you can close it.", ConsoleColor.Yellow);
                Log("SETUP", "    Option B: Manually download from:", ConsoleColor.Yellow);
                Log("SETUP", "             https://github.com/czastack/B1CSharpLoader/releases", ConsoleColor.Yellow);
                Log("SETUP", "             Extract the zip so that version.dll ends up in:", ConsoleColor.Yellow);
                Log("SETUP", $"               {win64Dir}", ConsoleColor.Yellow);
                Log("SETUP", "             and CSharpLoader/ folder ends up in the same directory.", ConsoleColor.Yellow);
                Log("SETUP", "", ConsoleColor.Red);
            }
        }
        else
        {
            string proxyName = File.Exists(versionDllPath) ? "version.dll" : "dwmapi.dll";
            Log("SETUP", $"CSharpLoader found (proxy: {proxyName}) — mod framework OK", ConsoleColor.Green);
        }

        // --- Install mod files ---
        string? modsSourceDir = null;
        string[] searchPaths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "mods"),
            Path.Combine(AppContext.BaseDirectory, "..", "mods"),
            Path.Combine(AppContext.BaseDirectory, "..", "MOD-FILES-FOR-FRIEND"),
            Path.Combine(AppContext.BaseDirectory, "MOD-FILES-FOR-FRIEND"),
        };

        foreach (var candidate in searchPaths)
        {
            if (Directory.Exists(candidate))
            {
                modsSourceDir = Path.GetFullPath(candidate);
                break;
            }
        }

        if (modsSourceDir != null)
        {
            Log("SETUP", $"Mod source: {modsSourceDir}", ConsoleColor.Cyan);

            // Create the CSharpLoader/Mods/WukongMp.Coop/ directory structure
            Directory.CreateDirectory(coopModDir);

            int copied = 0;
            int skipped = 0;

            foreach (var srcFile in Directory.GetFiles(modsSourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(modsSourceDir, srcFile);
                if (relativePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Install into CSharpLoader/Mods/WukongMp.Coop/ (not flat into Win64)
                string destFile = Path.Combine(coopModDir, relativePath);
                string? destSubDir = Path.GetDirectoryName(destFile);
                if (destSubDir != null && !Directory.Exists(destSubDir))
                    Directory.CreateDirectory(destSubDir);

                try
                {
                    var srcInfo = new FileInfo(srcFile);
                    bool needsCopy = true;

                    if (File.Exists(destFile))
                    {
                        var dstInfo = new FileInfo(destFile);
                        if (srcInfo.Length == dstInfo.Length && srcInfo.LastWriteTimeUtc == dstInfo.LastWriteTimeUtc)
                        {
                            skipped++;
                            needsCopy = false;
                        }
                    }

                    if (needsCopy)
                    {
                        File.Copy(srcFile, destFile, true);
                        copied++;
                        Log("SETUP", $"  [COPY] {relativePath} -> CSharpLoader/Mods/WukongMp.Coop/", ConsoleColor.Green);
                    }
                }
                catch (Exception ex)
                {
                    Log("SETUP", $"  [FAIL] {relativePath}: {ex.Message}", ConsoleColor.Red);
                }
            }

            Log("SETUP", $"Mods installed: {copied} copied, {skipped} already up-to-date", ConsoleColor.Green);
        }
        else
        {
            Log("SETUP", "No mods source folder found, checking if already installed...", ConsoleColor.DarkYellow);
        }

        // Verify critical mods
        string[] criticalMods = new[]
        {
            "ReadyM.Relay.Client.dll", "ReadyM.Relay.Common.dll", "ReadyM.Relay.Common.Wukong.dll",
            "ReadyM.Api.dll", "ReadyM.Api.Multiplayer.dll", "WukongMp.Api.dll", "WukongMp.Coop.dll"
        };

        int missing = criticalMods.Count(m => !File.Exists(Path.Combine(coopModDir, m)));
        if (missing > 0)
        {
            Log("SETUP", $"{missing} critical mod file(s) missing from {coopModDir}", ConsoleColor.Red);
            Log("SETUP", "Your game will not connect to this relay without them.", ConsoleColor.Red);
        }
        else
        {
            Log("SETUP", "All 7 critical mod files verified in CSharpLoader/Mods/WukongMp.Coop/", ConsoleColor.Green);
        }

        // --- Prepare co-op save data ---
        // The in-game mod stores save files in GetModDirectory("WukongMp.Coop")
        // which resolves to {MOD_FOLDER}/WukongMp.Coop/ or Win64/CSharpLoader/Mods/WukongMp.Coop/
        // Our mods are already in coopModDir = Win64/CSharpLoader/Mods/WukongMp.Coop/
        // so save files and data files go there too.
        Directory.CreateDirectory(coopModDir);
        Log("SETUP", $"Co-op mod+save directory: {coopModDir}", ConsoleColor.DarkCyan);

        // Pre-write world save to slot 8 (matches MP Launcher flow)
        string seedSave = Path.Combine(coopModDir, "ArchiveSaveFile.1.sav");
        string slot8Dst = Path.Combine(coopModDir, "ArchiveSaveFile.8.sav");
        if (File.Exists(seedSave) && !File.Exists(slot8Dst))
        {
            File.Copy(seedSave, slot8Dst);
            Log("SETUP", "  Pre-seeded world save: ArchiveSaveFile.1.sav -> ArchiveSaveFile.8.sav", ConsoleColor.Green);
        }
        else if (File.Exists(seedSave))
        {
            Log("SETUP", "  Co-op seed save already present.", ConsoleColor.DarkCyan);
        }

        // --- Write handshake file for HOST (pointing to 127.0.0.1) ---
        string appDataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ReadyM.Launcher");
        Directory.CreateDirectory(appDataDir);

        string handshakePath = Path.Combine(appDataDir, "wukong_handshake.env");

        using var md5 = System.Security.Cryptography.MD5.Create();
        byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(
            Environment.MachineName + Environment.UserName));
        Guid userGuid = new Guid(hash);

        var handshake = new[]
        {
            $"LAUNCHER_PID={Environment.ProcessId}",
            "GAME_MODE=co-op",
            $"PLAYER_ID={userGuid}",
            $"NICKNAME={Environment.UserName}",
            "SERVER_ID=1",
            "SERVER_IP=127.0.0.1",
            $"SERVER_PORT={port}",
            $"API_BASE_URL={apiBaseUrl}",
            "JWT_TOKEN=direct-relay",
            $"MOD_FOLDER={csLoaderModsDir}"
        };

        File.WriteAllLines(handshakePath, handshake);
        Log("SETUP", $"Handshake written: {handshakePath}", ConsoleColor.Green);
        Log("SETUP", $"  Player GUID : {userGuid}", ConsoleColor.DarkCyan);
        Log("SETUP", $"  Server      : 127.0.0.1:{port}", ConsoleColor.DarkCyan);

        // --- Launch the game ---
        Log("SETUP", "Launching game...", ConsoleColor.Yellow);
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = gamePath,
                // WorkingDirectory must be the game install root (matching the official launcher)
                WorkingDirectory = gameInstallRoot,
                UseShellExecute = true
            };
            var proc = Process.Start(psi);
            if (proc != null)
                Log("SETUP", $"Game launched! PID={proc.Id}", ConsoleColor.Green);
            else
                Log("SETUP", "Game process started (PID unknown)", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            Log("SETUP", $"Failed to launch game: {ex.Message}", ConsoleColor.Red);
            Log("SETUP", "You can launch the game manually - the handshake file is ready.", ConsoleColor.Yellow);
        }

        Log("SETUP", "=== Auto-Setup complete. Starting relay server... ===", ConsoleColor.Cyan);
        Console.WriteLine();
        return (coopModDir, userGuid);
    }

    static string? FindGameExe()
    {
        // Steam registry (HKLM)
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 2358720");
            string? installDir = key?.GetValue("InstallLocation") as string;
            if (installDir != null)
            {
                string exe = Path.Combine(installDir, "b1", "Binaries", "Win64", "b1-Win64-Shipping.exe");
                if (File.Exists(exe)) return exe;
            }
        }
        catch { }

        // Steam registry (HKCU) + library folders
        try
        {
            using var steamKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");
            string? steamDir = steamKey?.GetValue("SteamPath") as string;
            if (steamDir != null)
            {
                string exe = Path.Combine(steamDir, "steamapps", "common", "BlackMythWukong",
                    "b1", "Binaries", "Win64", "b1-Win64-Shipping.exe");
                if (File.Exists(exe)) return exe;

                string vdfPath = Path.Combine(steamDir, "steamapps", "libraryfolders.vdf");
                if (File.Exists(vdfPath))
                {
                    foreach (var line in File.ReadAllLines(vdfPath))
                    {
                        if (line.Contains("\"path\""))
                        {
                            var path = line.Split('"')
                                .Where(s => s.Contains('\\') || s.Contains('/'))
                                .FirstOrDefault();
                            if (path != null)
                            {
                                exe = Path.Combine(path, "steamapps", "common", "BlackMythWukong",
                                    "b1", "Binaries", "Win64", "b1-Win64-Shipping.exe");
                                if (File.Exists(exe)) return exe;
                            }
                        }
                    }
                }
            }
        }
        catch { }

        // Common paths
        string[] commonPaths = new[]
        {
            @"C:\Program Files (x86)\Steam\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
            @"C:\Program Files\Steam\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
            @"D:\SteamLibrary\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
            @"E:\SteamLibrary\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
            @"F:\SteamLibrary\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
        };

        foreach (var p in commonPaths)
        {
            if (File.Exists(p)) return p;
        }

        return null;
    }

    /// <summary>
    /// Downloads and installs B1CSharpLoader from GitHub (czastack/B1CSharpLoader).
    /// The zip contains a b1/ folder that maps to the game's b1/ directory.
    /// Files: b1/Binaries/Win64/version.dll + b1/Binaries/Win64/CSharpLoader/...
    /// </summary>
    static async Task<bool> TryDownloadCSharpLoader(string b1Dir, string win64Dir)
    {
        const string GH_API = "https://api.github.com/repos/czastack/B1CSharpLoader/releases/latest";
        string tempZip = Path.Combine(Path.GetTempPath(), $"B1CSharpLoader_{Guid.NewGuid():N}.zip");

        try
        {
            using var http = new System.Net.Http.HttpClient();
            http.Timeout = TimeSpan.FromSeconds(30);
            http.DefaultRequestHeaders.Add("User-Agent", "DirectRelay");

            // Step 1: Get latest release info from GitHub API
            Log("CSLOADER", "Querying GitHub for latest B1CSharpLoader release...", ConsoleColor.Cyan);
            string releaseJson = await http.GetStringAsync(GH_API);

            // Parse the first .zip asset download URL
            string? downloadUrl = null;
            string? tagName = null;
            using (var doc = JsonDocument.Parse(releaseJson))
            {
                tagName = doc.RootElement.GetProperty("tag_name").GetString();
                var assets = doc.RootElement.GetProperty("assets");
                foreach (var asset in assets.EnumerateArray())
                {
                    string? name = asset.GetProperty("name").GetString();
                    if (name != null && name.StartsWith("B1CSharpLoader") && name.EndsWith(".zip"))
                    {
                        downloadUrl = asset.GetProperty("browser_download_url").GetString();
                        break;
                    }
                }
            }

            if (downloadUrl == null)
            {
                Log("CSLOADER", "Could not find B1CSharpLoader zip in latest release.", ConsoleColor.Red);
                return false;
            }

            Log("CSLOADER", $"Found release {tagName}: {Path.GetFileName(downloadUrl)}", ConsoleColor.Cyan);

            // Step 2: Download the zip
            Log("CSLOADER", "Downloading CSharpLoader...", ConsoleColor.Yellow);
            byte[] zipData = await http.GetByteArrayAsync(downloadUrl);
            await File.WriteAllBytesAsync(tempZip, zipData);
            Log("CSLOADER", $"Downloaded {zipData.Length / 1024.0:F0} KB", ConsoleColor.Cyan);

            // Step 3: Extract — the zip contains b1/Binaries/Win64/... structure
            // We need to extract so files land in the right place relative to the game's b1/ dir
            Log("CSLOADER", "Extracting to game directory...", ConsoleColor.Yellow);

            string tempExtract = Path.Combine(Path.GetTempPath(), $"B1CSLoader_extract_{Guid.NewGuid():N}");
            System.IO.Compression.ZipFile.ExtractToDirectory(tempZip, tempExtract, true);

            // The zip has b1/ at the root. Copy b1/ contents to game's b1/ dir.
            string extractedB1 = Path.Combine(tempExtract, "b1");
            if (Directory.Exists(extractedB1))
            {
                int filesCopied = 0;
                foreach (var srcFile in Directory.GetFiles(extractedB1, "*", SearchOption.AllDirectories))
                {
                    string relativePath = Path.GetRelativePath(extractedB1, srcFile);
                    string destFile = Path.Combine(b1Dir, relativePath);
                    string? destDir = Path.GetDirectoryName(destFile);
                    if (destDir != null && !Directory.Exists(destDir))
                        Directory.CreateDirectory(destDir);

                    // Don't overwrite existing Mods/ — our mod files go there
                    if (relativePath.Contains("Mods" + Path.DirectorySeparatorChar))
                        continue;

                    File.Copy(srcFile, destFile, true);
                    filesCopied++;
                    Log("CSLOADER", $"  [INSTALL] {relativePath}", ConsoleColor.Green);
                }
                Log("CSLOADER", $"CSharpLoader installed successfully! ({filesCopied} files)", ConsoleColor.Green);

                // Verify key files
                bool hasProxy = File.Exists(Path.Combine(win64Dir, "version.dll")) || File.Exists(Path.Combine(win64Dir, "dwmapi.dll"));
                bool hasBase = File.Exists(Path.Combine(win64Dir, "CSharpLoader", "CSharpModBase.dll"));
                if (hasProxy && hasBase)
                {
                    Log("CSLOADER", "CSharpLoader verification passed!", ConsoleColor.Green);
                }
                else
                {
                    Log("CSLOADER", "WARNING: Some CSharpLoader files may be missing after extraction.", ConsoleColor.Yellow);
                }
            }
            else
            {
                Log("CSLOADER", "Unexpected zip structure — no b1/ folder found.", ConsoleColor.Red);
                return false;
            }

            // Cleanup
            try { File.Delete(tempZip); } catch { }
            try { Directory.Delete(tempExtract, true); } catch { }

            return true;
        }
        catch (Exception ex)
        {
            Log("CSLOADER", $"Auto-download failed: {ex.Message}", ConsoleColor.Red);
            try { File.Delete(tempZip); } catch { }
            return false;
        }
    }

    internal static void Log(string tag, string msg, ConsoleColor color = ConsoleColor.Gray)
    {
        var ts = DateTime.Now.ToString("HH:mm:ss.fff");
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"[{ts}] ");
        Console.ForegroundColor = color;
        Console.Write($"[{tag,-10}] ");
        Console.ForegroundColor = prev;
        Console.WriteLine(msg);

        // Also write to diagnostic log file
        try
        {
            lock (_logLock)
            {
                _logFile?.WriteLine($"[{ts}] [{tag,-10}] {msg}");
            }
        }
        catch { }
    }

    static void EnsureFirewallRule(int port)
    {
        try
        {
            string exePath = Process.GetCurrentProcess().MainModule?.FileName ?? "";
            
            // Try to add firewall rule for both the exe and the port
            var psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"advfirewall firewall show rule name=\"DirectRelay UDP {port}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var proc = Process.Start(psi);
            proc?.WaitForExit(5000);
            string output = proc?.StandardOutput.ReadToEnd() ?? "";
            
            if (!output.Contains("DirectRelay"))
            {
                Log("FIREWALL", $"Creating firewall rule for UDP port {port}...", ConsoleColor.Yellow);
                
                // Add UDP inbound rule
                var add = Process.Start(new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = $"advfirewall firewall add rule name=\"DirectRelay UDP {port}\" dir=in action=allow protocol=UDP localport={port}",
                    UseShellExecute = true,
                    Verb = "runas",  // Request admin elevation
                    CreateNoWindow = true
                });
                add?.WaitForExit(10000);
                
                if (add?.ExitCode == 0)
                    Log("FIREWALL", $"Firewall rule created for UDP {port} (inbound)", ConsoleColor.Green);
                else
                    Log("FIREWALL", $"Could not auto-create firewall rule (may need admin rights)", ConsoleColor.Yellow);
            }
            else
            {
                Log("FIREWALL", $"Firewall rule already exists for UDP {port}", ConsoleColor.Green);
            }
        }
        catch (Exception ex)
        {
            Log("FIREWALL", $"Firewall check failed: {ex.Message} — you may need to manually allow UDP {port}", ConsoleColor.Yellow);
        }
    }

    /// <summary>
    /// Attempts UPnP/NAT-PMP automatic port forwarding on the router.
    /// This is ESSENTIAL for internet play — without it, players on different
    /// networks cannot reach the relay server because the router drops the packets.
    /// </summary>
    static async Task TryUPnPPortForward(int port)
    {
        Log("UPNP", "Attempting automatic router port forwarding (UPnP)...", ConsoleColor.Yellow);
        Log("UPNP", "This lets your friend connect from the internet without manual router config.", ConsoleColor.DarkCyan);
        
        // Detect this machine's LAN IP (used in all code paths)
        string localIp = "unknown";
        try
        {
            using var tempSocket = new System.Net.Sockets.Socket(
                System.Net.Sockets.AddressFamily.InterNetwork,
                System.Net.Sockets.SocketType.Dgram, 0);
            tempSocket.Connect("8.8.8.8", 65530);
            localIp = ((IPEndPoint)tempSocket.LocalEndPoint!).Address.ToString();
        }
        catch { }

        try
        {
            var discoverer = new NatDiscoverer();
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp | PortMapper.Pmp, cts);
            
            var externalIp = await device.GetExternalIPAsync();
            Log("UPNP", $"Router found! External IP: {externalIp}", ConsoleColor.Green);
            
            // Check if mapping already exists
            try
            {
                var existing = await device.GetSpecificMappingAsync(Protocol.Udp, port);
                if (existing != null)
                {
                    // Check if the existing mapping points to THIS machine
                    if (existing.PrivateIP.ToString() == localIp && existing.PrivatePort == port)
                    {
                        Log("UPNP", $"Port mapping already exists for this PC: UDP {port} -> {existing.PrivateIP}:{existing.PrivatePort}", ConsoleColor.Green);
                        Log("UPNP", $"*** PORT FORWARDING OK ***", ConsoleColor.Green);
                        Log("UPNP", $"Tell your friend to connect to: {externalIp}:{port}", ConsoleColor.Cyan);
                        return;
                    }
                    else
                    {
                        // Mapping exists but for a different IP/port — stale entry, delete it
                        Log("UPNP", $"Stale port mapping found: UDP {port} -> {existing.PrivateIP}:{existing.PrivatePort} (not this PC: {localIp})", ConsoleColor.Yellow);
                        Log("UPNP", $"Removing stale mapping and recreating for this PC...", ConsoleColor.Yellow);
                        await device.DeletePortMapAsync(existing);
                        Log("UPNP", $"Stale mapping removed.", ConsoleColor.Green);
                    }
                }
            }
            catch { /* No existing mapping — we'll create one */ }
            
            // Create the port mapping (lifetime = 4 hours, auto-renew on next launch)
            try
            {
                await device.CreatePortMapAsync(new Mapping(Protocol.Udp, port, port, 
                    14400, "DirectRelay Wukong Co-op"));
            }
            catch (MappingException mex) when (mex.Message.Contains("718") || mex.Message.Contains("Conflict"))
            {
                // ConflictInMappingEntry — try to force-delete and retry
                Log("UPNP", $"Conflict detected, attempting to remove old mapping and retry...", ConsoleColor.Yellow);
                try
                {
                    // Delete by creating a mapping object that matches the external port
                    await device.DeletePortMapAsync(new Mapping(Protocol.Udp, port, port, 0, ""));
                    await Task.Delay(500); // Brief pause for router to process
                    await device.CreatePortMapAsync(new Mapping(Protocol.Udp, port, port, 
                        14400, "DirectRelay Wukong Co-op"));
                }
                catch (Exception retryEx)
                {
                    Log("UPNP", $"Could not resolve conflict automatically: {retryEx.Message}", ConsoleColor.Yellow);
                    Log("UPNP", "*** MANUAL PORT FORWARDING REQUIRED ***", ConsoleColor.Red);
                    Log("UPNP", $"Your router has a conflicting port mapping for UDP {port}.", ConsoleColor.Yellow);
                    Log("UPNP", $"To fix this:", ConsoleColor.White);
                    Log("UPNP", $"  1. Open your router admin page (usually http://192.168.1.1)", ConsoleColor.White);
                    Log("UPNP", $"  2. Delete any existing port forwarding/UPnP rule for port {port}", ConsoleColor.White);
                    Log("UPNP", $"  3. Add new rule: Protocol=UDP, External Port={port}, Internal IP={localIp}, Internal Port={port}", ConsoleColor.White);
                    Log("UPNP", $"  4. Or restart the router to clear stale UPnP entries, then re-run DirectRelay", ConsoleColor.White);
                    return;
                }
            }
            
            Log("UPNP", $"*** PORT FORWARDING SUCCESSFUL! ***", ConsoleColor.Green);
            Log("UPNP", $"UDP {port} is now forwarded through your router.", ConsoleColor.Green);
            Log("UPNP", $"Tell your friend to connect to: {externalIp}:{port}", ConsoleColor.Cyan);
        }
        catch (NatDeviceNotFoundException)
        {
            Log("UPNP", "No UPnP/NAT-PMP router found.", ConsoleColor.Yellow);
            Log("UPNP", "*** MANUAL PORT FORWARDING REQUIRED for internet play! ***", ConsoleColor.Red);
            Log("UPNP", $"Steps to forward UDP port {port}:", ConsoleColor.Yellow);
            Log("UPNP", $"  1. Open your router admin page (usually http://192.168.1.1 or http://192.168.0.1)", ConsoleColor.White);
            Log("UPNP", $"  2. Find 'Port Forwarding' or 'Virtual Server' settings", ConsoleColor.White);
            Log("UPNP", $"  3. Add rule: Protocol=UDP, External Port={port}, Internal Port={port}", ConsoleColor.White);
            Log("UPNP", $"  4. Set Internal IP to THIS computer's LAN IP (shown above)", ConsoleColor.White);
            Log("UPNP", $"  5. Save and restart the router if needed", ConsoleColor.White);
            Log("UPNP", $"  Alternative: Some routers have a 'DMZ' option — set this PC as DMZ host", ConsoleColor.DarkGray);
        }
        catch (MappingException mex)
        {
            if (mex.Message.Contains("718") || mex.Message.Contains("Conflict"))
            {
                // Error 718 = ConflictInMappingEntry — a rule already exists for this port
                // This is usually FINE if you have a manual port forward set up
                Log("UPNP", $"UPnP auto-create skipped: a port mapping for {port} already exists in your router.", ConsoleColor.Yellow);
                Log("UPNP", $"If you have manually set up port forwarding for UDP {port}, this is normal and OK.", ConsoleColor.Green);
                Log("UPNP", $"If connectivity still fails, check that your router's port forward rule is ENABLED", ConsoleColor.Yellow);
                Log("UPNP", $"  and points to this PC's LAN IP: {localIp}", ConsoleColor.Yellow);
            }
            else
            {
                Log("UPNP", $"Router rejected port mapping: {mex.Message}", ConsoleColor.Yellow);
                Log("UPNP", "*** MANUAL PORT FORWARDING REQUIRED ***", ConsoleColor.Red);
                Log("UPNP", $"Forward UDP port {port} in your router settings to this PC's LAN IP ({localIp}).", ConsoleColor.Yellow);
                Log("UPNP", $"  Tip: Open your router admin page and delete any existing UPnP/port-forward", ConsoleColor.White);
                Log("UPNP", $"  rules for port {port}, then re-run DirectRelay.", ConsoleColor.White);
            }
        }
        catch (Exception ex)
        {
            Log("UPNP", $"UPnP failed: {ex.Message}", ConsoleColor.Yellow);
            Log("UPNP", "*** You may need to manually forward the port for internet play. ***", ConsoleColor.Yellow);
            Log("UPNP", $"Forward UDP port {port} in your router settings.", ConsoleColor.Yellow);
        }
    }

    /// <summary>
    /// Lists all network interfaces and their IPs, then performs a self-test
    /// by sending a UDP packet to ourselves via the public IP to verify port forwarding.
    /// </summary>
    internal static async Task TestExternalPortReachability(int port)
    {
        Console.WriteLine();
        Log("PORTTEST", "=== Port Reachability Self-Test ===", ConsoleColor.Cyan);

        // 1. List all network interfaces
        Log("PORTTEST", "Network interfaces on this machine:", ConsoleColor.Cyan);
        try
        {
            foreach (var nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus != System.Net.NetworkInformation.OperationalStatus.Up)
                    continue;
                
                var ipProps = nic.GetIPProperties();
                var ipv4Addrs = ipProps.UnicastAddresses
                    .Where(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(a => a.Address.ToString())
                    .ToList();
                
                if (ipv4Addrs.Count == 0) continue;
                
                string nicType = nic.NetworkInterfaceType.ToString();
                string ips = string.Join(", ", ipv4Addrs);
                Log("PORTTEST", $"  [{nicType}] {nic.Name}: {ips}", ConsoleColor.DarkCyan);
            }
        }
        catch (Exception ex)
        {
            Log("PORTTEST", $"  Could not enumerate interfaces: {ex.Message}", ConsoleColor.Yellow);
        }
        Console.WriteLine();

        // 2. Get public IP
        string? publicIp = null;
        try
        {
            using var httpClient = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            publicIp = (await httpClient.GetStringAsync("https://api.ipify.org")).Trim();
        }
        catch { }

        if (string.IsNullOrEmpty(publicIp))
        {
            Log("PORTTEST", "Could not determine public IP — skipping external reachability test.", ConsoleColor.Yellow);
            Console.WriteLine();
            return;
        }

        // 3. Self-test: Send a UDP ping to our own public IP and see if the relay echoes it back
        Log("PORTTEST", $"Testing if UDP port {port} is reachable at your public IP {publicIp}...", ConsoleColor.Yellow);
        Log("PORTTEST", $"  (Sending a test packet to {publicIp}:{port} — should loop back through router)", ConsoleColor.DarkGray);

        try
        {
            using var testUdp = new UdpClient();
            testUdp.Client.ReceiveTimeout = 4000;
            testUdp.Client.SendTimeout = 2000;

            byte[] pingData = System.Text.Encoding.UTF8.GetBytes("DIRECTRELAY_DIAG_PING_SELFTEST");
            testUdp.Send(pingData, pingData.Length, publicIp, port);

            try
            {
                var remoteEp = new IPEndPoint(IPAddress.Any, 0);
                byte[] response = testUdp.Receive(ref remoteEp);
                string responseStr = System.Text.Encoding.UTF8.GetString(response);

                if (responseStr.Contains("DIRECTRELAY_DIAG_PONG"))
                {
                    Log("PORTTEST", $"*** PORT {port} IS REACHABLE FROM THE INTERNET! ***", ConsoleColor.Green);
                    Log("PORTTEST", $"  Your friend can connect to {publicIp}:{port}", ConsoleColor.Green);
                    Log("PORTTEST", $"  Port forwarding is WORKING correctly.", ConsoleColor.Green);
                }
                else
                {
                    Log("PORTTEST", $"  Got a response ({response.Length} bytes) but not from our relay.", ConsoleColor.Yellow);
                    Log("PORTTEST", $"  Port may be reachable but something else is responding.", ConsoleColor.Yellow);
                }
            }
            catch (SocketException sex) when (sex.SocketErrorCode == SocketError.TimedOut)
            {
                Log("PORTTEST", $"*** PORT {port} IS NOT REACHABLE FROM YOUR PUBLIC IP ***", ConsoleColor.Red);
                Log("PORTTEST", $"  No response from {publicIp}:{port} within 4 seconds.", ConsoleColor.Red);
                Log("PORTTEST", $"", ConsoleColor.Red);
                Log("PORTTEST", $"  This means your friend CANNOT connect to you right now.", ConsoleColor.Red);
                Log("PORTTEST", $"  Possible causes:", ConsoleColor.Yellow);
                Log("PORTTEST", $"    1. Port forwarding rule is not enabled in your router", ConsoleColor.Yellow);
                Log("PORTTEST", $"    2. Port forward rule points to wrong LAN IP", ConsoleColor.Yellow);
                Log("PORTTEST", $"    3. Windows Firewall is blocking inbound UDP {port}", ConsoleColor.Yellow);
                Log("PORTTEST", $"    4. Antivirus/security software is blocking the port", ConsoleColor.Yellow);
                Log("PORTTEST", $"    5. ISP is blocking inbound UDP traffic (rare but possible)", ConsoleColor.Yellow);
                Log("PORTTEST", $"    6. Router needs a reboot after adding port forward rules", ConsoleColor.Yellow);
                Console.WriteLine();
                Log("PORTTEST", $"  Quick fixes to try:", ConsoleColor.Cyan);
                Log("PORTTEST", $"    - Reboot your router", ConsoleColor.White);
                Log("PORTTEST", $"    - Temporarily disable Windows Firewall to test", ConsoleColor.White);
                Log("PORTTEST", $"    - Temporarily disable antivirus to test", ConsoleColor.White);
                Log("PORTTEST", $"    - Try setting this PC as DMZ host in router", ConsoleColor.White);
                Log("PORTTEST", $"    - Some routers can't hairpin (self-test may fail but friend can still connect)", ConsoleColor.DarkGray);
            }
        }
        catch (Exception ex)
        {
            Log("PORTTEST", $"  Self-test failed: {ex.Message}", ConsoleColor.Yellow);
            Log("PORTTEST", $"  Note: Some routers don't support hairpin NAT (testing your own public IP).", ConsoleColor.DarkGray);
            Log("PORTTEST", $"  The port may still work for external clients — ask your friend to test.", ConsoleColor.DarkGray);
        }
        Console.WriteLine();
    }
}

class DirectRelayServer : INetEventListener
{
    // ReadyMP RelayMessageCode (from decompiled source)
    const byte MSG_HandshakeConnected = 255;
    const byte MSG_RequestAreaEvent = 254;
    const byte MSG_AreaEvent = 253;
    const byte MSG_OtherPlayerConnectionEvent = 252;
    const byte MSG_OtherPlayerAreaEvent = 251;
    const byte MSG_EcsDelta = 250;
    const byte MSG_EcsSnapshot = 249;
    const byte MSG_EcsCreateEntity = 248;
    const byte MSG_EcsDeleteEntity = 247;
    const byte MSG_EcsChangeOwnership = 246;
    const byte MSG_RequestDownloadBlob = 245;
    const byte MSG_DownloadBlobData = 244;
    const byte MSG_RequestUploadBlob = 243;
    const byte MSG_UploadBlobAck = 242;

    // ReadyMP RelayMode
    const byte MODE_AreaOthers = 0;
    const byte MODE_AreaAll = 1;
    const byte MODE_GlobalOthers = 2;
    const byte MODE_GlobalAll = 3;
    const byte MODE_EntityOwner = 4;
    const byte MODE_Peers = 5;

    readonly int _port;
    readonly NetManager _net;
    readonly RelayConfig _config;

    // Lock-free concurrent dictionaries for hot path
    readonly ConcurrentDictionary<int, PlayerInfo> _byPeerId = new();
    readonly ConcurrentDictionary<ushort, PlayerInfo> _byPlayerId = new();
    readonly Dictionary<string, AreaInfo> _areas = new();
    readonly ConcurrentDictionary<string, byte[]> _blobs = new();

    ushort _nextPlayerId = 1;
    uint _nextNetworkId = 1;
    readonly object _lock = new(); // Only for connection/disconnection, not packet forwarding

    // --- Diagnostics counters ---
    long _totalConnectionAttempts;
    long _totalConnectionsAccepted;
    long _totalConnectionsRejected;
    long _totalPacketsReceived;
    long _totalPacketsSent;
    long _totalBytesReceived;
    long _totalBytesSent;
    long _totalErrors;
    bool _warnedProtocolMismatch;
    DateTime _startTime;
    DateTime _lastStatusTime;

    class PlayerInfo
    {
        public NetPeer Peer = null!;
        public ushort PlayerId;
        public Guid UserGuid;
        public string? CurrentArea;
        public DateTime ConnectedAt;
        public long PacketsSent;
        public long PacketsReceived;
    }

    class AreaInfo
    {
        public string Id = "";
        public List<PlayerInfo> Players = new();
    }

    static readonly Dictionary<byte, string> MessageNames = new()
    {
        [255] = "HandshakeConnected",
        [254] = "RequestAreaEvent",
        [253] = "AreaEvent",
        [252] = "OtherPlayerConnectionEvent",
        [251] = "OtherPlayerAreaEvent",
        [250] = "EcsDelta",
        [249] = "EcsSnapshot",
        [248] = "EcsCreateEntity",
        [247] = "EcsDeleteEntity",
        [246] = "EcsChangeOwnership",
        [245] = "RequestDownloadBlob",
        [244] = "DownloadBlobData",
        [243] = "RequestUploadBlob",
        [242] = "UploadBlobAck"
    };

    static readonly Dictionary<byte, string> ModeNames = new()
    {
        [0] = "AreaOthers",
        [1] = "AreaAll",
        [2] = "GlobalOthers",
        [3] = "GlobalAll",
        [4] = "EntityOwner",
        [5] = "Peers"
    };

    static string GetMessageName(byte code)
    {
        if (MessageNames.TryGetValue(code, out var name)) return name;
        if (code <= 149) return $"ClientRPC({code})";
        if (code <= 241) return $"ForwardMsg({code})";
        return $"Unknown({code})";
    }

    public DirectRelayServer(int port)
    {
        _port = port;
        
        // Load configuration file with defaults
        _config = LoadConfigInstance();
        
        // NetManager config matched EXACTLY to ReadyMP's relay server settings:
        //   AutoRecycle = true       — auto-recycle NetPacketReader after handler
        //   EnableStatistics = true  — bandwidth statistics enabled
        //   UnsyncedEvents = true    — fire events on receive thread immediately
        //   PacketLayerBase = null   — NO encryption layer (constructor default)
        //   DisconnectTimeout = 5000 — ReadyMP Constants.ClientConnectionTimeoutMs
        //   UpdateTime = 5           — ReadyMP Constants.ServerNetworkTickRateMs
        _net = new NetManager(this)
        {
            AutoRecycle = true,
            EnableStatistics = _config.Performance.EnableStatistics,
            UnsyncedEvents = true,
            DisconnectTimeout = _config.Network.DisconnectTimeoutMs,
            UpdateTime = _config.Network.UpdateTimeMs,
            IPv6Enabled = false,
            UnconnectedMessagesEnabled = true
        };
    }

    /// <summary>
    /// Pre-populate a named blob in the relay's storage so it's available
    /// for download before any client uploads it. This matches the MP Launcher
    /// flow where the API server already has world.sav available.
    /// </summary>
    public void PreSeedBlob(string name, byte[] data)
    {
        _blobs[name] = data;
        Log("BLOB", $"Pre-seeded blob '{name}' ({FormatBytes(data.Length)})", ConsoleColor.Cyan);
    }
    
    RelayConfig LoadConfigInstance()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, "relay_config.json");
        
        if (File.Exists(configPath))
        {
            try
            {
                string json = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<RelayConfig>(json, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
                if (config != null)
                {
                    Log("CONFIG", $"Loaded configuration from {configPath}", ConsoleColor.Cyan);
                    return config;
                }
            }
            catch (Exception ex)
            {
                Log("CONFIG", $"Failed to load config: {ex.Message}. Using defaults.", ConsoleColor.Yellow);
            }
        }
        
        Log("CONFIG", "Using default configuration", ConsoleColor.Cyan);
        return new RelayConfig();
    }

    public async Task RunAsync(CancellationToken ct)
    {
        _startTime = DateTime.Now;
        _lastStatusTime = _startTime;

        // Performance optimizations: Set high thread and process priorities
        if (_config.Performance.HighPriorityThread)
        {
            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Log("PERF", "Thread priority set to Highest", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                Log("PERF", $"Failed to set thread priority: {ex.Message}", ConsoleColor.Yellow);
            }
        }
        
        if (_config.Performance.HighPriorityProcess)
        {
            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                Log("PERF", "Process priority set to High", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                Log("PERF", $"Failed to set process priority: {ex.Message}", ConsoleColor.Yellow);
            }
        }

        bool started = _net.Start(_port);
        if (!started)
        {
            Log("ERROR", $"FAILED to start listener on port {_port}! Is the port already in use?", ConsoleColor.Red);
            Log("ERROR", $"Try: netstat -an | findstr {_port}", ConsoleColor.Red);
            return;
        }

        // NOTE: Socket buffer optimization would require accessing internal NetManager socket
        // LiteNetLib's NetManager doesn't expose socket directly after Start()
        // The buffers are already set to reasonable defaults by LiteNetLib

        Log("NETWORK", $"UDP socket bound on port {_port} - ready for connections", ConsoleColor.Green);
        Log("NETWORK", $"Diagnostic echo enabled — clients can test connectivity before connecting", ConsoleColor.Cyan);
        Log("NETWORK", $"Timeout: {_net.DisconnectTimeout}ms | PollRate: {_net.UpdateTime}ms | Stats: {_net.EnableStatistics}", ConsoleColor.Cyan);

        // === External port reachability test ===
        await Program.TestExternalPortReachability(_port);

        Log("STATUS", "Waiting for incoming connections...", ConsoleColor.DarkYellow);
        Log("STATUS", "Tell your friend to run DirectRelayConnect.exe and enter your IP address", ConsoleColor.DarkYellow);
        Log("STATUS", $"Event processing mode: {((_net.UnsyncedEvents) ? "UNSYNC (immediate)" : "SYNC (batched)")}", ConsoleColor.Cyan);

        while (!ct.IsCancellationRequested)
        {
            _net.PollEvents();

            // Periodic status heartbeat every 10 seconds
            if ((DateTime.Now - _lastStatusTime).TotalSeconds >= 10)
            {
                PrintStatus();
                _lastStatusTime = DateTime.Now;
            }

            // ReadyMP server tick rate: Constants.ServerNetworkTickRateMs = 5
            await Task.Delay(_config.Network.UpdateTimeMs, ct).ConfigureAwait(false);
        }

        Log("SHUTDOWN", $"Disconnecting {_byPlayerId.Count} player(s)...", ConsoleColor.Yellow);
        _net.DisconnectAll();
        _net.Stop();
        PrintFinalStats();
    }

    void PrintStatus()
    {
        var uptime = DateTime.Now - _startTime;
        int peers = _byPlayerId.Count;
        int areas = _areas.Count;

        if (peers == 0)
        {
            Log("HEARTBEAT", $"Uptime {uptime:hh\\:mm\\:ss} | No connections | Attempts: {_totalConnectionAttempts} | Errors: {_totalErrors}", ConsoleColor.DarkGray);
            
            // Detect the "ghost packet" issue: LiteNetLib stats show bytes received but no connections
            if (_net.EnableStatistics && _net.Statistics != null)
            {
                var s = _net.Statistics;
                if (s.PacketsReceived > 0 && _totalConnectionAttempts == 0 && !_warnedProtocolMismatch)
                {
                    _warnedProtocolMismatch = true;
                    Log("WARNING", "*** PACKETS RECEIVED BUT NO CONNECTION ATTEMPTS ***", ConsoleColor.Red);
                    Log("WARNING", $"  LiteNetLib received {s.PacketsReceived} raw packet(s) ({FormatBytes(s.BytesReceived)})", ConsoleColor.Red);
                    Log("WARNING", $"  but OnConnectionRequest was never triggered.", ConsoleColor.Red);
                    Log("WARNING", $"  This likely means a LiteNetLib PROTOCOL VERSION MISMATCH.", ConsoleColor.Yellow);
                    Log("WARNING", $"  The game's mod is using a different LiteNetLib version than this relay.", ConsoleColor.Yellow);
                    Log("WARNING", $"  This relay uses LiteNetLib 1.3.1 (ProtocolId may differ from game).", ConsoleColor.Yellow);
                    Log("WARNING", $"  Raw packet could also be a DirectRelayConnect diagnostic ping.", ConsoleColor.DarkCyan);
                }
            }
        }
        else
        {
            Log("HEARTBEAT", $"Uptime {uptime:hh\\:mm\\:ss} | Players: {peers} | Areas: {areas} | " +
                $"Pkts In: {_totalPacketsReceived} Out: {_totalPacketsSent} | " +
                $"Bytes In: {FormatBytes(_totalBytesReceived)} Out: {FormatBytes(_totalBytesSent)}", ConsoleColor.Cyan);

            foreach (var p in _byPlayerId.Values)
            {
                var dur = DateTime.Now - p.ConnectedAt;
                Log("  PLAYER", $"ID={p.PlayerId} GUID={p.UserGuid:N} EP={p.Peer} " +
                    $"Area={p.CurrentArea ?? "(none)"} Connected={dur:mm\\:ss} " +
                    $"Ping={p.Peer.Ping}ms", ConsoleColor.DarkCyan);
            }
        }

        // Print LiteNetLib statistics if available
        if (_net.EnableStatistics && _net.Statistics != null)
        {
            var s = _net.Statistics;
            Log("NETSTATS", $"LiteNet: Sent={s.PacketsSent} Recv={s.PacketsReceived} " +
                $"BytesSent={FormatBytes(s.BytesSent)} BytesRecv={FormatBytes(s.BytesReceived)} " +
                $"Loss={s.PacketLoss}", ConsoleColor.DarkGray);
        }
    }

    void PrintFinalStats()
    {
        var uptime = DateTime.Now - _startTime;
        Console.WriteLine();
        Log("FINAL", "=== Session Summary ===", ConsoleColor.White);
        Log("FINAL", $"  Uptime              : {uptime:hh\\:mm\\:ss}", ConsoleColor.White);
        Log("FINAL", $"  Connection attempts  : {_totalConnectionAttempts}", ConsoleColor.White);
        Log("FINAL", $"  Accepted             : {_totalConnectionsAccepted}", ConsoleColor.White);
        Log("FINAL", $"  Rejected             : {_totalConnectionsRejected}", ConsoleColor.White);
        Log("FINAL", $"  Packets received     : {_totalPacketsReceived}", ConsoleColor.White);
        Log("FINAL", $"  Packets sent         : {_totalPacketsSent}", ConsoleColor.White);
        Log("FINAL", $"  Bytes received       : {FormatBytes(_totalBytesReceived)}", ConsoleColor.White);
        Log("FINAL", $"  Bytes sent           : {FormatBytes(_totalBytesSent)}", ConsoleColor.White);
        Log("FINAL", $"  Network errors       : {_totalErrors}", ConsoleColor.White);
        Log("FINAL", "========================", ConsoleColor.White);
    }

    static string FormatBytes(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        return $"{bytes / (1024.0 * 1024.0):F2} MB";
    }

    // === INetEventListener ===

    private string GetIdModeName(byte idMode) => idMode switch
    {
        0 => "Auto",
        1 => "MinId",
        2 => "ExactId",
        _ => $"Unknown({idMode})"
    };

    public void OnConnectionRequest(ConnectionRequest request)
    {
        Interlocked.Increment(ref _totalConnectionAttempts);
        Log("CONNECT", $">>> Incoming connection from {request.RemoteEndPoint}", ConsoleColor.Yellow);

        try
        {
            var reader = request.Data;
            int dataLen = reader.AvailableBytes;
            Log("CONNECT", $"    Data available: {dataLen} bytes", ConsoleColor.DarkCyan);

            // CHANGED: Accept connections even without handshake data
            Guid userGuid;
            byte idMode = 0;  // Default: Auto
            ushort reqId = 0;

            if (dataLen > 0)
            {
                try
                {
                    // Parse handshake if data is present
                    string guidStr = reader.GetString();
                    idMode = reader.GetByte();
                    reqId = reader.GetUShort();
                    userGuid = Guid.Parse(guidStr);
                    Log("CONNECT", $"    Parsed: GUID={userGuid:N} IdMode={GetIdModeName(idMode)} ReqId={reqId}", ConsoleColor.DarkCyan);
                }
                catch (Exception ex)
                {
                    Log("CONNECT", $"    Handshake parse failed ({ex.GetType().Name}), using generated GUID", ConsoleColor.Yellow);
                    userGuid = Guid.NewGuid();
                    idMode = 0;
                    reqId = 0;
                }
            }
            else
            {
                Log("CONNECT", $"    No handshake data, generating temporary GUID", ConsoleColor.DarkCyan);
                userGuid = Guid.NewGuid();
            }

            lock (_lock)
            {
                ushort id;
                if (idMode == 2)      // ExactId
                    id = reqId;
                else if (idMode == 1) // MinId
                    id = Math.Max(_nextPlayerId, reqId);
                else                  // Auto
                    id = _nextPlayerId;

                while (_byPlayerId.ContainsKey(id))
                    id++;
                if (id >= _nextPlayerId)
                    _nextPlayerId = (ushort)(id + 1);

                Log("CONNECT", $"    Assigning PlayerId={id}...", ConsoleColor.DarkCyan);
                var peer = request.Accept();
                Log("CONNECT", $"    Peer registered: NetPeerId={peer.Id}", ConsoleColor.DarkCyan);
                
                var info = new PlayerInfo
                {
                    Peer = peer,
                    PlayerId = id,
                    UserGuid = userGuid,
                    ConnectedAt = DateTime.Now
                };
                _byPeerId[peer.Id] = info;
                _byPlayerId[id] = info;

                Interlocked.Increment(ref _totalConnectionsAccepted);
                Log("CONNECT", $"    ✓ CONNECTED: PlayerId={id} | Peer {peer.Id} | GUID={userGuid:N}", ConsoleColor.Green);

                // Send HandshakeConnected
                var w = new NetDataWriter();
                w.Put(MSG_HandshakeConnected);
                w.Put(id);
                w.Put(_nextNetworkId);
                _nextNetworkId += 1000;

                var others = _byPlayerId.Values.Where(x => x.PlayerId != id).ToList();
                w.Put(others.Count);
                foreach (var o in others)
                    w.Put(o.PlayerId);

                peer.Send(w, DeliveryMethod.ReliableOrdered);
                TrackSend(info, w.Length);
                Log("SEND", $"    >> HandshakeConnected: {w.Length}B to PlayerId={id} (base={_nextNetworkId - 1000}, {others.Count} others)", ConsoleColor.DarkGreen);

                // Tell existing players about the new one
                BroadcastPlayerConnection(id, true);
            }
        }
        catch (Exception ex)
        {
            Interlocked.Increment(ref _totalConnectionsRejected);
            Log("ERROR", $"Connection from {request.RemoteEndPoint} failed: {ex.GetType().Name}: {ex.Message}", ConsoleColor.Red);
            Log("DEBUG", $"Stack: {ex.StackTrace?.Split('\n').FirstOrDefault() ?? "(unknown)"}", ConsoleColor.DarkRed);
            try
            {
                request.Reject();
            }
            catch { }
        }
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Log("PEER", $"Peer connected callback: {peer} (Id={peer.Id})", ConsoleColor.Green);
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo info)
    {
        lock (_lock)
        {
            if (!_byPeerId.TryRemove(peer.Id, out var pi))
            {
                Log("DISCONN", $"Unknown peer disconnected: {peer} Reason={info.Reason}", ConsoleColor.DarkYellow);
                return;
            }

            var duration = DateTime.Now - pi.ConnectedAt;
            Log("DISCONN", $"Player {pi.PlayerId} disconnected: Reason={info.Reason} " +
                $"Duration={duration:mm\\:ss} Pkts(in={pi.PacketsReceived}/out={pi.PacketsSent})", ConsoleColor.Yellow);

            if (info.Reason == DisconnectReason.Timeout)
                Log("DISCONN", $"    *** TIMEOUT - player may have crashed or lost connection", ConsoleColor.Red);
            else if (info.Reason == DisconnectReason.RemoteConnectionClose)
                Log("DISCONN", $"    Player closed their connection normally", ConsoleColor.DarkGreen);

            if (pi.CurrentArea != null)
                DoLeaveArea(pi);

            _byPlayerId.TryRemove(pi.PlayerId, out _);
            BroadcastPlayerConnection(pi.PlayerId, false);

            Log("STATUS", $"Players remaining: {_byPlayerId.Count}", ConsoleColor.Cyan);
        }
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod dm)
    {
        // Atomic counters - no lock needed
        Interlocked.Increment(ref _totalPacketsReceived);
        Interlocked.Add(ref _totalBytesReceived, reader.AvailableBytes);

        if (reader.AvailableBytes == 0)
        {
#if DIAGNOSTIC_MODE
            Log("RECV", $"Empty packet from Peer {peer.Id}", ConsoleColor.DarkYellow);
#endif
            return;
        }

        byte code = reader.GetByte();

        // Lock-free lookup using ConcurrentDictionary
        if (!_byPeerId.TryGetValue(peer.Id, out var sender))
        {
            Log("RECV", $"Packet from unknown peer {peer.Id}! Code={GetMessageName(code)}", ConsoleColor.Red);
            return;
        }

        // Atomic counter increment - no lock needed
        Interlocked.Increment(ref sender.PacketsReceived);

#if DIAGNOSTIC_MODE
        // Detailed packet logging only in diagnostic mode
        Log("RECV", $"Player {sender.PlayerId}: {GetMessageName(code)} ({reader.AvailableBytes}B) dm={dm}", ConsoleColor.Gray);
#endif

        // Process packet without lock - safe for 2-player forwarding
        switch (code)
        {
            case MSG_RequestAreaEvent:
                HandleAreaRequest(sender, reader);
                break;
            case MSG_RequestUploadBlob:
                HandleUpload(sender, reader);
                break;
            case MSG_RequestDownloadBlob:
                HandleDownload(sender, reader);
                break;
            case MSG_EcsDelta:
            case MSG_EcsSnapshot:
            case MSG_EcsCreateEntity:
            case MSG_EcsDeleteEntity:
            case MSG_EcsChangeOwnership:
                ForwardToAreaOthers(sender, code, reader, dm);
                break;
            default:
                if (code <= 149)
                    HandleClientRpc(sender, code, reader, dm);
                else if (code >= 150 && code <= 241)
                    ForwardToAreaOthers(sender, code, reader, dm);
#if DIAGNOSTIC_MODE
                else
                    Log("RECV", $"    Unhandled message code {code} from Player {sender.PlayerId}", ConsoleColor.DarkYellow);
#endif
                break;
        }
    }

    public void OnNetworkError(IPEndPoint ep, SocketError err)
    {
        Interlocked.Increment(ref _totalErrors);
        Log("ERROR", $"Network error from {ep}: {err} ({(int)err})", ConsoleColor.Red);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint ep, NetPacketReader r, UnconnectedMessageType t)
    {
        // Diagnostic echo: respond to DIRECTRELAY_DIAG_PING with relay status
        if (r.AvailableBytes >= 4)
        {
            string? msg = null;
            try { msg = System.Text.Encoding.UTF8.GetString(r.RawData, r.Position, Math.Min(r.AvailableBytes, 64)); } catch { }
            
            if (msg != null && msg.StartsWith("DIRECTRELAY_DIAG"))
            {
                Log("DIAG", $"Diagnostic ping from {ep}", ConsoleColor.Cyan);
                var uptime = DateTime.Now - _startTime;
                var response = System.Text.Encoding.UTF8.GetBytes(
                    $"DIRECTRELAY_DIAG_PONG|" +
                    $"players={_byPlayerId.Count}|" +
                    $"areas={_areas.Count}|" +
                    $"uptime={uptime:hh\\:mm\\:ss}|" +
                    $"port={_port}|" +
                    $"version=1.0|" +
                    $"pkts_in={_totalPacketsReceived}|" +
                    $"pkts_out={_totalPacketsSent}");
                var writer = new NetDataWriter();
                writer.Put(response);
                _net.SendUnconnectedMessage(writer, ep);
                Log("DIAG", $"Diagnostic pong sent to {ep}", ConsoleColor.Green);
                return;
            }
        }
        Log("UNCON", $"Unconnected message from {ep}: Type={t} ({r.AvailableBytes}B)", ConsoleColor.DarkYellow);
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        // Only log significant latency changes (>200ms)
        if (latency > 200)
        {
            lock (_lock)
            {
                if (_byPeerId.TryGetValue(peer.Id, out var pi))
                    Log("LATENCY", $"Player {pi.PlayerId}: {latency}ms (HIGH)", ConsoleColor.Red);
            }
        }
    }

    // === Protocol Handlers ===

    void HandleAreaRequest(PlayerInfo sender, NetDataReader reader)
    {
        ushort pid = reader.GetUShort();  // PlayerId
        bool join = reader.GetBool();

        if (join)
        {
            ushort areaVal = reader.GetUShort();  // AreaId
            string key = areaVal.ToString();

            if (sender.CurrentArea != null)
            {
                Log("AREA", $"Player {sender.PlayerId} switching areas: '{sender.CurrentArea}' -> '{key}'", ConsoleColor.Magenta);
                DoLeaveArea(sender);
            }

            if (!_areas.TryGetValue(key, out var area))
            {
                area = new AreaInfo { Id = key };
                _areas[key] = area;
                Log("AREA", $"Area '{key}' CREATED (first player joining)", ConsoleColor.Green);
            }

            area.Players.Add(sender);
            sender.CurrentArea = key;

            // Send join confirmation
            var w = new NetDataWriter();
            w.Put(MSG_AreaEvent);
            w.Put(sender.PlayerId);
            w.Put(true);
            w.Put(areaVal);

            var others = area.Players.Where(x => x.PlayerId != sender.PlayerId).ToList();
            w.Put((ushort)others.Count);
            foreach (var o in others)
                w.Put(o.PlayerId);

            sender.Peer.Send(w, DeliveryMethod.ReliableOrdered);
            TrackSend(sender, w.Length);
            BroadcastAreaEvent(sender.PlayerId, true, area);

            Log("AREA", $"Player {sender.PlayerId} JOINED area '{key}' -> {area.Players.Count} player(s) in area " +
                $"[{string.Join(", ", area.Players.Select(x => x.PlayerId))}]", ConsoleColor.Green);
        }
        else
        {
            Log("AREA", $"Player {sender.PlayerId} requesting to LEAVE area '{sender.CurrentArea}'", ConsoleColor.Magenta);
            DoLeaveArea(sender);
        }
    }

    void DoLeaveArea(PlayerInfo pi)
    {
        if (pi.CurrentArea == null) return;
        if (!_areas.TryGetValue(pi.CurrentArea, out var area)) return;

        area.Players.Remove(pi);
        BroadcastAreaEvent(pi.PlayerId, false, area);

        var w = new NetDataWriter();
        w.Put(MSG_AreaEvent);
        w.Put(pi.PlayerId);
        w.Put(false);
        pi.Peer.Send(w, DeliveryMethod.ReliableOrdered);
        TrackSend(pi, w.Length);

        Log("AREA", $"Player {pi.PlayerId} LEFT area '{pi.CurrentArea}' ({area.Players.Count} remain)", ConsoleColor.Yellow);

        if (area.Players.Count == 0)
        {
            _areas.Remove(pi.CurrentArea);
            Log("AREA", $"Area '{pi.CurrentArea}' REMOVED (empty)", ConsoleColor.DarkYellow);
        }
        pi.CurrentArea = null;
    }

    void HandleUpload(PlayerInfo sender, NetDataReader reader)
    {
        int reqId = reader.GetInt();
        string name = reader.GetString();
        int len = reader.GetInt();
        byte[] data = new byte[len];
        reader.GetBytes(data, len);

        _blobs[name] = data;
        Log("BLOB", $"Player {sender.PlayerId} UPLOADED '{name}' ({FormatBytes(len)})", ConsoleColor.Cyan);

        var w = new NetDataWriter();
        w.Put(MSG_UploadBlobAck);
        w.Put(reqId);
        w.Put(true);
        sender.Peer.Send(w, DeliveryMethod.ReliableOrdered);
        TrackSend(sender, w.Length);
    }

    void HandleDownload(PlayerInfo sender, NetDataReader reader)
    {
        int reqId = reader.GetInt();
        string name = reader.GetString();

        var w = new NetDataWriter();
        w.Put(MSG_DownloadBlobData);
        w.Put(reqId);

        if (_blobs.TryGetValue(name, out var data))
        {
            w.Put(true);
            w.Put(name);
            w.Put(data.Length);
            w.Put(data);
            Log("BLOB", $"Player {sender.PlayerId} DOWNLOADED '{name}' ({FormatBytes(data.Length)})", ConsoleColor.Cyan);
        }
        else
        {
            w.Put(false);
            Log("BLOB", $"Player {sender.PlayerId} requested '{name}' - NOT FOUND", ConsoleColor.DarkYellow);
        }
        sender.Peer.Send(w, DeliveryMethod.ReliableOrdered);
        TrackSend(sender, w.Length);
    }

    void ForwardToAreaOthers(PlayerInfo sender, byte code, NetDataReader reader, DeliveryMethod dm)
    {
        if (sender.CurrentArea == null || !_areas.TryGetValue(sender.CurrentArea, out var area))
        {
#if DIAGNOSTIC_MODE
            Log("FORWARD", $"Player {sender.PlayerId} sent {GetMessageName(code)} but is NOT in any area - DROPPED", ConsoleColor.Red);
#endif
            return;
        }

        // Direct forwarding: copy the relevant portion of the packet and send
        int sent = 0;
        
        // Calculate the position of the code byte (reader has already read it)
        // reader.Position now points after the code byte, so we go back 1 byte to include it
        int codeByteOffset = reader.Position - 1;
        int totalLength = reader.AvailableBytes + 1; // Remaining bytes + the code byte

        // Copy the relevant portion so we can reuse it for all recipients
        byte[] fwdBuf = new byte[totalLength];
        Buffer.BlockCopy(reader.RawData, codeByteOffset, fwdBuf, 0, totalLength);
        
        foreach (var p in area.Players)
        {
            if (p.PlayerId != sender.PlayerId)
            {
                p.Peer.Send(fwdBuf, dm);
                TrackSend(p, totalLength);
                sent++;
            }
        }

#if DIAGNOSTIC_MODE
        // Only log ECS messages at high volume as a single line
        if (code >= 246 && code <= 250)
            Log("FORWARD", $"Player {sender.PlayerId} -> {GetMessageName(code)} to {sent} peer(s) in area '{sender.CurrentArea}' ({reader.AvailableBytes}B) [zero-copy]", ConsoleColor.DarkGray);
        else
            Log("FORWARD", $"Player {sender.PlayerId} -> {GetMessageName(code)} to {sent} peer(s) in area '{sender.CurrentArea}' [zero-copy]", ConsoleColor.Gray);
#endif
    }

    void HandleClientRpc(PlayerInfo sender, byte code, NetDataReader reader, DeliveryMethod dm)
    {
        if (reader.AvailableBytes < 3)
        {
#if DIAGNOSTIC_MODE
            Log("RPC", $"Player {sender.PlayerId}: RPC code={code} too small ({reader.AvailableBytes}B) - DROPPED", ConsoleColor.Red);
#endif
            return;
        }

        int hdrStart = reader.Position;
        // CustomRelayEventHeader wire format: [ushort senderPlayerId] [byte relayMode] [optional peers]
        // CRITICAL: senderId comes BEFORE mode — was previously swapped!
        ushort senderId = reader.GetUShort();
        byte mode = reader.GetByte();

        ushort[]? targets = null;
        if (mode == MODE_Peers)
        {
            ushort cnt = reader.GetUShort();
            targets = new ushort[cnt];
            for (int i = 0; i < cnt; i++)
                targets[i] = reader.GetUShort();
        }

#if DIAGNOSTIC_MODE
        string modeName = ModeNames.TryGetValue(mode, out var mn) ? mn : $"Unknown({mode})";
        Log("RPC", $"Player {sender.PlayerId}: RPC({code}) sender={senderId} mode={modeName} " +
            (targets != null ? $"targets=[{string.Join(",", targets)}]" : "") +
            $" ({reader.AvailableBytes}B payload)", ConsoleColor.DarkCyan);
#endif

        // Reconstruct the complete original message for forwarding:
        // [byte eventCode] + [everything from hdrStart to end] = original wire format
        var w = NetDataWriterPool.Rent();
        try
        {
            w.Put(code);
            w.Put(reader.RawData, hdrStart, reader.AvailableBytes + (reader.Position - hdrStart));

            switch (mode)
            {
                case MODE_AreaOthers:
                    SendAreaOthers(sender, w, dm);
                    break;
                case MODE_AreaAll:
                    SendAreaAll(sender, w, dm);
                    break;
                case MODE_GlobalOthers:
                    SendAllOthers(sender, w, dm);
                    break;
                case MODE_GlobalAll:
                    SendAll(w, dm);
                    break;
                case MODE_Peers:
                    if (targets != null)
                        SendToPeers(targets, w, dm);
                    break;
                case MODE_EntityOwner:
                    SendAreaOthers(sender, w, dm);
                    break;
            }
        }
        finally
        {
            // Return writer to pool for reuse
            NetDataWriterPool.Return(w);
        }
    }

    // === Broadcast helpers ===

    void BroadcastPlayerConnection(ushort pid, bool connected)
    {
        var w = new NetDataWriter();
        w.Put(MSG_OtherPlayerConnectionEvent);
        w.Put(pid);
        w.Put(connected);
        int sent = 0;
        foreach (var p in _byPlayerId.Values)
        {
            if (p.PlayerId != pid)
            {
                p.Peer.Send(w, DeliveryMethod.ReliableOrdered);
                TrackSend(p, w.Length);
                sent++;
            }
        }
        Log("BROADCAST", $"PlayerConnection: ID={pid} Connected={connected} -> notified {sent} player(s)", ConsoleColor.DarkCyan);
    }

    void BroadcastAreaEvent(ushort pid, bool joined, AreaInfo area)
    {
        var w = new NetDataWriter();
        w.Put(MSG_OtherPlayerAreaEvent);
        w.Put(pid);
        w.Put(joined);
        int sent = 0;
        foreach (var p in area.Players)
        {
            if (p.PlayerId != pid)
            {
                p.Peer.Send(w, DeliveryMethod.ReliableOrdered);
                TrackSend(p, w.Length);
                sent++;
            }
        }
    }

    // === Send helpers ===

    void SendAreaOthers(PlayerInfo s, NetDataWriter w, DeliveryMethod dm)
    {
        if (s.CurrentArea != null && _areas.TryGetValue(s.CurrentArea, out var a))
            foreach (var p in a.Players)
                if (p.PlayerId != s.PlayerId)
                {
                    p.Peer.Send(w, dm);
                    TrackSend(p, w.Length);
                }
    }

    void SendAreaAll(PlayerInfo s, NetDataWriter w, DeliveryMethod dm)
    {
        if (s.CurrentArea != null && _areas.TryGetValue(s.CurrentArea, out var a))
            foreach (var p in a.Players)
            {
                p.Peer.Send(w, dm);
                TrackSend(p, w.Length);
            }
    }

    void SendAllOthers(PlayerInfo s, NetDataWriter w, DeliveryMethod dm)
    {
        foreach (var p in _byPlayerId.Values)
            if (p.PlayerId != s.PlayerId)
            {
                p.Peer.Send(w, dm);
                TrackSend(p, w.Length);
            }
    }

    void SendAll(NetDataWriter w, DeliveryMethod dm)
    {
        foreach (var p in _byPlayerId.Values)
        {
            p.Peer.Send(w, dm);
            TrackSend(p, w.Length);
        }
    }

    void SendToPeers(ushort[] ids, NetDataWriter w, DeliveryMethod dm)
    {
        foreach (var id in ids)
        {
            if (_byPlayerId.TryGetValue(id, out var p))
            {
                p.Peer.Send(w, dm);
                TrackSend(p, w.Length);
            }
            else
            {
                Log("SEND", $"Target player {id} not found - message dropped", ConsoleColor.Red);
            }
        }
    }

    void TrackSend(PlayerInfo p, int bytes)
    {
        Interlocked.Increment(ref p.PacketsSent);
        Interlocked.Increment(ref _totalPacketsSent);
        Interlocked.Add(ref _totalBytesSent, bytes);
    }

    void Log(string tag, string msg, ConsoleColor color = ConsoleColor.Gray) =>
        Program.Log(tag, msg, color);
}
