<#
.SYNOPSIS
    DirectRelay Connection Diagnostics — Run on BOTH the HOST and CLIENT PCs
.DESCRIPTION
    Checks every piece of the connection chain:
    1. Game installation & mod DLLs
    2. Handshake file (existence, content, freshness)
    3. Network interfaces & IP addresses
    4. Firewall rules for UDP port 7777
    5. Port binding status (is the relay actually listening?)
    6. UDP connectivity test to a remote relay
    7. DNS resolution
    8. NAT type detection hints
    
    Run as: powershell -ExecutionPolicy Bypass -File diagnose.ps1
    Or right-click → Run with PowerShell
#>

param(
    [int]$Port = 7777,
    [string]$RemoteHost = "",
    [switch]$FixFirewall
)

$ErrorActionPreference = "Continue"
$script:PassCount = 0
$script:WarnCount = 0
$script:FailCount = 0
$script:DiagLog = @()

function Write-Header($text) {
    $line = "=" * 60
    Write-Host ""
    Write-Host $line -ForegroundColor Cyan
    Write-Host "  $text" -ForegroundColor White
    Write-Host $line -ForegroundColor Cyan
    $script:DiagLog += "", $line, "  $text", $line
}

function Write-Pass($text) {
    Write-Host "  [PASS] $text" -ForegroundColor Green
    $script:PassCount++
    $script:DiagLog += "  [PASS] $text"
}

function Write-Warn($text) {
    Write-Host "  [WARN] $text" -ForegroundColor Yellow
    $script:WarnCount++
    $script:DiagLog += "  [WARN] $text"
}

function Write-Fail($text) {
    Write-Host "  [FAIL] $text" -ForegroundColor Red
    $script:FailCount++
    $script:DiagLog += "  [FAIL] $text"
}

function Write-Info($text) {
    Write-Host "  [INFO] $text" -ForegroundColor Gray
    $script:DiagLog += "  [INFO] $text"
}

function Write-Fix($text) {
    Write-Host "  [FIX]  $text" -ForegroundColor Magenta
    $script:DiagLog += "  [FIX]  $text"
}

# ============================================================
# HEADER
# ============================================================
Clear-Host
Write-Host ""
Write-Host "  DirectRelay Connection Diagnostics" -ForegroundColor White
Write-Host "  $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor DarkGray
Write-Host "  Machine: $env:COMPUTERNAME | User: $env:USERNAME" -ForegroundColor DarkGray
Write-Host "  OS: $([System.Environment]::OSVersion.VersionString)" -ForegroundColor DarkGray
Write-Host ""

# ============================================================
# 1. GAME INSTALLATION
# ============================================================
Write-Header "1. GAME INSTALLATION"

$gameExe = $null
$gameDir = $null

# Check Steam registry
$steamPaths = @()
try {
    $key = Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 2358720" -ErrorAction SilentlyContinue
    if ($key -and $key.InstallLocation) {
        $steamPaths += $key.InstallLocation
        Write-Info "Registry install dir: $($key.InstallLocation)"
    }
} catch {}

try {
    $steamKey = Get-ItemProperty -Path "HKCU:\SOFTWARE\Valve\Steam" -ErrorAction SilentlyContinue
    if ($steamKey -and $steamKey.SteamPath) {
        $steamDir = $steamKey.SteamPath
        $steamPaths += "$steamDir\steamapps\common\BlackMythWukong"
        
        # Parse libraryfolders.vdf
        $vdfPath = "$steamDir\steamapps\libraryfolders.vdf"
        if (Test-Path $vdfPath) {
            $vdfContent = Get-Content $vdfPath -Raw
            $pathMatches = [regex]::Matches($vdfContent, '"path"\s+"([^"]+)"')
            foreach ($m in $pathMatches) {
                $libPath = $m.Groups[1].Value -replace '\\\\', '\'
                $steamPaths += "$libPath\steamapps\common\BlackMythWukong"
            }
        }
    }
} catch {}

