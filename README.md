# DirectRelay for Black Myth: Wukong Co-op

Play Black Myth: Wukong co-op using a direct relay server — no third-party launcher needed.

---

## What's In This Repo

```
HOST/                       <- The person hosting the game uses this folder
  DirectRelay.exe           <- Run this. That's it.
  mods/                     <- Mod files (auto-installed to your game)

CLIENT/                     <- The person joining uses this folder
  DirectRelayConnect.exe    <- Run this. That's it.
  mods/                     <- Mod files (auto-installed to your game)

src/                        <- Source code (for developers)
  DirectRelay/              <- HOST relay server source (C# / .NET 8)
  DirectRelayConnect/       <- CLIENT connector source (C# / .NET 8)
  DirectRelay-Wukong-Coop.sln <- Visual Studio solution file

docs/                       <- Reference documentation
  decompiled/               <- Decompiled ReadyM mod sources (read-only reference)
```

---

## Quick Start

### HOST (the person running the server)

1. Download the `HOST` folder
2. Run `DirectRelay.exe`
3. **Everything else is automatic:**
   - Finds your Black Myth: Wukong installation
   - Installs all mod files to your game directory
   - Writes the connection handshake file
   - Launches the game
   - Starts the relay server and waits for your friend

### CLIENT (the person joining)

1. Download the `CLIENT` folder
2. Run `DirectRelayConnect.exe`
3. Enter the host's IP address when prompted
4. **Everything else is automatic:**
   - Finds your Black Myth: Wukong installation
   - Installs all mod files to your game directory
   - Writes the connection handshake file
   - Launches the game and connects to the relay

---

## Things That Cannot Be Automated

These are the **only** manual steps. The software handles everything else.

### 1. Port Forwarding (HOST only)

The relay server listens on **UDP port 7777**. If you and your friend are connecting over the internet (not the same local network), you must forward this port on your router.

**How to port forward:**
- Open your router's admin page (usually `192.168.1.1` or `192.168.0.1` in a browser)
- Find "Port Forwarding" (sometimes under Advanced, NAT, or Firewall)
- Add a new rule:
  - **Protocol:** UDP
  - **External Port:** 7777
  - **Internal Port:** 7777
  - **Internal IP:** Your PC's local IP (run `ipconfig` in Command Prompt → look for IPv4 Address, usually `192.168.x.x`)
- Save and apply

> Every router is different. Search "[your router model] port forwarding" if you need help finding the setting.

### 2. Windows Firewall (HOST only)

Windows may block the relay server. If your friend can't connect:

- When you first run `DirectRelay.exe`, Windows may show a firewall popup — **click "Allow access"**
- If you missed the popup or it didn't appear, manually allow it:
  1. Open **Windows Defender Firewall** (search in Start menu)
  2. Click **"Allow an app or feature through Windows Defender Firewall"**
  3. Click **"Change settings"** → **"Allow another app"**
  4. Browse to `DirectRelay.exe` and add it
  5. Check both **Private** and **Public** boxes

### 3. Share Your IP Address (HOST only)

Your friend needs your public IP address to connect. Find it by:
- Going to [whatismyip.com](https://whatismyip.com) in your browser
- Or running this in Command Prompt: `curl ifconfig.me`

Give that IP to your friend. They enter it when `DirectRelayConnect.exe` asks for the Host IP.

> If you're both on the **same local network** (same house/WiFi), your friend can use your local IP instead (`192.168.x.x` from `ipconfig`). No port forwarding needed in this case.

### 4. Own the Game (BOTH)

Both players must have Black Myth: Wukong installed via Steam. The tools auto-detect the game location from Steam's registry and common install paths.

---

## Troubleshooting

| Problem | Solution |
|---|---|
| "Game executable NOT FOUND" | Make sure Black Myth: Wukong is installed via Steam. If it's in a non-standard location, the tool will ask you to paste the path to `b1-Win64-Shipping.exe`. |
| Server shows no connections | Check port forwarding (UDP 7777), Windows firewall, and make sure your friend has the correct IP. |
| Game launches but no co-op | Both players need the mod files installed. The tools do this automatically, but check the console output for any `[FAIL]` messages during the `[MOD]` step. |
| Friend's connection times out | The relay server must be running BEFORE the friend launches. Host runs `DirectRelay.exe` first, then the friend runs `DirectRelayConnect.exe`. |
| "Multiplayer is disabled" in-game | The handshake file was missing or expired. Close the game, run the tool again (it writes a fresh handshake), and relaunch. |

---

## How It Works

1. **Mod files** (`ReadyM.Relay.Client.dll`, `WukongMp.Coop.dll`, etc.) add multiplayer networking to the game using LiteNetLib
2. A **handshake file** (`%AppData%\ReadyM.Launcher\wukong_handshake.env`) tells the game where to connect and who you are
3. The **relay server** (`DirectRelay.exe`) accepts connections from both players and forwards game data between them
4. The game reads the handshake on startup (and deletes it), connects to the relay, and co-op begins

The tools automate all of this — installing mods, writing the handshake, launching the game, and running the relay.

---

## Default Settings

| Setting | Value |
|---|---|
| Protocol | UDP |
| Port | 7777 (configurable: `DirectRelay.exe 8888`) |
| Timeout | 30 seconds |
| Max Players | 2+ (relay forwards to all connected) |

---

## Building from Source

The relay supports two build modes for different use cases:

### Production Mode (Default - Maximum Performance)
```bash
dotnet build -c Release
```
- Zero logging in critical packet forwarding path (hot path)
- Statistics disabled
- Optimized for sub-5ms relay overhead
- Lock-free packet forwarding
- Zero-copy buffer operations
- For actual gameplay

### Diagnostic Mode (Troubleshooting)
```bash
dotnet build -c Debug
```
- Full packet logging enabled
- Statistics tracking enabled
- Performance monitoring
- For diagnosing connection issues
- Automatically enabled in Debug configuration

### Configuration File

The relay can be tuned via `relay_config.json` without recompiling:

```json
{
  "Network": {
    "Port": 7777,
    "UpdateTimeMs": 1,
    "DisconnectTimeoutMs": 30000,
    "SendBufferSizeKB": 1024,
    "ReceiveBufferSizeKB": 1024,
    "MTU": 1400
  },
  "Performance": {
    "HighPriorityThread": true,
    "HighPriorityProcess": true,
    "EnableStatistics": false
  },
  "Diagnostics": {
    "LogPackets": false,
    "LogConnections": true,
    "LogErrors": true,
    "LogFilePath": "relay_diagnostics.log"
  }
}
```

### Performance Optimizations

The relay includes several network performance optimizations for 2-player co-op:

- **Lock-free hot path**: ConcurrentDictionary + atomic operations eliminate lock contention in the critical packet forwarding code path
- **Zero-copy forwarding**: Direct buffer sends without intermediate allocations
- **Object pooling**: NetDataWriter pool eliminates GC pressure
- **Conditional compilation**: Production builds have zero logging overhead
- **Thread priority**: High-priority threads for consistent frame times
- **Optimized LiteNetLib**: 1ms update time, disabled statistics in production

**Performance Targets:**
- Packet forwarding latency: <1ms
- Total relay overhead: <5ms
- Memory allocations: Near-zero in hot path (critical forwarding code)
- CPU usage: <2% on modern hardware

