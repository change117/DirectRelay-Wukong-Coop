using System;
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

namespace DirectRelayConnect;

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
            string logPath = Path.Combine(AppContext.BaseDirectory, $"directrelayconnect_{DateTime.Now:yyyyMMdd_HHmmss}.log");
            _logFile = new StreamWriter(logPath, append: false) { AutoFlush = true };
            _logFile.WriteLine($"=== DirectRelayConnect Diagnostic Log - {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
            _logFile.WriteLine($"Machine: {Environment.MachineName} | User: {Environment.UserName} | OS: {Environment.OSVersion}");
            _logFile.WriteLine();
        }
        catch { }

        Console.Title = "DirectRelay Connect [DIAG]";
        Console.WriteLine("========================================================");
        Console.WriteLine("   DirectRelay Connect for ReadyMP (Wukong Co-op) [DIAG] ");
        Console.WriteLine("========================================================");
        Console.WriteLine($"  Machine : {Environment.MachineName}");
        Console.WriteLine($"  User    : {Environment.UserName}");
        Console.WriteLine($"  Time    : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"  OS      : {Environment.OSVersion}");
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
        Log("GUI", "Waiting for user to configure and launch...", ConsoleColor.Yellow);
        Console.WriteLine();

        // Block until user clicks "Connect & Launch Game"
        launchSignal.WaitOne();
        
        Log("GUI", "Launch signal received from control panel", ConsoleColor.Green);
        Console.WriteLine();

        // --- Load / prompt for settings ---
        string settingsPath = Path.Combine(AppContext.BaseDirectory, "connect_settings.txt");
        string? savedIp = null;
        string? savedPort = null;
        string? savedGamePath = null;

        if (File.Exists(settingsPath))
        {
            Log("SETTINGS", $"Loading saved settings from: {settingsPath}", ConsoleColor.Cyan);
            foreach (var line in File.ReadAllLines(settingsPath))
            {
                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    switch (parts[0].Trim())
                    {
                        case "HOST_IP": savedIp = parts[1].Trim(); break;
                        case "HOST_PORT": savedPort = parts[1].Trim(); break;
                        case "GAME_PATH": savedGamePath = parts[1].Trim(); break;
                    }
                }
            }
            if (savedIp != null) Log("SETTINGS", $"  Saved IP   : {savedIp}", ConsoleColor.DarkCyan);
            if (savedPort != null) Log("SETTINGS", $"  Saved Port : {savedPort}", ConsoleColor.DarkCyan);
            if (savedGamePath != null) Log("SETTINGS", $"  Saved Game : {savedGamePath}", ConsoleColor.DarkCyan);
            Console.WriteLine();
        }

        // Host IP
        if (!string.IsNullOrEmpty(savedIp))
            Console.Write($"  Host IP [{savedIp}]: ");
        else
            Console.Write("  Host IP: ");

        string? ipInput = Console.ReadLine()?.Trim();
        string hostIp = string.IsNullOrEmpty(ipInput) ? (savedIp ?? "") : ipInput;

        if (string.IsNullOrEmpty(hostIp))
        {
            Log("ERROR", "Host IP is required! Cannot connect without a server IP.", ConsoleColor.Red);
            WaitForExit();
            return;
        }

        // Port
        string defaultPort = savedPort ?? "7777";
        Console.Write($"  Port [{defaultPort}]: ");
        string? portInput = Console.ReadLine()?.Trim();
        string port = string.IsNullOrEmpty(portInput) ? defaultPort : portInput;

        if (!int.TryParse(port, out int portNum))
        {
            Log("ERROR", $"Port must be a number, got: '{port}'", ConsoleColor.Red);
            WaitForExit();
            return;
        }
        Console.WriteLine();

        // === Auto-create Windows Firewall rule ===
        EnsureFirewallRule(portNum);

        // === STEP 1: Verify network connectivity to the relay server ===
        Log("NETWORK", $"Testing connectivity to {hostIp}:{port}...", ConsoleColor.Yellow);

        // DNS resolution check
        try
        {
            if (!IPAddress.TryParse(hostIp, out _))
            {
                Log("NETWORK", $"  Resolving hostname '{hostIp}'...", ConsoleColor.Yellow);
                var addresses = Dns.GetHostAddresses(hostIp);
                Log("NETWORK", $"  Resolved to: {string.Join(", ", addresses.Select(a => a.ToString()))}", ConsoleColor.Green);
            }
            else
            {
                Log("NETWORK", $"  IP address: {hostIp} (no DNS needed)", ConsoleColor.Green);
            }
        }
        catch (Exception ex)
        {
            Log("ERROR", $"  DNS resolution FAILED: {ex.Message}", ConsoleColor.Red);
            Log("ERROR", $"  Cannot resolve '{hostIp}' - check the IP/hostname", ConsoleColor.Red);
            WaitForExit();
            return;
        }

        // UDP reachability test — send LiteNetLib-formatted diagnostic ping
        Log("NETWORK", $"  Sending diagnostic ping to relay at {hostIp}:{portNum}...", ConsoleColor.Yellow);
        bool relayReachable = false;
        try
        {
            // Use LiteNetLib to send the diagnostic ping so it arrives as a proper
            // unconnected message that the relay's OnNetworkReceiveUnconnected can handle.
            // Raw UDP (UdpClient.Send) doesn't have the LiteNetLib packet header, so the
            // relay's LiteNetLib would silently drop it.
            var diagListener = new DiagPingListener();
            var diagNet = new NetManager(diagListener)
            {
                UnconnectedMessagesEnabled = true,
                AutoRecycle = true
            };
            diagNet.Start();
            Log("NETWORK", $"  LiteNetLib diagnostic socket started on port {diagNet.LocalPort}", ConsoleColor.DarkGray);

            var pingPayload = System.Text.Encoding.UTF8.GetBytes("DIRECTRELAY_DIAG_PING");
            var writer = new NetDataWriter();
            writer.Put(pingPayload);
            
            var targetEp = new IPEndPoint(IPAddress.Parse(hostIp), portNum);
            diagNet.SendUnconnectedMessage(writer, targetEp);
            Log("NETWORK", $"  Diagnostic ping sent (LiteNetLib format), waiting for relay response...", ConsoleColor.Yellow);

            // Poll for up to 3 seconds waiting for the pong
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < 3000 && !diagListener.GotPong)
            {
                diagNet.PollEvents();
                Thread.Sleep(10);
            }

            diagNet.Stop();

            if (diagListener.GotPong)
            {
                relayReachable = true;
                Log("NETWORK", $"  *** RELAY IS ALIVE AND RESPONDING! ***", ConsoleColor.Green);

                // Parse pong fields
                if (diagListener.PongData != null)
                {
                    var fields = diagListener.PongData.Split('|');
                    foreach (var field in fields)
                    {
                        if (field.StartsWith("players="))
                            Log("NETWORK", $"    Players connected: {field.Split('=')[1]}", ConsoleColor.Cyan);
                        else if (field.StartsWith("uptime="))
                            Log("NETWORK", $"    Relay uptime: {field.Split('=')[1]}", ConsoleColor.Cyan);
                        else if (field.StartsWith("areas="))
                            Log("NETWORK", $"    Active areas: {field.Split('=')[1]}", ConsoleColor.Cyan);
                        else if (field.StartsWith("pkts_in="))
                            Log("NETWORK", $"    Packets received: {field.Split('=')[1]}", ConsoleColor.DarkCyan);
                        else if (field.StartsWith("pkts_out="))
                            Log("NETWORK", $"    Packets sent: {field.Split('=')[1]}", ConsoleColor.DarkCyan);
                    }
                }
            }
            else
            {
                Log("WARNING", $"  *** NO RESPONSE FROM RELAY within 3 seconds ***", ConsoleColor.Red);
                Log("WARNING", $"  This usually means one of:", ConsoleColor.Red);
                Log("WARNING", $"    1. DirectRelay.exe is NOT running on the host machine", ConsoleColor.Yellow);
                Log("WARNING", $"    2. Wrong IP address (host's IP might be different)", ConsoleColor.Yellow);
                Log("WARNING", $"    3. Firewall on HOST is blocking inbound UDP {portNum}", ConsoleColor.Yellow);
                Log("WARNING", $"    4. Router/NAT is not forwarding port {portNum} to the host (if over internet)", ConsoleColor.Yellow);
                Log("WARNING", $"    5. An antivirus/security suite is blocking the traffic", ConsoleColor.Yellow);
                Log("WARNING", $"", ConsoleColor.Yellow);
                Log("WARNING", $"  Ask the HOST to:", ConsoleColor.Cyan);
                Log("WARNING", $"    - Verify DirectRelay console shows 'Waiting for connections...'", ConsoleColor.Cyan);
                Log("WARNING", $"    - Run: powershell -File diagnose.ps1 -FixFirewall", ConsoleColor.Cyan);
                Log("WARNING", $"    - Check router port forwarding for UDP {portNum}", ConsoleColor.Cyan);
                Console.WriteLine();
                Console.Write("  Continue anyway? (y/N): ");
                var answer = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (answer != "y" && answer != "yes")
                {
                    Log("STATUS", "Aborted by user", ConsoleColor.Yellow);
                    WaitForExit();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Log("WARNING", $"  UDP test failed: {ex.Message}", ConsoleColor.Red);
            Log("WARNING", $"  This may indicate a firewall or routing issue", ConsoleColor.Red);
        }

        if (relayReachable)
        {
            Log("NETWORK", $"  Network connectivity: VERIFIED", ConsoleColor.Green);
        }
        Console.WriteLine();

        // === STEP 2: Find the game executable ===
        Log("GAME", "Locating game executable...", ConsoleColor.Yellow);

        string? gamePath = savedGamePath;
        if (!string.IsNullOrEmpty(gamePath) && File.Exists(gamePath))
        {
            Log("GAME", $"  Using saved path: {gamePath}", ConsoleColor.Green);
        }
        else
        {
            if (!string.IsNullOrEmpty(savedGamePath))
                Log("GAME", $"  Saved path no longer valid: {savedGamePath}", ConsoleColor.DarkYellow);

            Log("GAME", "  Searching for game...", ConsoleColor.Yellow);
            gamePath = FindGameExe();
            if (gamePath == null)
            {
                Log("WARNING", "  Could not auto-detect game location!", ConsoleColor.Red);
                Log("WARNING", "  Searched:", ConsoleColor.DarkYellow);
                Log("WARNING", "    - Steam registry (HKLM + HKCU)", ConsoleColor.DarkYellow);
                Log("WARNING", "    - Steam libraryfolders.vdf", ConsoleColor.DarkYellow);
                Log("WARNING", "    - Common paths on drives C: D: E: F:", ConsoleColor.DarkYellow);
                Console.WriteLine();
                Console.Write("  Paste full path to b1-Win64-Shipping.exe: ");
                gamePath = Console.ReadLine()?.Trim().Trim('"');
            }
            else
            {
                Log("GAME", $"  AUTO-DETECTED: {gamePath}", ConsoleColor.Green);
            }
        }

        if (string.IsNullOrEmpty(gamePath) || !File.Exists(gamePath))
        {
            Log("ERROR", $"Game executable NOT FOUND: {gamePath ?? "(empty)"}", ConsoleColor.Red);
            Log("ERROR", "Make sure Black Myth: Wukong is installed and provide the correct path.", ConsoleColor.Red);
            WaitForExit();
            return;
        }

        // Verify it looks like the right exe
        var fileInfo = new FileInfo(gamePath);
        Log("GAME", $"  Path    : {gamePath}", ConsoleColor.Green);
        Log("GAME", $"  Size    : {fileInfo.Length / (1024.0 * 1024.0):F1} MB", ConsoleColor.DarkCyan);
        Log("GAME", $"  Modified: {fileInfo.LastWriteTime:yyyy-MM-dd HH:mm:ss}", ConsoleColor.DarkCyan);
        Console.WriteLine();

        // === STEP 3: Auto-install ReadyMP mod files ===
        Log("MOD", "Installing ReadyMP mod files to game directory...", ConsoleColor.Yellow);

        // Path layout: {install}/b1/Binaries/Win64/b1-Win64-Shipping.exe
        string win64Dir = Path.GetDirectoryName(gamePath)!;
        // Navigate up to game install root (Win64 -> Binaries -> b1 -> root)
        string gameInstallRoot = Path.GetFullPath(Path.Combine(win64Dir, "..", "..", ".."));
        // b1 directory (CSharpLoader zip extracts relative to this)
        string b1Dir = Path.GetFullPath(Path.Combine(win64Dir, "..", ".."));
        // CSharpLoader mod directory: Win64/CSharpLoader/Mods/WukongMp.Coop/
        string csLoaderModsDir = Path.Combine(win64Dir, "CSharpLoader", "Mods");
        string coopModDir = Path.Combine(csLoaderModsDir, "WukongMp.Coop");

        Log("MOD", $"  Win64 dir      : {win64Dir}", ConsoleColor.DarkGray);
        Log("MOD", $"  Install root   : {gameInstallRoot}", ConsoleColor.DarkGray);
        Log("MOD", $"  Mod target dir : {coopModDir}", ConsoleColor.DarkGray);

        // --- Check for CSharpLoader (the mod framework) ---
        string versionDllPath = Path.Combine(win64Dir, "version.dll");
        string dwmapiPath = Path.Combine(win64Dir, "dwmapi.dll");
        string csLoaderDir = Path.Combine(win64Dir, "CSharpLoader");
        bool hasProxyDll = File.Exists(versionDllPath) || File.Exists(dwmapiPath);
        bool hasLoaderDir = Directory.Exists(csLoaderDir) && File.Exists(Path.Combine(csLoaderDir, "CSharpModBase.dll"));

        if (!hasProxyDll || !hasLoaderDir)
        {
            Log("MOD", "", ConsoleColor.Yellow);
            Log("MOD", "*** CSharpLoader NOT FOUND — attempting automatic download... ***", ConsoleColor.Yellow);
            Log("MOD", "  CSharpLoader is the mod framework that loads WukongMp.Coop into the game.", ConsoleColor.Cyan);

            bool installed = await TryDownloadCSharpLoader(b1Dir, win64Dir);
            if (installed)
            {
                hasProxyDll = File.Exists(versionDllPath) || File.Exists(dwmapiPath);
                hasLoaderDir = Directory.Exists(csLoaderDir) && File.Exists(Path.Combine(csLoaderDir, "CSharpModBase.dll"));
            }

            if (!hasProxyDll || !hasLoaderDir)
            {
                Log("MOD", "", ConsoleColor.Red);
                Log("MOD", "*** CSharpLoader auto-install FAILED — manual install required ***", ConsoleColor.Red);
                Log("MOD", "  TO FIX — Install CSharpLoader for Black Myth: Wukong:", ConsoleColor.Yellow);
                Log("MOD", "    Option A: Run the ReadyMP Launcher ONCE (free download at readymp.com)", ConsoleColor.Yellow);
                Log("MOD", "             It auto-installs CSharpLoader, then you can close it.", ConsoleColor.Yellow);
                Log("MOD", "    Option B: Manually download from:", ConsoleColor.Yellow);
                Log("MOD", "             https://github.com/czastack/B1CSharpLoader/releases", ConsoleColor.Yellow);
                Log("MOD", "             Extract the zip so that version.dll ends up in:", ConsoleColor.Yellow);
                Log("MOD", $"               {win64Dir}", ConsoleColor.Yellow);
                Log("MOD", "             and CSharpLoader/ folder ends up in the same directory.", ConsoleColor.Yellow);
                Log("MOD", "", ConsoleColor.Red);
            }
        }
        else
        {
            string proxyName = File.Exists(versionDllPath) ? "version.dll" : "dwmapi.dll";
            Log("MOD", $"CSharpLoader found (proxy: {proxyName}) — mod framework OK", ConsoleColor.Green);
        }

        // Look for mods folder - check multiple locations relative to this exe
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
            Log("MOD", $"  Mod source: {modsSourceDir}", ConsoleColor.Cyan);

            // Create the CSharpLoader/Mods/WukongMp.Coop/ directory structure
            Directory.CreateDirectory(coopModDir);

            int copied = 0;
            int skipped = 0;
            int errors = 0;

            foreach (var srcFile in Directory.GetFiles(modsSourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(modsSourceDir, srcFile);

                // Skip instruction/text files
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
                        // Skip if same size and same write time (already installed)
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
                        Log("MOD", $"  [COPY] {relativePath} ({srcInfo.Length / 1024.0:F0} KB)", ConsoleColor.Green);
                    }
                }
                catch (Exception ex)
                {
                    errors++;
                    Log("MOD", $"  [FAIL] {relativePath}: {ex.Message}", ConsoleColor.Red);
                }
            }

            if (errors > 0)
                Log("WARNING", $"  {errors} file(s) failed to copy!", ConsoleColor.Red);
            else
                Log("MOD", $"  Install complete: {copied} copied, {skipped} already up-to-date", ConsoleColor.Green);
        }
        else
        {
            Log("MOD", "  No mods source folder found. Checking locations searched:", ConsoleColor.DarkYellow);
            foreach (var sp in searchPaths)
                Log("MOD", $"    - {Path.GetFullPath(sp)}", ConsoleColor.DarkGray);
            Log("MOD", "  Will verify if mods are already in game directory...", ConsoleColor.DarkYellow);
        }

        // Verify all critical mod files are present
        string[] criticalMods = new[]
        {
            "ReadyM.Relay.Client.dll",
            "ReadyM.Relay.Common.dll",
            "ReadyM.Relay.Common.Wukong.dll",
            "ReadyM.Api.dll",
            "ReadyM.Api.Multiplayer.dll",
            "WukongMp.Api.dll",
            "WukongMp.Coop.dll"
        };

        int modsOk = 0;
        int modsMissing = 0;
        foreach (var mod in criticalMods)
        {
            string modPath = Path.Combine(coopModDir, mod);
            if (File.Exists(modPath))
            {
                modsOk++;
            }
            else
            {
                Log("MOD", $"  [MISSING] {mod} - CRITICAL", ConsoleColor.Red);
                modsMissing++;
            }
        }

        if (modsMissing > 0)
        {
            Log("ERROR", $"{modsMissing} critical mod file(s) missing after install!", ConsoleColor.Red);
            Log("ERROR", "The game WILL NOT connect without these files.", ConsoleColor.Red);
            Log("ERROR", "Place a 'mods' folder next to this exe with all the DLLs.", ConsoleColor.Red);
            WaitForExit();
            return;
        }

        Log("MOD", $"All {modsOk} critical mod files verified in CSharpLoader/Mods/WukongMp.Coop/!", ConsoleColor.Green);
        Console.WriteLine();

        // --- Prepare co-op save data ---
        // The in-game mod stores save files in GetModDirectory("WukongMp.Coop")
        // which resolves to {MOD_FOLDER}/WukongMp.Coop/ or Win64/CSharpLoader/Mods/WukongMp.Coop/
        // Our mods are already in coopModDir, so save files go there too.
        Directory.CreateDirectory(coopModDir);
        Log("MOD", $"Co-op mod+save directory ready: {coopModDir}", ConsoleColor.DarkCyan);

        // Pre-write world save to slot 8 (matches MP Launcher flow)
        string seedSave = Path.Combine(coopModDir, "ArchiveSaveFile.1.sav");
        string seedSaveDst = seedSave;
        string slot8Dst = Path.Combine(coopModDir, "ArchiveSaveFile.8.sav");
        if (File.Exists(seedSave) && !File.Exists(slot8Dst))
        {
            File.Copy(seedSave, slot8Dst);
            Log("MOD", "  Pre-seeded world save: ArchiveSaveFile.1.sav -> ArchiveSaveFile.8.sav", ConsoleColor.Green);
        }
        else if (File.Exists(seedSaveDst))
        {
            Log("MOD", "  Co-op seed save already present.", ConsoleColor.DarkCyan);
        }

        // cacert.pem is already in coopModDir from the mod install step
        // (No separate copy needed since mods are installed directly to CSharpLoader/Mods/WukongMp.Coop/)
        Console.WriteLine();

        // Save settings for next time
        File.WriteAllText(settingsPath,
            $"HOST_IP={hostIp}\n" +
            $"HOST_PORT={port}\n" +
            $"GAME_PATH={gamePath}\n");
        Log("SETTINGS", $"Settings saved to: {settingsPath}", ConsoleColor.DarkCyan);

        // === START LOCAL MOCK API SERVER ===
        // The in-game WukongMp.Coop mod makes HTTP requests to API_BASE_URL during initialization.
        // Without a server responding, the mod silently fails and never calls RequestConnect().
        // This mock handles the 4 endpoints the mod needs for save file management.
        const int mockApiPort = 8769;
        var mockApi = new MockApiServer(mockApiPort, Log);
        var mockApiCts = new CancellationTokenSource();

        // Pre-seed with save data if available
        if (Directory.Exists(coopModDir))
        {
            mockApi.SetSeedSavePath(coopModDir);
            string seedSavePath = Path.Combine(coopModDir, "ArchiveSaveFile.1.sav");
            if (File.Exists(seedSavePath))
            {
                byte[] saveData = File.ReadAllBytes(seedSavePath);
                Guid clientGuid = GenerateStableGuid(Environment.MachineName + Environment.UserName);
                mockApi.PreSeedBlob("world.sav", saveData);
                mockApi.PreSeedBlob($"player_{clientGuid:N}.sav", saveData);
            }
        }

        _ = Task.Run(() => mockApi.StartAsync(mockApiCts.Token));
        await Task.Delay(500); // Give mock server a moment to bind
        Log("API-MOCK", $"Mock API server ready at {mockApi.BaseUrl}", ConsoleColor.Green);
        Console.WriteLine();

        // === STEP 4: Write the handshake file ===
        Log("HANDSHAKE", "Writing handshake environment file...", ConsoleColor.Yellow);

        string appDataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ReadyM.Launcher");
        Directory.CreateDirectory(appDataDir);

        string handshakePath = Path.Combine(appDataDir, "wukong_handshake.env");

        Guid userGuid = GenerateStableGuid(Environment.MachineName + Environment.UserName);
        Log("HANDSHAKE", $"  Player GUID : {userGuid}", ConsoleColor.DarkCyan);
        Log("HANDSHAKE", $"  Server      : {hostIp}:{port}", ConsoleColor.DarkCyan);

        var handshake = new[]
        {
            $"LAUNCHER_PID={Environment.ProcessId}",
            $"GAME_MODE=co-op",
            $"PLAYER_ID={userGuid}",
            $"NICKNAME={Environment.UserName}",
            $"SERVER_ID=1",
            $"SERVER_IP={hostIp}",
            $"SERVER_PORT={port}",
            $"API_BASE_URL={mockApi.BaseUrl}",
            $"JWT_TOKEN=direct-relay",
            $"MOD_FOLDER={csLoaderModsDir}"
        };

        File.WriteAllLines(handshakePath, handshake);
        Log("HANDSHAKE", $"  Written to: {handshakePath}", ConsoleColor.Green);

        foreach (var line in handshake)
            Log("HANDSHAKE", $"    {line}", ConsoleColor.DarkGray);
        Console.WriteLine();

        // === STEP 5: Launch the game ===
        Console.WriteLine("  ========================================");
        Log("LAUNCH", $"  Server  : {hostIp}:{port}", ConsoleColor.White);
        Log("LAUNCH", $"  Player  : {userGuid}", ConsoleColor.White);
        Log("LAUNCH", $"  Game    : {Path.GetFileName(gamePath)}", ConsoleColor.White);
        Console.WriteLine("  ========================================");
        Console.WriteLine();

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
            {
                Log("LAUNCH", $"Game launched! PID={proc.Id}", ConsoleColor.Green);
            }
            else
            {
                Log("LAUNCH", "Game process started (PID unknown - launched via shell)", ConsoleColor.Green);
            }

            Console.WriteLine();
            Log("STATUS", $"Game should connect to the relay server at {hostIp}:{port}", ConsoleColor.Cyan);
            Log("STATUS", "Check the DirectRelay server console for [CONNECT] messages.", ConsoleColor.Cyan);
            Log("STATUS", $"Mock API server running at {mockApi.BaseUrl} (handling game save requests)", ConsoleColor.DarkCyan);
            Console.WriteLine();
            Log("STATUS", "Press any key to exit (mock API server will stop)...", ConsoleColor.Gray);
            Console.ReadKey();
            mockApiCts.Cancel();
        }
        catch (Exception ex)
        {
            Log("ERROR", $"FAILED to launch game: {ex.GetType().Name}: {ex.Message}", ConsoleColor.Red);
            Log("ERROR", $"Path: {gamePath}", ConsoleColor.Red);
            WaitForExit();
        }
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

    static string? FindGameExe()
    {
        // Try Steam registry
        Log("SEARCH", "  Checking Steam registry...", ConsoleColor.DarkGray);
        string? steamPath = TryGetSteamGamePath();
        if (steamPath != null)
        {
            Log("SEARCH", "  Found via Steam registry!", ConsoleColor.Green);
            return steamPath;
        }

        // Common install paths - check all drive letters
        Log("SEARCH", "  Checking common install paths...", ConsoleColor.DarkGray);
        string[] commonPaths = new[]
        {
            @"C:\Program Files (x86)\Steam\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
            @"C:\Program Files\Steam\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
            @"D:\SteamLibrary\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
            @"E:\SteamLibrary\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
            @"F:\SteamLibrary\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
            @"G:\SteamLibrary\steamapps\common\BlackMythWukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
        };

        foreach (var p in commonPaths)
        {
            string drive = Path.GetPathRoot(p) ?? "?";
            if (File.Exists(p))
            {
                Log("SEARCH", $"  Found at: {p}", ConsoleColor.Green);
                return p;
            }
            Log("SEARCH", $"  Not at {drive}...", ConsoleColor.DarkGray);
        }

        return null;
    }

    static string? TryGetSteamGamePath()
    {
        try
        {
            Log("SEARCH", "    Checking HKLM Steam App 2358720...", ConsoleColor.DarkGray);
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 2358720");
            string? installDir = key?.GetValue("InstallLocation") as string;
            if (installDir != null)
            {
                Log("SEARCH", $"    Registry install dir: {installDir}", ConsoleColor.DarkGray);
                string exe = Path.Combine(installDir, "b1", "Binaries", "Win64", "b1-Win64-Shipping.exe");
                if (File.Exists(exe)) return exe;
                Log("SEARCH", "    Exe not at expected subpath", ConsoleColor.DarkGray);
            }
            else
            {
                Log("SEARCH", "    HKLM key not found", ConsoleColor.DarkGray);
            }
        }
        catch (Exception ex)
        {
            Log("SEARCH", $"    HKLM failed: {ex.Message}", ConsoleColor.DarkGray);
        }

        try
        {
            Log("SEARCH", "    Checking HKCU Steam path...", ConsoleColor.DarkGray);
            using var steamKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");
            string? steamDir = steamKey?.GetValue("SteamPath") as string;
            if (steamDir != null)
            {
                Log("SEARCH", $"    Steam dir: {steamDir}", ConsoleColor.DarkGray);
                string exe = Path.Combine(steamDir, "steamapps", "common", "BlackMythWukong", "b1", "Binaries", "Win64", "b1-Win64-Shipping.exe");
                if (File.Exists(exe)) return exe;

                string vdfPath = Path.Combine(steamDir, "steamapps", "libraryfolders.vdf");
                if (File.Exists(vdfPath))
                {
                    Log("SEARCH", "    Parsing libraryfolders.vdf...", ConsoleColor.DarkGray);
                    foreach (var line in File.ReadAllLines(vdfPath))
                    {
                        if (line.Contains("\"path\""))
                        {
                            var path = line.Split('"').Where(s => s.Contains('\\') || s.Contains('/')).FirstOrDefault();
                            if (path != null)
                            {
                                Log("SEARCH", $"    Library folder: {path}", ConsoleColor.DarkGray);
                                exe = Path.Combine(path, "steamapps", "common", "BlackMythWukong", "b1", "Binaries", "Win64", "b1-Win64-Shipping.exe");
                                if (File.Exists(exe)) return exe;
                            }
                        }
                    }
                }
                else
                {
                    Log("SEARCH", "    libraryfolders.vdf not found", ConsoleColor.DarkGray);
                }
            }
            else
            {
                Log("SEARCH", "    HKCU Steam key not found", ConsoleColor.DarkGray);
            }
        }
        catch (Exception ex)
        {
            Log("SEARCH", $"    HKCU failed: {ex.Message}", ConsoleColor.DarkGray);
        }

        return null;
    }

    static Guid GenerateStableGuid(string input)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        return new Guid(hash);
    }

    static void Log(string tag, string msg, ConsoleColor color = ConsoleColor.Gray)
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
            // Check if rule already exists
            var psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"advfirewall firewall show rule name=\"DirectRelayConnect UDP {port}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var proc = Process.Start(psi);
            proc?.WaitForExit(5000);
            string output = proc?.StandardOutput.ReadToEnd() ?? "";

            if (!output.Contains("DirectRelayConnect"))
            {
                Log("FIREWALL", $"Creating firewall rule for UDP port {port}...", ConsoleColor.Yellow);
                var add = Process.Start(new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = $"advfirewall firewall add rule name=\"DirectRelayConnect UDP {port}\" dir=in action=allow protocol=UDP localport={port}",
                    UseShellExecute = true,
                    Verb = "runas",
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

    static void WaitForExit()
    {
        Console.WriteLine();
        Log("STATUS", "Press any key to exit...", ConsoleColor.Gray);
        Console.ReadKey();
    }
}

/// <summary>
/// Minimal INetEventListener for diagnostic ping/pong exchange via LiteNetLib unconnected messages.
/// </summary>
class DiagPingListener : INetEventListener
{
    public bool GotPong;
    public string? PongData;

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        if (reader.AvailableBytes > 4)
        {
            try
            {
                // The relay wraps the response in writer.Put(byte[]) which is length-prefixed
                byte[] rawData = reader.GetBytesWithLength();
                string text = System.Text.Encoding.UTF8.GetString(rawData);
                int pongIdx = text.IndexOf("DIRECTRELAY_DIAG_PONG");
                if (pongIdx >= 0)
                {
                    PongData = text.Substring(pongIdx);
                    GotPong = true;
                }
            }
            catch { }
        }
    }

    public void OnConnectionRequest(ConnectionRequest request) => request.Reject();
    public void OnPeerConnected(NetPeer peer) { }
    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) { }
    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod) { }
    public void OnNetworkError(IPEndPoint endPoint, System.Net.Sockets.SocketError socketError) { }
    public void OnNetworkLatencyUpdate(NetPeer peer, int latency) { }
}
