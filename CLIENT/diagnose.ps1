# ============================================================
# DirectRelay CLIENT Diagnostics
# Run this on the CLIENT machine to check network/firewall/game
# Usage: Right-click -> Run with PowerShell (or open PS and .\diagnose.ps1)
# ============================================================
param(
    [string]$HostIP = "",
    [int]$Port = 7777
)

$ErrorActionPreference = "Continue"
$pass = 0; $fail = 0; $warn = 0

function Write-Check($label, $status, $detail) {
    switch ($status) {
        "PASS" { Write-Host "  [PASS] " -ForegroundColor Green -NoNewline; $script:pass++ }
        "FAIL" { Write-Host "  [FAIL] " -ForegroundColor Red -NoNewline; $script:fail++ }
        "WARN" { Write-Host "  [WARN] " -ForegroundColor Yellow -NoNewline; $script:warn++ }
        "INFO" { Write-Host "  [INFO] " -ForegroundColor Cyan -NoNewline }
    }
    Write-Host "$label" -NoNewline
    if ($detail) { Write-Host " - $detail" -ForegroundColor DarkGray } else { Write-Host "" }
}

Write-Host ""
Write-Host "========================================================" -ForegroundColor White
Write-Host "   DirectRelay CLIENT Diagnostics                       " -ForegroundColor Cyan
Write-Host "========================================================" -ForegroundColor White
Write-Host ""

if (-not $HostIP) {
    $HostIP = Read-Host "  Enter HOST IP address"
}
Write-Host "  Target: ${HostIP}:${Port}" -ForegroundColor DarkCyan
Write-Host ""

# --- 1. Network Adapters ---
Write-Host "[1/8] Network Adapters" -ForegroundColor Yellow
try {
    $adapters = Get-NetAdapter -Physical -ErrorAction SilentlyContinue | Where-Object Status -eq "Up"
    if ($adapters) {
        foreach ($a in $adapters) {
            $ips = (Get-NetIPAddress -InterfaceIndex $a.ifIndex -AddressFamily IPv4 -ErrorAction SilentlyContinue).IPAddress
            Write-Check $a.Name "PASS" "IPs: $($ips -join ', ')"
        }
    } else {
        Write-Check "No active physical adapters" "FAIL"
    }
} catch { Write-Check "Could not enumerate adapters" "WARN" $_.Exception.Message }

# --- 2. DNS Resolution ---
Write-Host ""
Write-Host "[2/8] DNS / IP Resolution" -ForegroundColor Yellow
$resolvedIP = $null
try {
    if ([System.Net.IPAddress]::TryParse($HostIP, [ref]$null)) {
        Write-Check "IP address format valid" "PASS" $HostIP
        $resolvedIP = $HostIP
    } else {
        $addrs = [System.Net.Dns]::GetHostAddresses($HostIP)
        $resolvedIP = ($addrs | Where-Object { $_.AddressFamily -eq 'InterNetwork' } | Select-Object -First 1).ToString()
        Write-Check "Hostname resolved" "PASS" "$HostIP -> $resolvedIP"
    }
} catch {
    Write-Check "DNS resolution FAILED for '$HostIP'" "FAIL" $_.Exception.Message
}

# --- 3. ICMP Ping ---
Write-Host ""
Write-Host "[3/8] Ping Test" -ForegroundColor Yellow
if ($resolvedIP) {
    try {
        $ping = Test-Connection -ComputerName $resolvedIP -Count 3 -ErrorAction SilentlyContinue
        if ($ping) {
            $avg = ($ping | Measure-Object -Property ResponseTime -Average).Average
            Write-Check "Ping OK" "PASS" "Average: $([math]::Round($avg,1))ms"
        } else {
            Write-Check "Ping FAILED" "WARN" "ICMP may be blocked (this doesn't always mean UDP won't work)"
        }
    } catch {
        Write-Check "Ping test error" "WARN" $_.Exception.Message
    }
} else {
    Write-Check "Skipped (no resolved IP)" "WARN"
}

