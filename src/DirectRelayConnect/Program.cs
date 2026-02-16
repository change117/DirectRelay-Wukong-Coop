using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;

namespace DirectRelayConnect;

class Program
{
    static void Main(string[] args)
    {
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

        // UDP reachability test
        try
        {
            using var udp = new UdpClient();
            udp.Connect(hostIp, portNum);
            var localEp = (IPEndPoint)udp.Client.LocalEndPoint!;
            Log("NETWORK", $"  UDP socket OK - local endpoint: {localEp}", ConsoleColor.Green);
            Log("NETWORK", $"  NOTE: UDP is connectionless - can't verify server is listening", ConsoleColor.DarkGray);
            Log("NETWORK", $"        until the game actually connects. Check the relay console.", ConsoleColor.DarkGray);
        }
        catch (Exception ex)
        {
            Log("WARNING", $"  UDP test failed: {ex.Message}", ConsoleColor.Red);
            Log("WARNING", $"  This may indicate a firewall or routing issue", ConsoleColor.Red);
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
        string gameDir = Path.GetDirectoryName(gamePath)!;
        Log("MOD", $"  Game dir: {gameDir}", ConsoleColor.DarkGray);

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
            int copied = 0;
            int skipped = 0;
            int errors = 0;

            foreach (var srcFile in Directory.GetFiles(modsSourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(modsSourceDir, srcFile);

                // Skip instruction/text files
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
            string modPath = Path.Combine(gameDir, mod);
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

        Log("MOD", $"All {modsOk} critical mod files verified in game directory!", ConsoleColor.Green);
        Console.WriteLine();

        // Save settings for next time
        File.WriteAllText(settingsPath,
            $"HOST_IP={hostIp}\n" +
            $"HOST_PORT={port}\n" +
            $"GAME_PATH={gamePath}\n");
        Log("SETTINGS", $"Settings saved to: {settingsPath}", ConsoleColor.DarkCyan);

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
            $"API_BASE_URL=http://localhost",
            $"JWT_TOKEN=direct-relay"
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
                WorkingDirectory = Path.GetDirectoryName(gamePath)!,
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
            Console.WriteLine();
            Log("STATUS", "Press any key to exit...", ConsoleColor.Gray);
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Log("ERROR", $"FAILED to launch game: {ex.GetType().Name}: {ex.Message}", ConsoleColor.Red);
            Log("ERROR", $"Path: {gamePath}", ConsoleColor.Red);
            WaitForExit();
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
    }

    static void WaitForExit()
    {
        Console.WriteLine();
        Log("STATUS", "Press any key to exit...", ConsoleColor.Gray);
        Console.ReadKey();
    }
}
