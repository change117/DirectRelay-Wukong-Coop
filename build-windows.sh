#!/bin/bash

echo "=========================================="
echo "DirectRelay Windows Build Script"
echo "=========================================="
echo ""

# Set colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Define paths
SOURCE_DIR="/workspaces/DirectRelay-Wukong-Coop/src"
PUBLISH_DIR="/workspaces/DirectRelay-Wukong-Coop/publish"
MODS_SOURCE_HOST="/workspaces/DirectRelay-Wukong-Coop/HOST/mods"
MODS_SOURCE_CLIENT="/workspaces/DirectRelay-Wukong-Coop/CLIENT/mods"

# Clean previous builds
echo -e "${BLUE}[1/6]${NC} Cleaning previous builds..."
rm -rf "$PUBLISH_DIR"
mkdir -p "$PUBLISH_DIR"
echo -e "${GREEN}✓${NC} Clean complete"
echo ""

# Build HOST (Server)
echo -e "${BLUE}[2/6]${NC} Building DirectRelay HOST for Windows..."
dotnet publish "$SOURCE_DIR/DirectRelay/DirectRelay.csproj" \
    -c Release \
    -r win-x64 \
    --self-contained \
    -o "$PUBLISH_DIR/HOST" \
    /p:PublishSingleFile=false \
    /p:DebugType=None \
    /p:DebugSymbols=false

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓${NC} HOST build successful"
else
    echo -e "${RED}✗${NC} HOST build failed!"
    exit 1
fi
echo ""

# Build CLIENT
echo -e "${BLUE}[3/6]${NC} Building DirectRelayConnect CLIENT for Windows..."
dotnet publish "$SOURCE_DIR/DirectRelayConnect/DirectRelayConnect.csproj" \
    -c Release \
    -r win-x64 \
    --self-contained \
    -o "$PUBLISH_DIR/CLIENT" \
    /p:PublishSingleFile=false \
    /p:DebugType=None \
    /p:DebugSymbols=false

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓${NC} CLIENT build successful"
else
    echo -e "${RED}✗${NC} CLIENT build failed!"
    exit 1
fi
echo ""

# Copy mods folder to HOST
echo -e "${BLUE}[4/6]${NC} Copying mods to HOST..."
if [ -d "$MODS_SOURCE_HOST" ]; then
    cp -r "$MODS_SOURCE_HOST" "$PUBLISH_DIR/HOST/"
    echo -e "${GREEN}✓${NC} HOST mods copied"
else
    echo -e "${RED}⚠${NC} Warning: HOST mods folder not found at $MODS_SOURCE_HOST"
fi
echo ""

# Copy mods folder to CLIENT
echo -e "${BLUE}[5/6]${NC} Copying mods to CLIENT..."
if [ -d "$MODS_SOURCE_CLIENT" ]; then
    cp -r "$MODS_SOURCE_CLIENT" "$PUBLISH_DIR/CLIENT/"
    echo -e "${GREEN}✓${NC} CLIENT mods copied"
else
    echo -e "${RED}⚠${NC} Warning: CLIENT mods folder not found at $MODS_SOURCE_CLIENT"
fi
echo ""

# Create release ZIP
echo -e "${BLUE}[6/6]${NC} Creating Windows release package..."
cd "$PUBLISH_DIR"
zip -r -q DirectRelay-Windows-v1.4.0.zip HOST/ CLIENT/

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓${NC} Release package created successfully!"
else
    echo -e "${RED}✗${NC} Failed to create ZIP file!"
    exit 1
fi
echo ""

# Show summary
echo "=========================================="
echo -e "${GREEN}BUILD COMPLETE!${NC}"
echo "=========================================="
echo ""
echo "📦 Release package location:"
echo "   $PUBLISH_DIR/DirectRelay-Windows-v1.4.0.zip"
echo ""
echo "📁 Build outputs:"
echo "   HOST:   $PUBLISH_DIR/HOST/DirectRelay.exe"
echo "   CLIENT: $PUBLISH_DIR/CLIENT/DirectRelayConnect.exe"
echo ""
echo "🎉 Ready to release on GitHub!"
echo ""
echo "Next steps:"
echo "  1. Download DirectRelay-Windows-v1.4.0.zip from Codespaces"
echo "  2. Test on Windows to verify it works"
echo "  3. Upload to GitHub releases"
echo ""