# Common paths
$commonDrives = @("C", "D", "E", "F", "G")
foreach ($drive in $commonDrives) {
    $steamPaths += "${drive}:\Program Files (x86)\Steam\steamapps\common\BlackMythWukong"
    $steamPaths += "${drive}:\Program Files\Steam\steamapps\common\BlackMythWukong"
    $steamPaths += "${drive}:\SteamLibrary\steamapps\common\BlackMythWukong"
}

$exeRelPath = "b1\Binaries\Win64\b1-Win64-Shipping.exe"
foreach ($base in ($steamPaths | Select-Object -Unique)) {
    $candidate = Join-Path $base $exeRelPath
    if (Test-Path $candidate) {
        $gameExe = $candidate
        $gameDir = Split-Path $candidate
        break
    }
}

if ($gameExe) {
    Write-Pass "Game found: $gameExe"
    $fi = Get-Item $gameExe
    Write-Info "Size: $([math]::Round($fi.Length / 1MB, 1)) MB | Modified: $($fi.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss'))"
} else {
    Write-Fail "Game executable NOT FOUND"
    Write-Fix "Install Black Myth: Wukong via Steam, or check that it's in a standard Steam library folder"
}

# ============================================================
# 2. MOD FILES
# ============================================================
Write-Header "2. MOD DLL FILES"

$criticalMods = @(
    "ReadyM.Relay.Client.dll",
    "ReadyM.Relay.Common.dll",
    "ReadyM.Relay.Common.Wukong.dll",
    "ReadyM.Api.dll",
    "ReadyM.Api.Multiplayer.dll",
    "WukongMp.Api.dll",
    "WukongMp.Coop.dll"
)

if ($gameDir) {
    $modsOk = 0
    $modsMissing = 0
    foreach ($mod in $criticalMods) {
        $modPath = Join-Path $gameDir $mod
        if (Test-Path $modPath) {
            $modInfo = Get-Item $modPath
            Write-Pass "$mod ($([math]::Round($modInfo.Length / 1KB)) KB, $($modInfo.LastWriteTime.ToString('MM/dd HH:mm')))"
            $modsOk++
        } else {
            Write-Fail "$mod — MISSING"
            $modsMissing++
        }
    }
    
    if ($modsMissing -gt 0) {
        Write-Fix "Re-run DirectRelay.exe or DirectRelayConnect.exe — it auto-installs mods from the 'mods' folder"
        Write-Fix "Or manually copy all DLLs from the HOST/mods or CLIENT/mods folder to: $gameDir"
    }
    
    # Check for LiteNetLib in game directory (the mod needs this too)
    $liteNetPath = Join-Path $gameDir "LiteNetLib.dll"
    if (Test-Path $liteNetPath) {
        Write-Pass "LiteNetLib.dll found (networking library)"
    } else {
        Write-Warn "LiteNetLib.dll not found in game dir — the mod bundles this internally, but check if needed"
    }
} else {
    Write-Warn "Skipped mod check — game directory not found"
}

# ============================================================
# 3. HANDSHAKE FILE
# ============================================================
Write-Header "3. HANDSHAKE FILE"

$appDataDir = Join-Path $env:APPDATA "ReadyM.Launcher"
$handshakePath = Join-Path $appDataDir "wukong_handshake.env"

Write-Info "Expected location: $handshakePath"

