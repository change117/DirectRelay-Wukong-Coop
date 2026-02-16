using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    public int UpdateTimeMs { get; set; } = 1;
    public int DisconnectTimeoutMs { get; set; } = 30000;
    public int SendBufferSizeKB { get; set; } = 1024;
    public int ReceiveBufferSizeKB { get; set; } = 1024;
    public int MTU { get; set; } = 1400;
}

class PerformanceConfig
{
    public bool HighPriorityThread { get; set; } = true;
    public bool HighPriorityProcess { get; set; } = true;
    public bool EnableStatistics { get; set; } = false;
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

    [STAThread]
    static async Task Main(string[] args)
    {
        // Ensure console is visible
        if (!AttachConsole(-1))
        {
            AllocConsole();
        }

        int port = 7777;
        if (args.Length > 0 && int.TryParse(args[0], out int p))
            port = p;

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

        // === AUTO-SETUP: Install mods + write handshake for the HOST ===
        AutoSetupHost(port);

        var server = new DirectRelayServer(port);
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

        Log("SHUTDOWN", "Server stopped.", ConsoleColor.Yellow);
    }

    static void AutoSetupHost(int port)
    {
        Log("SETUP", "=== Auto-Setup: Preparing host machine ===", ConsoleColor.Cyan);

        // --- Find the game ---
        string? gamePath = FindGameExe();
        if (gamePath == null)
        {
            Log("SETUP", "Could not find Black Myth: Wukong installation.", ConsoleColor.DarkYellow);
            Log("SETUP", "Mod auto-install skipped. Server will run relay-only.", ConsoleColor.DarkYellow);
            Console.WriteLine();
            return;
        }

        string gameDir = Path.GetDirectoryName(gamePath)!;
        Log("SETUP", $"Game found: {gamePath}", ConsoleColor.Green);

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
            int copied = 0;
            int skipped = 0;

            foreach (var srcFile in Directory.GetFiles(modsSourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(modsSourceDir, srcFile);
                if (relativePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    continue;

                string destFile = Path.Combine(gameDir, relativePath);
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
                        Log("SETUP", $"  [COPY] {relativePath}", ConsoleColor.Green);
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

        int missing = criticalMods.Count(m => !File.Exists(Path.Combine(gameDir, m)));
        if (missing > 0)
        {
            Log("SETUP", $"{missing} critical mod file(s) missing from {gameDir}", ConsoleColor.Red);
            Log("SETUP", "Your game will not connect to this relay without them.", ConsoleColor.Red);
        }
        else
        {
            Log("SETUP", "All 7 critical mod files verified in game directory.", ConsoleColor.Green);
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
            "API_BASE_URL=http://localhost",
            "JWT_TOKEN=direct-relay"
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
                WorkingDirectory = gameDir,
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
        
#if DIAGNOSTIC_MODE
        // In diagnostic mode, enable statistics
        _net = new NetManager(this)
        {
            AutoRecycle = true,
            EnableStatistics = true,
            UnsyncedEvents = false,
            DisconnectTimeout = _config.Network.DisconnectTimeoutMs,
            UpdateTime = _config.Network.UpdateTimeMs,
            IPv6Enabled = false,
            UnconnectedMessagesEnabled = false,
            NatPunchEnabled = false
        };
#else
        // In production mode, disable statistics for maximum performance
        _net = new NetManager(this)
        {
            AutoRecycle = true,
            EnableStatistics = false, // No statistics overhead in production
            UnsyncedEvents = false,
            DisconnectTimeout = _config.Network.DisconnectTimeoutMs,
            UpdateTime = _config.Network.UpdateTimeMs, // 1ms for ultra-low latency
            IPv6Enabled = false,
            UnconnectedMessagesEnabled = false,
            NatPunchEnabled = false
        };
#endif
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
#if DIAGNOSTIC_MODE
        Log("NETWORK", $"Timeout: {_net.DisconnectTimeout}ms | PollRate: {_net.UpdateTime}ms | Stats: {_net.EnableStatistics}", ConsoleColor.Cyan);
#else
        Log("NETWORK", $"Timeout: {_net.DisconnectTimeout}ms | PollRate: {_net.UpdateTime}ms | Production Mode", ConsoleColor.Cyan);
#endif
        Log("STATUS", "Waiting for incoming connections...", ConsoleColor.DarkYellow);

        while (!ct.IsCancellationRequested)
        {
            _net.PollEvents();

            // Periodic status heartbeat every 10 seconds
            if ((DateTime.Now - _lastStatusTime).TotalSeconds >= 10)
            {
                PrintStatus();
                _lastStatusTime = DateTime.Now;
            }

            await Task.Delay(2, ct).ConfigureAwait(false);
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
                $"PacketLoss={s.PacketLoss}", ConsoleColor.DarkGray);
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

    public void OnConnectionRequest(ConnectionRequest request)
    {
        Interlocked.Increment(ref _totalConnectionAttempts);
        Log("CONNECT", $">>> Incoming connection request from {request.RemoteEndPoint}", ConsoleColor.Yellow);

        try
        {
            var reader = request.Data;
            int dataLen = reader.AvailableBytes;
            Log("CONNECT", $"    Handshake data: {dataLen} bytes", ConsoleColor.Yellow);

            if (dataLen == 0)
            {
                Interlocked.Increment(ref _totalConnectionsRejected);
                Log("REJECT", $"    REJECTED: Empty handshake data from {request.RemoteEndPoint}", ConsoleColor.Red);
                request.Reject();
                return;
            }

            string guidStr = reader.GetString();
            byte idMode = reader.GetByte();
            ushort reqId = reader.GetUShort();
            Guid userGuid = Guid.Parse(guidStr);

            string modeStr = idMode switch
            {
                0 => "Auto",
                1 => "MinId",
                2 => "ExactId",
                _ => $"Unknown({idMode})"
            };

            Log("CONNECT", $"    GUID={userGuid}  IdMode={modeStr}  RequestedId={reqId}", ConsoleColor.Yellow);

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

                var peer = request.Accept();
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
                Log("CONNECT", $"    ACCEPTED: Player ID={id}  Peer={peer}  Total players now: {_byPlayerId.Count}", ConsoleColor.Green);

                // Send HandshakeConnected
                var w = new NetDataWriter();
                w.Put(MSG_HandshakeConnected);
                w.Put(id);                   // PlayerId (ushort)
                w.Put(_nextNetworkId);       // next NetworkId (uint)
                _nextNetworkId += 1000;

                var others = _byPlayerId.Values.Where(x => x.PlayerId != id).ToList();
                w.Put(others.Count);         // int
                foreach (var o in others)
                    w.Put(o.PlayerId);       // ushort

                peer.Send(w, DeliveryMethod.ReliableOrdered);
                TrackSend(info, w.Length);
                Log("SEND", $"    -> HandshakeConnected to Player {id}: assigned NetworkId base={_nextNetworkId - 1000}, {others.Count} existing player(s)", ConsoleColor.DarkGreen);

                // Tell existing players about the new one
                BroadcastPlayerConnection(id, true);
            }
        }
        catch (Exception ex)
        {
            Interlocked.Increment(ref _totalConnectionsRejected);
            Log("REJECT", $"    REJECTED from {request.RemoteEndPoint}: {ex.GetType().Name}: {ex.Message}", ConsoleColor.Red);
            request.Reject();
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

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod dm)
    {
        // Atomic counters - no lock needed
        Interlocked.Increment(ref _totalPacketsReceived);
        Interlocked.Add(ref _totalBytesReceived, reader.AvailableBytes);

        if (reader.AvailableBytes == 0)
        {
#if DIAGNOSTIC_MODE
            Log("RECV", $"Empty packet from Peer {peer.Id} on ch={channel}", ConsoleColor.DarkYellow);
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
        Log("RECV", $"Player {sender.PlayerId}: {GetMessageName(code)} ({reader.AvailableBytes}B) ch={channel} dm={dm}", ConsoleColor.Gray);
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

        // OPTIMIZATION: For 2-player co-op, use zero-copy direct forwarding
        // This avoids allocating NetDataWriter and copying buffer data
        int sent = 0;
        
        // Calculate the position of the code byte (reader has already read it)
        // reader.Position now points after the code byte, so we go back 1 byte to include it
        int codeByteOffset = reader.Position - 1;
        int totalLength = reader.AvailableBytes + 1; // Remaining bytes + the code byte
        
        foreach (var p in area.Players)
        {
            if (p.PlayerId != sender.PlayerId)
            {
                // Zero-copy: Send raw buffer directly from the code byte position
                p.Peer.Send(reader.RawData, codeByteOffset, totalLength, dm);
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
        byte mode = reader.GetByte();
        ushort senderId = reader.GetUShort();

#if DIAGNOSTIC_MODE
        string modeName = ModeNames.TryGetValue(mode, out var mn) ? mn : $"Unknown({mode})";
#endif

        ushort[]? targets = null;
        if (mode == MODE_Peers)
        {
            ushort cnt = reader.GetUShort();
            targets = new ushort[cnt];
            for (int i = 0; i < cnt; i++)
                targets[i] = reader.GetUShort();
        }

#if DIAGNOSTIC_MODE
        Log("RPC", $"Player {sender.PlayerId}: RPC({code}) mode={modeName} " +
            (targets != null ? $"targets=[{string.Join(",", targets)}]" : "") +
            $" ({reader.AvailableBytes}B)", ConsoleColor.DarkCyan);
#endif

        // Use pooled writer to eliminate allocations
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