# --- 4. UDP Connectivity Test ---
Write-Host ""
Write-Host "[4/8] UDP Connectivity to Relay" -ForegroundColor Yellow
if ($resolvedIP) {
    try {
        $udp = New-Object System.Net.Sockets.UdpClient
        $udp.Client.ReceiveTimeout = 3000
        $udp.Client.SendTimeout = 3000
        $udp.Connect($resolvedIP, $Port)
        $localEP = $udp.Client.LocalEndPoint
        Write-Check "UDP socket opened" "PASS" "Local: $localEP"

        # Send diagnostic ping
        $pingBytes = [System.Text.Encoding]::UTF8.GetBytes("DIRECTRELAY_DIAG_PING")
        [void]$udp.Send($pingBytes, $pingBytes.Length)
        Write-Check "Diagnostic ping sent" "INFO"

        try {
            $remoteEP = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
            $response = $udp.Receive([ref]$remoteEP)
            $responseStr = [System.Text.Encoding]::UTF8.GetString($response)
            
            if ($responseStr -match "DIRECTRELAY_DIAG_PONG") {
                Write-Check "RELAY IS ALIVE AND RESPONDING!" "PASS"
                
                # Parse fields
                $fields = $responseStr -split '\|'
                foreach ($f in $fields) {
                    if ($f -match "^(players|uptime|areas|port|version)=(.+)$") {
                        Write-Check "  $($Matches[1]): $($Matches[2])" "INFO"
                    }
                }
            } else {
                Write-Check "Got response but not a diagnostic pong" "WARN" "$($response.Length) bytes"
            }
        } catch [System.Net.Sockets.SocketException] {
            if ($_.Exception.SocketErrorCode -eq 'TimedOut') {
                Write-Check "NO RESPONSE from relay (3s timeout)" "FAIL"
                Write-Host "    Possible causes:" -ForegroundColor DarkYellow
                Write-Host "      1. DirectRelay.exe is NOT running on the host" -ForegroundColor Yellow
                Write-Host "      2. Wrong IP address" -ForegroundColor Yellow
                Write-Host "      3. Host firewall blocking inbound UDP $Port" -ForegroundColor Yellow
                Write-Host "      4. Router NAT not forwarding port $Port (if over internet)" -ForegroundColor Yellow
            } else {
                Write-Check "UDP error: $($_.Exception.SocketErrorCode)" "FAIL"
            }
        }
        $udp.Close()
    } catch {
        Write-Check "UDP test failed" "FAIL" $_.Exception.Message
    }
} else {
    Write-Check "Skipped (no resolved IP)" "WARN"
}

# --- 5. Firewall ---
Write-Host ""
Write-Host "[5/8] Windows Firewall Rules" -ForegroundColor Yellow
try {
    $rules = Get-NetFirewallRule -DisplayName "*DirectRelay*" -ErrorAction SilentlyContinue
    if ($rules) {
        foreach ($r in $rules) {
            $portFilter = Get-NetFirewallPortFilter -AssociatedNetFirewallRule $r -ErrorAction SilentlyContinue
            Write-Check "$($r.DisplayName)" "PASS" "Action=$($r.Action) Dir=$($r.Direction)"
        }
    } else {
        Write-Check "No 'DirectRelay' firewall rules found" "INFO" "Client usually doesn't need inbound rules"
    }

    # Check outbound isn't blocked
    $outBlocked = Get-NetFirewallRule -Direction Outbound -Action Block -Enabled True -ErrorAction SilentlyContinue |
        Where-Object { $_.DisplayName -match "DirectRelay|Wukong|b1-Win64" }
    if ($outBlocked) {
        foreach ($r in $outBlocked) {
            Write-Check "OUTBOUND BLOCK: $($r.DisplayName)" "FAIL" "This may prevent the game from connecting!"
        }
    } else {
        Write-Check "No outbound blocks found for DirectRelay/game" "PASS"
    }
} catch {
    Write-Check "Could not query firewall" "WARN" $_.Exception.Message
}