if (Test-Path $handshakePath) {
    $handshakeContent = Get-Content $handshakePath -Raw
    $handshakeAge = (Get-Date) - (Get-Item $handshakePath).LastWriteTime
    
    if ($handshakeAge.TotalMinutes -lt 30) {
        Write-Pass "Handshake file exists (written $([math]::Round($handshakeAge.TotalMinutes, 1)) min ago)"
    } else {
        Write-Warn "Handshake file exists but is OLD ($([math]::Round($handshakeAge.TotalHours, 1)) hours ago)"
        Write-Fix "The game mod DELETES this file after reading it. If it's old, the game never read it (mod not loaded?)"
    }
    
    Write-Info "--- Handshake Contents ---"
    $handshakeLines = Get-Content $handshakePath
    $handshakeData = @{}
    foreach ($line in $handshakeLines) {
        Write-Info "  $line"
        if ($line -match '^(?<key>[^=]+)=(?<value>.*)$') {
            $handshakeData[$Matches['key'].Trim()] = $Matches['value'].Trim()
        }
    }
    
    # Validate critical fields
    $requiredFields = @("GAME_MODE", "PLAYER_ID", "SERVER_IP", "SERVER_PORT", "SERVER_ID", "JWT_TOKEN", "API_BASE_URL")
    foreach ($field in $requiredFields) {
        if ($handshakeData.ContainsKey($field) -and $handshakeData[$field]) {
            # silent pass
        } else {
            Write-Fail "Missing or empty required field: $field"
        }
    }
    
    if ($handshakeData["GAME_MODE"] -ne "co-op") {
        Write-Fail "GAME_MODE is '$($handshakeData["GAME_MODE"])' — must be 'co-op'"
    } else {
        Write-Pass "GAME_MODE = co-op"
    }
    
    if ($handshakeData["SERVER_IP"]) {
        Write-Pass "SERVER_IP = $($handshakeData["SERVER_IP"])"
    }
    if ($handshakeData["SERVER_PORT"]) {
        Write-Pass "SERVER_PORT = $($handshakeData["SERVER_PORT"])"
    }
    if ($handshakeData["PLAYER_ID"]) {
        try {
            $guid = [Guid]::Parse($handshakeData["PLAYER_ID"])
            if ($guid -eq [Guid]::Empty) {
                Write-Fail "PLAYER_ID is all zeros (Guid.Empty) — invalid"
            } else {
                Write-Pass "PLAYER_ID = $guid (valid GUID)"
            }
        } catch {
            Write-Fail "PLAYER_ID '$($handshakeData["PLAYER_ID"])' is NOT a valid GUID"
        }
    }
} else {
    Write-Warn "Handshake file does NOT exist"
    Write-Info "This is NORMAL if: (a) you haven't run the tool yet, or (b) the game already read and deleted it"
    Write-Info "The game mod reads and DELETES this file on startup — absence after a launch is expected"
    
    if (Test-Path $appDataDir) {
        Write-Pass "ReadyM.Launcher directory exists: $appDataDir"
    } else {
        Write-Warn "ReadyM.Launcher directory does not exist yet"
    }
}

# ============================================================
# 4. NETWORK INTERFACES & IP ADDRESSES
# ============================================================
Write-Header "4. NETWORK INTERFACES & IPs"

try {
    $adapters = Get-NetAdapter -ErrorAction SilentlyContinue | Where-Object { $_.Status -eq "Up" }
    if ($adapters) {
        foreach ($adapter in $adapters) {
            $ips = Get-NetIPAddress -InterfaceIndex $adapter.ifIndex -ErrorAction SilentlyContinue | 
                   Where-Object { $_.AddressFamily -eq "IPv4" -and $_.IPAddress -ne "127.0.0.1" }
            foreach ($ip in $ips) {
                $adapterType = if ($adapter.InterfaceDescription -match "Wi-Fi|Wireless|802\.11") { "WiFi" }
                               elseif ($adapter.InterfaceDescription -match "Ethernet|Realtek|Intel") { "Ethernet" }
                               elseif ($adapter.InterfaceDescription -match "VPN|TAP|Virtual|Hamachi|ZeroTier|Tailscale") { "VPN/Virtual" }
                               else { "Other" }
                Write-Pass "$($adapter.Name) ($adapterType): $($ip.IPAddress) / $($ip.PrefixLength)"
            }
        }
    }
} catch {
    # Fallback for older systems
    $ipconfig = ipconfig | Select-String "IPv4"
    foreach ($line in $ipconfig) {
        Write-Info $line.Line.Trim()
    }
}

# Get public IP
Write-Info "Checking public IP..."
try {
    $publicIp = (Invoke-WebRequest -Uri "https://api.ipify.org" -TimeoutSec 5 -UseBasicParsing).Content.Trim()
    Write-Pass "Public IP: $publicIp"
    Write-Info "If your friend is on a DIFFERENT network, they need to enter: $publicIp"
    Write-Info "You also need UDP port $Port forwarded to this PC in your router"
} catch {
    Write-Warn "Could not determine public IP (no internet?)"
}

