# Download Instructions for DirectRelay-Wukong-Coop

## Your Build is Ready! 🎮

Your repository already contains pre-built executables that are ready to use. Since you've reset your PC and don't have development tools, here's how to get your application:

## Quick Download Links

**Download the entire repository as a ZIP file:**
1. Go to: https://github.com/change117/DirectRelay-Wukong-Coop
2. Click the green **"Code"** button
3. Click **"Download ZIP"**
4. Extract the ZIP file to a folder on your PC

## What You'll Get

After extracting, you'll have two main folders ready to use:

### 🏠 HOST Folder (For the person hosting the game)
```
HOST/
├── DirectRelay.exe          ← Run this (65 MB)
├── DirectRelay.pdb
├── diagnose.ps1             ← Troubleshooting script
└── mods/                    ← Game mod files (auto-installed)
```

**Size:** ~65 MB executable + mods
**Requires:** Windows (no .NET installation needed - self-contained!)

### 👥 CLIENT Folder (For the person joining)
```
CLIENT/
├── DirectRelayConnect.exe   ← Run this (65 MB)
├── DirectRelayConnect.pdb
├── diagnose.ps1             ← Troubleshooting script
└── mods/                    ← Game mod files (auto-installed)
```

**Size:** ~65 MB executable + mods
**Requires:** Windows (no .NET installation needed - self-contained!)

## How to Use

### For HOST (Person running the server):
1. Navigate to the `HOST` folder
2. Double-click `DirectRelay.exe`
3. A GUI control panel will appear
4. Click **"Launch Game & Start Server"**
5. Share your IP address with your friend

### For CLIENT (Person joining):
1. Navigate to the `CLIENT` folder
2. Double-click `DirectRelayConnect.exe`
3. Enter the host's IP address when prompted
4. Click connect and the game will launch

## Alternative: Clone with Git

If you install Git later, you can clone the repository:
```bash
git clone https://github.com/change117/DirectRelay-Wukong-Coop.git
cd DirectRelay-Wukong-Coop
```

## Build Information

- **Build Date:** February 17, 2026
- **Version:** Latest (main branch)
- **.NET Target:** .NET 8.0 (included in executables - no installation needed)
- **Platform:** Windows x64
- **Build Type:** Release (optimized for performance)

## Files Included

✅ Self-contained executables (no .NET runtime installation required)  
✅ All mod files for Black Myth: Wukong  
✅ Configuration files  
✅ Diagnostic scripts  
✅ Complete source code (in `src/` folder if you need it later)

## Need to Rebuild from Source?

If you later install Visual Studio or .NET 8 SDK, you can rebuild from source:
```bash
cd src
dotnet build DirectRelay-Wukong-Coop.sln -c Release
```

## Support

For issues or questions, please open an issue on GitHub:
https://github.com/change117/DirectRelay-Wukong-Coop/issues

---

**Enjoy playing Black Myth: Wukong with your friends!** 🎮🐵
