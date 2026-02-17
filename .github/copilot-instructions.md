# DirectRelay for Black Myth: Wukong Co-op — AI Coding Guide

## Project Architecture

This project enables direct peer-to-peer co-op for Black Myth: Wukong using a relay server, eliminating third-party launcher dependencies.

**Two complementary applications** (both C# .NET 8 Windows Forms):
- **DirectRelay** (`src/DirectRelay/`) — The HOST server that relays packets between players
- **DirectRelayConnect** (`src/DirectRelayConnect/`) — The CLIENT connector that joins a relay

**Critical workflow**: User launches GUI control panel → Manages saves → DirectRelay auto-installs mods + writes handshake file (`%APPDATA%/ReadyM.Launcher/wukong_handshake.env`) → Launches Black Myth: Wukong → Relay/connection established.

## Network Protocol

The relay implements ReadyMP's message protocol using **LiteNetLib** (UDP-based):

### Message Codes (constants in `DirectRelayServer`)
- `255` — HandshakeConnected (server assigns player ID + initial network state)
- `250-242` — ECS game state messages (EcsDelta, EcsSnapshot, EntityCreate/Delete, Ownership changes)
- `254-251` — Area/connection events (RequestAreaEvent, AreaEvent, PlayerConnection)
- `245-242` — Blob upload/download (for large data transfers)
- `0-149` — ClientRPC (client-side remote procedures)

### Relay Modes (byte mode in forwarded messages)
- `0` = AreaOthers, `1` = AreaAll, `2` = GlobalOthers, `3` = GlobalAll, `4` = EntityOwner, `5` = Peers

**Packet flow**: Game → Client mod → Relay (deserialize + determine destination) → Broadcast/unicast based on mode → Target client.

## Project Structure & Key Files

| File | Purpose |
|------|---------|
| `Program.cs` (DirectRelay) | Main server loop, AutoSetupHost (mods/handshake), INetEventListener implementation |
| `DirectRelayServer` class | Connection handling, player/area management, packet relay logic |
| `ControlPanelWindow.cs` | WinForms GUI for save backup/restore before game launch |
| `SaveManager.cs` | `.wksave` format handling (ZIP archives with .sav files + JSON metadata) |
| `relay_config.json` | Network/performance tuning (port, buffer sizes, thread priority, logging) |
| `docs/decompiled/` | Reference decompiled ReadyMP mod sources (read-only; shows protocol) |

## Performance Patterns

The relay prioritizes **ultra-low latency**:

1. **Object pooling** — `NetDataWriterPool` reuses UDP packet writers to eliminate allocations
2. **Concurrent collections** — `_byPeerId`, `_byPlayerId` are lock-free for hot-path packet forwarding
3. **High-priority execution**:
   ```csharp
   if (_config.Performance.HighPriorityThread) Thread.CurrentThread.Priority = ThreadPriority.Highest;
   if (_config.Performance.HighPriorityProcess) Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
   ```
4. **Configuration-driven** — `relay_config.json` tunes MTU (1400), buffer sizes (1024KB), poll rate (1ms)
5. **Conditional compilation** — `DIAGNOSTIC_MODE` enables statistics/logging; `PRODUCTION` disables for performance

**When adding relay logic**: Reuse pools, avoid allocations in packet forwarding, prefer concurrent collections over locks on the hot path.

## Mod Installation & Handshake

`AutoSetupHost()` in Program.cs runs **before** game launch:

1. **Finds game** — Registry lookup or steam directory scanning for `Black Myth Wukong.exe`
2. **Installs mods** — Copies `mods/` folder contents to game directory, skipping files with matching size/timestamp
3. **Verifies essentials** — Checks 7 critical mod DLLs (ReadyM.Relay.*, WukongMp.Coop, etc.)
4. **Writes handshake** — Creates `%APPDATA%/ReadyM.Launcher/wukong_handshake.env` with:
   - `PLAYER_ID={GUID}`, `SERVER_IP=127.0.0.1`, `SERVER_PORT={port}`, `JWT_TOKEN=direct-relay`
5. **Launches game** — Starts as child process in game directory

The handshake file tells the in-game mods how to authenticate with the relay.

## Save File Management

`.wksave` format is a ZIP archive containing:
- All `ArchiveFile_*.sav` files from the game's save directory (`%LOCALAPPDATA%/b1/Saved/SaveGames/{SteamID}/`)
- Metadata JSON with timestamp, username, machine info

**Slot mapping** (hardcoded in SaveManager):
- Slot 1 — Single-player save
- Slot 7 & 8 — Co-op player/world saves

GUI allows download (backup current state) or upload (restore from `.wksave`) before launching.

## Build & Run

```bash
cd src
dotnet build DirectRelay-Wukong-Coop.sln -c Release  # or Debug for DIAGNOSTIC_MODE

# Run DirectRelay (HOST)
./DirectRelay/bin/Release/net8.0-windows/DirectRelay.exe

# Run DirectRelayConnect (CLIENT)
./DirectRelayConnect/bin/Release/net8.0-windows/DirectRelayConnect.exe
```

Build outputs go to `bin/Release` (or `bin/Debug`). GUI and relay runs in same process.

## Logging Pattern

All components use a standard logging callback:
```csharp
void Log(string category, string message, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{category,-10}] {message}");
    Console.ResetColor();
}
```

**Categories** (self-documenting): `SETUP`, `CONNECT`, `RELAY`, `SEND`, `RECV`, `DISCONN`, `ERROR`, `HEARTBEAT`, `PERF`, `GUI`, `SAVE`.

Use this pattern when adding features; avoid raw Console output.

## Common Tasks

### Adding a new relay message type
1. Add message code constant in `DirectRelayServer` (e.g., `const byte MSG_NewType = 241;`)
2. Add to `MessageNames` dictionary for logging
3. Handle in `OnNetworkReceive()` switch block
4. Forward to appropriate recipients based on mode

### Modifying relay forwarding logic
Look in `OnNetworkReceive()` method. Current logic:
- Reads message code + mode
- Looks up sender's area
- Broadcasts to category (area/global) × recipient type (others/all/owner)
- Reuses `NetDataWriter` from pool

### Testing connectivity
Both clients validate network connectivity before launching:
- DNS resolution check
- UDP reachability test (echo probe)
- Handshake exchange verification

See `Program.cs` (DirectRelayConnect) for validation pattern.

## Decompiled References

`docs/decompiled/` contains read-only `.cs` files from the game's ReadyMP mod. Use these to understand:
- Protocol constants and message structures
- Area/player lifecycle
- ECS entity synchronization patterns

Do **not** modify these files; they're reference-only.

## Known Constraints

1. **Windows-only** — Uses WinForms, registry lookups, process priority APIs
2. **Single game instance** — Handshake file is per-machine (not per-process)
3. **Internet play requires port forwarding** — UPnP auto-forwarding is attempted at startup; if it fails, the HOST must manually forward UDP port in their router
4. **Save sync is manual** — No automatic cloud sync between players' machines