# Check if both PCs are on same subnet (LAN check)
Write-Info ""
Write-Info "--- LAN vs Internet ---"
Write-Info "If BOTH PCs are on the same home network (same router/WiFi): use your LOCAL IP above"
Write-Info "If PCs are on DIFFERENT networks: the HOST must forward UDP port $Port in their router"

# ============================================================
# 5. FIREWALL
# ============================================================
Write-Header "5. WINDOWS FIREWALL"

try {
    $fwProfiles = Get-NetFirewallProfile -ErrorAction SilentlyContinue
    foreach ($profile in $fwProfiles) {
        $status = if ($profile.Enabled) { "ENABLED" } else { "DISABLED" }
        $color = if ($profile.Enabled) { "Yellow" } else { "Green" }
        Write-Info "$($profile.Name) firewall: $status"
    }
} catch {
    Write-Warn "Could not query firewall profiles (need admin?)"
}

# Check for existing rules allowing our port
$hasInboundRule = $false
$hasOutboundRule = $false
try {
    $rules = Get-NetFirewallRule -ErrorAction SilentlyContinue | Where-Object { $_.Enabled -eq $true }
    foreach ($rule in $rules) {
        try {
            $portFilter = Get-NetFirewallPortFilter -AssociatedNetFirewallRule $rule -ErrorAction SilentlyContinue
            if ($portFilter.LocalPort -eq $Port -or $portFilter.LocalPort -eq "Any") {
                if ($portFilter.Protocol -eq "UDP" -or $portFilter.Protocol -eq "Any") {
                    if ($rule.Direction -eq "Inbound" -and $rule.Action -eq "Allow") {
                        Write-Pass "Inbound UDP $Port ALLOWED by rule: $($rule.DisplayName)"
                        $hasInboundRule = $true
                    }
                    if ($rule.Direction -eq "Outbound" -and $rule.Action -eq "Allow") {
                        $hasOutboundRule = $true
                    }
                }
            }
        } catch {}
    }
} catch {
    Write-Warn "Could not query firewall rules (need admin?)"
}

if (-not $hasInboundRule) {
    Write-Fail "No firewall rule allowing INBOUND UDP on port $Port"
    Write-Fix "Run this script with -FixFirewall to auto-create the rule (requires admin)"
    Write-Fix "Or manually: New-NetFirewallRule -DisplayName 'DirectRelay UDP' -Direction Inbound -Protocol UDP -LocalPort $Port -Action Allow"
    
    if ($FixFirewall) {
        Write-Info "Attempting to create firewall rule..."
        try {
            New-NetFirewallRule -DisplayName "DirectRelay UDP $Port" -Direction Inbound -Protocol UDP -LocalPort $Port -Action Allow -Profile Any -ErrorAction Stop | Out-Null
            Write-Pass "Firewall rule created successfully!"
        } catch {
            Write-Fail "Failed to create rule: $($_.Exception.Message)"
            Write-Fix "Run PowerShell as Administrator and try again"
        }
    }
}

# Check for third-party firewalls
$thirdPartyFW = @(
    @{ Name = "Bitdefender"; Process = "bdagent" },
    @{ Name = "Norton"; Process = "NortonSecurity" },
    @{ Name = "McAfee"; Process = "mcshield" },
    @{ Name = "Kaspersky"; Process = "avp" },
    @{ Name = "ESET"; Process = "ekrn" },
    @{ Name = "Avast"; Process = "AvastSvc" },
    @{ Name = "AVG"; Process = "avgnt" },
    @{ Name = "Malwarebytes"; Process = "MBAMService" },
    @{ Name = "Comodo"; Process = "cmdagent" },
    @{ Name = "ZoneAlarm"; Process = "zlclient" }
)

foreach ($fw in $thirdPartyFW) {
    $proc = Get-Process -Name $fw.Process -ErrorAction SilentlyContinue
    if ($proc) {
        Write-Warn "$($fw.Name) detected — may be blocking UDP $Port"
        Write-Fix "Add an exception in $($fw.Name) for UDP port $Port (both inbound and outbound)"
    }
}