# --- 6. Game Installation ---
Write-Host ""
Write-Host "[6/8] Game Installation" -ForegroundColor Yellow
$gamePaths = @(
    "C:\Program Files (x86)\Steam\steamapps\common\Black Myth Wukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
    "D:\SteamLibrary\steamapps\common\Black Myth Wukong\b1\Binaries\Win64\b1-Win64-Shipping.exe",
    "E:\SteamLibrary\steamapps\common\Black Myth Wukong\b1\Binaries\Win64\b1-Win64-Shipping.exe"
)
$gameFound = $false
foreach ($gp in $gamePaths) {
    if (Test-Path $gp) {
        Write-Check "Game found" "PASS" $gp
        $gameFound = $true
        break
    }
}
if (-not $gameFound) {
    Write-Check "Game not found in common paths" "WARN" "Will be auto-detected at runtime via registry"
}

# --- 7. Handshake File ---
Write-Host ""
Write-Host "[7/8] Handshake File" -ForegroundColor Yellow
$handshakePath = Join-Path $env:APPDATA "ReadyM.Launcher\wukong_handshake.env"
if (Test-Path $handshakePath) {
    $content = Get-Content $handshakePath -Raw
    Write-Check "Handshake file EXISTS" "PASS" $handshakePath
    $content -split "`n" | ForEach-Object {
        $line = $_.Trim()
        if ($line -and -not $line.StartsWith("#")) {
            Write-Check "  $line" "INFO"
        }
    }
    Write-Host ""
    Write-Host "  NOTE: The game DELETES this file when it reads it." -ForegroundColor DarkYellow
} else {
    Write-Check "Handshake file does not exist" "INFO" "Normal if game already started or not launched yet"
}

# --- 8. Mod DLLs ---
Write-Host ""
Write-Host "[8/8] Mod DLLs (CLIENT/mods folder)" -ForegroundColor Yellow
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$modsDir = Join-Path $scriptDir "mods"
$requiredMods = @(
    "ReadyM.Relay.Client.dll",
    "ReadyM.Relay.Common.dll",
    "ReadyM.Relay.Common.Wukong.dll",
    "ReadyM.Launcher.Core.dll",
    "ReadyM.Core.dll",
    "WukongMp.Coop.dll",
    "WukongMp.Api.dll"
)

if (Test-Path $modsDir) {
    foreach ($mod in $requiredMods) {
        $modPath = Get-ChildItem -Path $modsDir -Recurse -Filter $mod -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($modPath) {
            Write-Check $mod "PASS" "$([math]::Round($modPath.Length/1KB,1))KB"
        } else {
            Write-Check $mod "FAIL" "MISSING from mods folder!"
        }
    }
} else {
    Write-Check "mods/ folder not found" "FAIL" "Expected at: $modsDir"
}

# --- Summary ---
Write-Host ""
Write-Host "========================================================" -ForegroundColor White
Write-Host "  Results: $pass PASS | $warn WARN | $fail FAIL" -ForegroundColor $(if ($fail -gt 0) { "Red" } elseif ($warn -gt 0) { "Yellow" } else { "Green" })
Write-Host "========================================================" -ForegroundColor White

if ($fail -gt 0) {
    Write-Host ""
    Write-Host "  FIX REQUIRED: Address the FAIL items above." -ForegroundColor Red
    Write-Host "  Most common fix: Make sure DirectRelay.exe is running on HOST" -ForegroundColor Yellow
    Write-Host "  and UDP port $Port is not blocked by the HOST's firewall." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "  Tip: Share this output with your co-op partner to compare." -ForegroundColor DarkGray
Write-Host "  Tip: Also check the .log files in DirectRelayConnect's folder for session logs." -ForegroundColor DarkGray
Write-Host ""
Read-Host "Press Enter to exit"
