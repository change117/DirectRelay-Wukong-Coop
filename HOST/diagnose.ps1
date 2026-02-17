# ============================================================
# DirectRelay HOST Diagnostics
# Run this on the HOST machine to check network/firewall/game
# Usage: Right-click -> Run with PowerShell (or open PS and .\diagnose.ps1)
# ============================================================
param(
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
Write-Host "   DirectRelay HOST Diagnostics                         " -ForegroundColor Cyan
Write-Host "   Port: $Port                                          " -ForegroundColor DarkCyan
Write-Host "========================================================" -ForegroundColor White
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

# --- 2. Local IPs ---
Write-Host ""
Write-Host "[2/8] Local IP Addresses" -ForegroundColor Yellow
try {
    $localIPs = [System.Net.Dns]::GetHostAddresses([System.Net.Dns]::GetHostName()) |
        Where-Object { $_.AddressFamily -eq 'InterNetwork' -and $_.ToString() -ne '127.0.0.1' }
    foreach ($ip in $localIPs) {
        Write-Check "LAN IP: $($ip.ToString())" "INFO" "Give this to your friend if on same network"
    }
    if (-not $localIPs) { Write-Check "No LAN IPv4 addresses found" "FAIL" }
} catch { Write-Check "Could not detect local IPs" "WARN" }

# Public IP
try {
    $publicIp = (Invoke-WebRequest -Uri "https://api.ipify.org" -TimeoutSec 5 -UseBasicParsing).Content.Trim()
    Write-Check "Public IP: $publicIp" "INFO" "Give this to your friend if on different networks"
} catch { Write-Check "Could not determine public IP" "WARN" }

# --- 3. Port in use ---
Write-Host ""
Write-Host "[3/8] Port $Port Availability" -ForegroundColor Yellow
$portInUse = netstat -an | Select-String ":$Port\s" | Select-String "UDP"
if ($portInUse) {
    Write-Check "UDP port $Port is bound" "PASS" "Something is listening (should be DirectRelay)"
    $portInUse | ForEach-Object { Write-Check "  $_" "INFO" }
} else {
    Write-Check "UDP port $Port is NOT bound" "WARN" "DirectRelay.exe may not be running yet"
}

# --- 4. Firewall ---
Write-Host ""
Write-Host "[4/8] Windows Firewall Rules" -ForegroundColor Yellow
try {
    $rules = Get-NetFirewallRule -DisplayName "*DirectRelay*" -ErrorAction SilentlyContinue
    if ($rules) {
        foreach ($r in $rules) {
            $portFilter = Get-NetFirewallPortFilter -AssociatedNetFirewallRule $r -ErrorAction SilentlyContinue
            Write-Check "$($r.DisplayName)" "PASS" "Action=$($r.Action) Dir=$($r.Direction) Proto=$($portFilter.Protocol) Port=$($portFilter.LocalPort)"
        }
    } else {
        Write-Check "No 'DirectRelay' firewall rules found" "WARN" "DirectRelay.exe should auto-create one on first run"
    }

    # Check for any rule allowing inbound UDP on our port
    $allInbound = Get-NetFirewallRule -Direction Inbound -Action Allow -Enabled True -ErrorAction SilentlyContinue
    $matchingPort = $false
    foreach ($rule in $allInbound) {
        $pf = Get-NetFirewallPortFilter -AssociatedNetFirewallRule $rule -ErrorAction SilentlyContinue
        if ($pf.Protocol -eq "UDP" -and ($pf.LocalPort -eq $Port -or $pf.LocalPort -eq "Any")) {
            $matchingPort = $true
            break
        }
    }
    if ($matchingPort) {
        Write-Check "Inbound UDP $Port is ALLOWED by at least one rule" "PASS"
    } else {
        Write-Check "No firewall rule allows inbound UDP $Port" "FAIL" "Run: netsh advfirewall firewall add rule name=`"DirectRelay UDP $Port`" dir=in action=allow protocol=UDP localport=$Port"
    }
} catch {
    Write-Check "Could not query firewall" "WARN" $_.Exception.Message
}

# --- 5. DirectRelay Process ---
Write-Host ""
Write-Host "[5/8] DirectRelay Process" -ForegroundColor Yellow
$proc = Get-Process -Name "DirectRelay" -ErrorAction SilentlyContinue
if ($proc) {
    foreach ($p in $proc) {
        Write-Check "DirectRelay.exe is running" "PASS" "PID=$($p.Id) CPU=$($p.CPU)s WorkingSet=$([math]::Round($p.WorkingSet64/1MB,1))MB"
    }
} else {
    Write-Check "DirectRelay.exe is NOT running" "WARN" "Start it before running diagnostics for full check"
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
# Also try registry
if (-not $gameFound) {
    try {
        $steamPath = (Get-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Valve\Steam" -Name InstallPath -ErrorAction SilentlyContinue).InstallPath
        if ($steamPath) {
            $libFolders = Get-Content "$steamPath\steamapps\libraryfolders.vdf" -ErrorAction SilentlyContinue
            Write-Check "Steam found at $steamPath" "INFO" "Check library folders for game"
        }
    } catch {}
    
    if (-not $gameFound) {
        Write-Check "Game not found in common paths" "WARN" "Will be auto-detected at runtime via registry"
    }
}

# --- 7. Handshake File ---
Write-Host ""
Write-Host "[7/8] Handshake File" -ForegroundColor Yellow
$handshakePath = Join-Path $env:APPDATA "ReadyM.Launcher\wukong_handshake.env"
if (Test-Path $handshakePath) {
    $content = Get-Content $handshakePath -Raw
    Write-Check "Handshake file EXISTS" "PASS" $handshakePath
    # Parse key fields
    $content -split "`n" | ForEach-Object {
        $line = $_.Trim()
        if ($line -and -not $line.StartsWith("#")) {
            Write-Check "  $line" "INFO"
        }
    }
    Write-Host ""
    Write-Host "  NOTE: The game DELETES this file when it reads it." -ForegroundColor DarkYellow
    Write-Host "  If you see it here, the game hasn't read it yet (or wasn't launched)." -ForegroundColor DarkYellow
} else {
    Write-Check "Handshake file does not exist" "INFO" "Normal if game already started (it deletes the file)"
}

# --- 8. Mod DLLs ---
Write-Host ""
Write-Host "[8/8] Mod DLLs (HOST/mods folder)" -ForegroundColor Yellow
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
    Write-Host "  FIX REQUIRED: Address the FAIL items above before trying to connect." -ForegroundColor Red
}

Write-Host ""
Write-Host "  Tip: Share this output with your co-op partner to compare." -ForegroundColor DarkGray
Write-Host "  Tip: Also check the .log files in DirectRelay's folder for session logs." -ForegroundColor DarkGray
Write-Host ""
Read-Host "Press Enter to exit"