# ============================================================
# 6. PORT STATUS
# ============================================================
Write-Header "6. PORT $Port STATUS"

# Check if anything is listening on the port
$listening = $false
try {
    $udpListeners = Get-NetUDPEndpoint -ErrorAction SilentlyContinue | Where-Object { $_.LocalPort -eq $Port }
    if ($udpListeners) {
        foreach ($listener in $udpListeners) {
            $proc = Get-Process -Id $listener.OwningProcess -ErrorAction SilentlyContinue
            $procName = if ($proc) { "$($proc.ProcessName) (PID $($proc.Id))" } else { "PID $($listener.OwningProcess)" }
            Write-Pass "UDP $Port is BOUND by: $procName"
            $listening = $true
        }
    } else {
        Write-Info "UDP $Port is NOT bound — relay server is not running"
        Write-Info "This is expected on the CLIENT side. On the HOST, start DirectRelay.exe first."
    }
} catch {
    # Fallback
    $netstat = netstat -aon -p UDP 2>$null | Select-String ":$Port\s"
    if ($netstat) {
        Write-Pass "UDP $Port appears bound (netstat)"
        $listening = $true
        foreach ($line in $netstat) {
            Write-Info "  $($line.Line.Trim())"
        }
    } else {
        Write-Info "UDP $Port is NOT bound"
    }
}

# Check TCP too (sometimes confused)
try {
    $tcpListeners = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue
    if ($tcpListeners) {
        Write-Warn "TCP $Port is also in use — DirectRelay uses UDP not TCP"
    }
} catch {}

# ============================================================
# 7. UDP CONNECTIVITY TEST
# ============================================================
Write-Header "7. UDP CONNECTIVITY TEST"

if ($RemoteHost) {
    Write-Info "Testing UDP connectivity to $RemoteHost`:$Port..."
    
    # DNS resolution
    try {
        $resolved = [System.Net.Dns]::GetHostAddresses($RemoteHost)
        Write-Pass "DNS resolved $RemoteHost to: $($resolved -join ', ')"
    } catch {
        Write-Fail "Cannot resolve hostname: $RemoteHost"
        Write-Fix "Check the IP address — use the HOST's local IP (for LAN) or public IP (for internet)"
    }
    
    # UDP send/receive test
    try {
        $udpClient = New-Object System.Net.Sockets.UdpClient
        $udpClient.Client.ReceiveTimeout = 3000
        $udpClient.Client.SendTimeout = 3000
        
        # Send a test packet
        $testData = [System.Text.Encoding]::UTF8.GetBytes("DIRECTRELAY_DIAG_PING")
        $udpClient.Send($testData, $testData.Length, $RemoteHost, $Port) | Out-Null
        Write-Pass "UDP packet SENT to $RemoteHost`:$Port (21 bytes)"
        
        # Try to receive a response (will likely timeout since relay doesn't echo)
        try {
            $ep = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
            $response = $udpClient.Receive([ref]$ep)
            Write-Pass "Received response from $($ep.Address):$($ep.Port)! ($($response.Length) bytes)"
        } catch [System.Net.Sockets.SocketException] {
            if ($_.Exception.SocketErrorCode -eq "TimedOut") {
                Write-Warn "No UDP response within 3s — this is EXPECTED if the relay doesn't echo diagnostic packets"
                Write-Info "The relay only responds to LiteNetLib protocol packets, not raw UDP"
                Write-Info "If the HOST relay console shows NO activity after this test, the packet didn't arrive"
                Write-Fix "Check: (1) firewall on HOST, (2) correct IP, (3) port forwarding if over internet"
            } else {
                Write-Fail "UDP error: $($_.Exception.SocketErrorCode) — $($_.Exception.Message)"
            }
        }
        
        $udpClient.Close()
    } catch {
        Write-Fail "UDP test failed: $($_.Exception.Message)"
        Write-Fix "Check the IP address and ensure UDP is not blocked by router/ISP"
    }
    
    # ICMP ping
    Write-Info "ICMP ping test..."
    try {
        $ping = Test-Connection -ComputerName $RemoteHost -Count 2 -TimeoutSeconds 3 -ErrorAction SilentlyContinue
        if ($ping) {
            $avgMs = ($ping | Measure-Object -Property Latency -Average).Average
            Write-Pass "ICMP ping: $([math]::Round($avgMs, 1)) ms average"
        } else {
            Write-Warn "ICMP ping failed — host may block ping (not necessarily a problem for UDP)"
        }
    } catch {
        Write-Warn "ICMP ping unavailable"
    }
} else {
    Write-Info "No remote host specified. To test connectivity to a relay:"
    Write-Info "  .\diagnose.ps1 -RemoteHost <HOST_IP>"
    Write-Info ""
    Write-Info "HOST: Run this without -RemoteHost to check local config"
    Write-Info "CLIENT: Run with -RemoteHost <host_ip> to test connectivity"
}

# ============================================================
# 8. DIRECTRELAY PROCESS CHECK
# ============================================================
Write-Header "8. DIRECTRELAY PROCESSES"

$relayProcs = Get-Process -Name "DirectRelay*" -ErrorAction SilentlyContinue
$gameProcs = Get-Process -Name "b1-Win64-Shipping" -ErrorAction SilentlyContinue

if ($relayProcs) {
    foreach ($p in $relayProcs) {
        Write-Pass "$($p.ProcessName) is running (PID $($p.Id), started $($p.StartTime.ToString('HH:mm:ss')))"
    }
} else {
    Write-Info "No DirectRelay process running"
}

if ($gameProcs) {
    foreach ($p in $gameProcs) {
        Write-Pass "Game is running (PID $($p.Id), Memory: $([math]::Round($p.WorkingSet64 / 1GB, 1)) GB)"
    }
} else {
    Write-Info "Game is not running"
}

# ============================================================
# 9. GAME LOG FILES
# ============================================================
Write-Header "9. GAME LOG FILES (ReadyMP mod logs)"

$logPaths = @(
    "$env:LOCALAPPDATA\b1\Saved\Logs\b1.log",
    "$env:LOCALAPPDATA\b1\Saved\Logs\ReadyM.log"
)

foreach ($logPath in $logPaths) {
    if (Test-Path $logPath) {
        $logInfo = Get-Item $logPath
        $logAge = (Get-Date) - $logInfo.LastWriteTime
        Write-Info "Found: $logPath ($([math]::Round($logInfo.Length / 1KB)) KB, $([math]::Round($logAge.TotalMinutes)) min ago)"
        
        # Search for connection-related entries
        $logContent = Get-Content $logPath -Tail 200 -ErrorAction SilentlyContinue
        $relevantLines = $logContent | Select-String -Pattern "ReadyM|Relay|Connect|Disconnect|handshake|WukongMp|co.op|error|fail|timeout" -AllMatches
        
        if ($relevantLines) {
            Write-Info "--- Recent Relay/Connection Log Entries ---"
            $shown = 0
            foreach ($line in $relevantLines) {
                if ($shown -lt 30) {
                    Write-Info "  $($line.Line.Trim())"
                    $shown++
                }
            }
            if ($relevantLines.Count -gt 30) {
                Write-Info "  ... ($($relevantLines.Count - 30) more lines)"
            }
        } else {
            Write-Warn "No relay/connection log entries found in recent logs"
        }
    } else {
        Write-Info "Not found: $logPath"
    }
}

# Also check UE4 logs for the mod loading
$ue4LogPath = "$env:LOCALAPPDATA\b1\Saved\Logs\b1.log"
if (Test-Path $ue4LogPath) {
    $ue4Log = Get-Content $ue4LogPath -Tail 500 -ErrorAction SilentlyContinue
    $modLoadLines = $ue4Log | Select-String -Pattern "WukongMp|ReadyM|ICSharpMod|co-op|Multiplayer is disabled" -AllMatches
    if ($modLoadLines) {
        Write-Info "--- Mod Loading Status (from game log) ---"
        $shown = 0
        foreach ($line in $modLoadLines) {
            if ($shown -lt 15) {
                if ($line.Line -match "disabled|error|fail") {
                    Write-Fail "  $($line.Line.Trim())"
                } else {
                    Write-Info "  $($line.Line.Trim())"
                }
                $shown++
            }
        }
    }
}

# ============================================================
# 10. SAVE FILES
# ============================================================
Write-Header "10. SAVE FILES"

$saveBasePath = Join-Path $env:LOCALAPPDATA "b1\Saved\SaveGames"
if (Test-Path $saveBasePath) {
    $steamDirs = Get-ChildItem $saveBasePath -Directory -ErrorAction SilentlyContinue
    if ($steamDirs) {
        foreach ($dir in $steamDirs) {
            Write-Info "Steam ID directory: $($dir.Name)"
            $saves = Get-ChildItem $dir.FullName -Filter "ArchiveFile_*.sav" -ErrorAction SilentlyContinue
            foreach ($save in $saves) {
                $slotNum = if ($save.Name -match 'ArchiveFile_(\d+)_') { $Matches[1] } else { "?" }
                $slotType = switch ($slotNum) {
                    "1" { "Single-player" }
                    "7" { "Co-op Player" }
                    "8" { "Co-op World" }
                    default { "Unknown" }
                }
                Write-Info "  Slot $slotNum ($slotType): $($save.Name) — $([math]::Round($save.Length / 1KB)) KB, $($save.LastWriteTime.ToString('MM/dd HH:mm'))"
            }
        }
    } else {
        Write-Warn "No Steam ID directories found in save path"
    }
} else {
    Write-Warn "Save directory not found: $saveBasePath"
}

# ============================================================
# SUMMARY
# ============================================================
Write-Header "SUMMARY"

Write-Host "  Passed : $script:PassCount" -ForegroundColor Green
Write-Host "  Warnings: $script:WarnCount" -ForegroundColor Yellow
Write-Host "  Failed : $script:FailCount" -ForegroundColor Red
Write-Host ""

if ($script:FailCount -eq 0 -and $script:WarnCount -eq 0) {
    Write-Host "  Everything looks good! If you still can't connect:" -ForegroundColor Green
    Write-Host "  1. Make sure BOTH players run this diagnostic" -ForegroundColor White
    Write-Host "  2. HOST runs DirectRelay.exe first, then CLIENT runs DirectRelayConnect.exe" -ForegroundColor White
    Write-Host "  3. Check the relay console for [CONNECT] messages after the game launches" -ForegroundColor White
} elseif ($script:FailCount -gt 0) {
    Write-Host "  ISSUES FOUND — fix the [FAIL] items above before trying to connect" -ForegroundColor Red
    Write-Host ""
    Write-Host "  Most common fixes:" -ForegroundColor Yellow
    Write-Host "  1. Run: .\diagnose.ps1 -FixFirewall  (creates UDP firewall rule)" -ForegroundColor White
    Write-Host "  2. Check that both PCs can reach each other (same network or port forwarding)" -ForegroundColor White
    Write-Host "  3. Verify mod DLLs are installed to the game directory" -ForegroundColor White
} else {
    Write-Host "  Some warnings detected — these may or may not cause problems" -ForegroundColor Yellow
}

# Save diagnostic log
$logFile = Join-Path $PSScriptRoot "diagnostic_results.txt"
try {
    $header = @(
        "DirectRelay Diagnostic Results",
        "Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')",
        "Machine: $env:COMPUTERNAME",
        "User: $env:USERNAME",
        ""
    )
    ($header + $script:DiagLog) | Out-File $logFile -Encoding UTF8
    Write-Host ""
    Write-Host "  Results saved to: $logFile" -ForegroundColor DarkGray
    Write-Host "  Share this file when asking for help" -ForegroundColor DarkGray
} catch {
    Write-Warn "Could not save diagnostic log"
}

Write-Host ""
Write-Host "  Press any key to exit..." -ForegroundColor DarkGray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
