using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;

namespace DirectRelayConnect;

public class SaveManager
{
    private static readonly string SaveBasePath = 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                     "b1", "Saved", "SaveGames");

    public class SaveInfo
    {
        public bool Slot1Exists { get; set; }
        public bool Slot7Exists { get; set; }
        public bool Slot8Exists { get; set; }
        public DateTime? Slot1Modified { get; set; }
        public DateTime? Slot7Modified { get; set; }
        public DateTime? Slot8Modified { get; set; }
        public string? SaveDirectory { get; set; }
    }

    public class SaveMetadata
    {
        public DateTime BackupTime { get; set; }
        public string MachineName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Version { get; set; } = "1.0";
    }

    public static SaveInfo GetSaveStatus()
    {
        var info = new SaveInfo();

        if (!Directory.Exists(SaveBasePath))
        {
            return info;
        }

        // Find the first Steam ID directory
        var steamDirs = Directory.GetDirectories(SaveBasePath);
        if (steamDirs.Length == 0)
        {
            return info;
        }

        var saveDir = steamDirs[0]; // Use first Steam ID found
        info.SaveDirectory = saveDir;

        // Check for save files
        var saves = Directory.GetFiles(saveDir, "ArchiveFile_*.sav");
        foreach (var save in saves)
        {
            var fileName = Path.GetFileName(save);
            var fileInfo = new FileInfo(save);

            if (fileName.StartsWith("ArchiveFile_1_"))
            {
                info.Slot1Exists = true;
                info.Slot1Modified = fileInfo.LastWriteTime;
            }
            else if (fileName.StartsWith("ArchiveFile_7_"))
            {
                info.Slot7Exists = true;
                info.Slot7Modified = fileInfo.LastWriteTime;
            }
            else if (fileName.StartsWith("ArchiveFile_8_"))
            {
                info.Slot8Exists = true;
                info.Slot8Modified = fileInfo.LastWriteTime;
            }
        }

        return info;
    }

    public static void UploadSave(string backupFilePath, Action<string, string, ConsoleColor> logger)
    {
        logger("SAVE", $"Extracting save archive: {Path.GetFileName(backupFilePath)}", ConsoleColor.Yellow);

        if (!File.Exists(backupFilePath))
        {
            logger("SAVE", "ERROR: Backup file not found!", ConsoleColor.Red);
            return;
        }

        // Verify it's a valid ZIP
        try
        {
            using var zip = ZipFile.OpenRead(backupFilePath);
            var saveFiles = zip.Entries.Where(e => e.Name.EndsWith(".sav")).ToList();
            
            if (saveFiles.Count == 0)
            {
                logger("SAVE", "ERROR: No .sav files found in backup!", ConsoleColor.Red);
                return;
            }

            logger("SAVE", $"Found {saveFiles.Count} save file(s) in archive", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            logger("SAVE", $"ERROR: Invalid backup file: {ex.Message}", ConsoleColor.Red);
            return;
        }

        // Find or create save directory
        if (!Directory.Exists(SaveBasePath))
        {
            logger("SAVE", $"ERROR: Save directory not found: {SaveBasePath}", ConsoleColor.Red);
            logger("SAVE", "Make sure the game has been run at least once.", ConsoleColor.Yellow);
            return;
        }

        var steamDirs = Directory.GetDirectories(SaveBasePath);
        if (steamDirs.Length == 0)
        {
            logger("SAVE", "ERROR: No Steam ID directory found in save path!", ConsoleColor.Red);
            logger("SAVE", "Run the game at least once to create the save directory.", ConsoleColor.Yellow);
            return;
        }

        var targetDir = steamDirs[0];
        logger("SAVE", $"Target directory: {targetDir}", ConsoleColor.DarkCyan);

        // Extract save files
        try
        {
            using var zip = ZipFile.OpenRead(backupFilePath);
            foreach (var entry in zip.Entries)
            {
                if (entry.Name.EndsWith(".sav"))
                {
                    var destPath = Path.Combine(targetDir, entry.Name);
                    entry.ExtractToFile(destPath, true);
                    
                    var fileInfo = new FileInfo(destPath);
                    logger("SAVE", $"  ✓ Restored {entry.Name} ({fileInfo.Length / (1024.0 * 1024.0):F1} MB)", ConsoleColor.Green);
                }
            }

            logger("SAVE", "Save upload complete!", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            logger("SAVE", $"ERROR during extraction: {ex.Message}", ConsoleColor.Red);
        }
    }

    public static void DownloadSave(string outputPath, Action<string, string, ConsoleColor> logger)
    {
        logger("SAVE", "Creating save backup...", ConsoleColor.Yellow);

        var saveInfo = GetSaveStatus();
        if (saveInfo.SaveDirectory == null)
        {
            logger("SAVE", "ERROR: No save files found!", ConsoleColor.Red);
            logger("SAVE", $"Checked: {SaveBasePath}", ConsoleColor.DarkYellow);
            return;
        }

        logger("SAVE", $"Source: {saveInfo.SaveDirectory}", ConsoleColor.DarkCyan);

        try
        {
            // Create metadata
            var metadata = new SaveMetadata
            {
                BackupTime = DateTime.Now,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                Version = "1.0"
            };

            // Create ZIP archive
            using var zipStream = File.Create(outputPath);
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);

            // Add save files
            var saves = Directory.GetFiles(saveInfo.SaveDirectory, "ArchiveFile_*.sav");
            foreach (var save in saves)
            {
                var fileName = Path.GetFileName(save);
                var entry = archive.CreateEntry(fileName);
                
                using var saveStream = File.OpenRead(save);
                using var entryStream = entry.Open();
                saveStream.CopyTo(entryStream);

                var fileInfo = new FileInfo(save);
                logger("SAVE", $"  ✓ Added {fileName} ({fileInfo.Length / (1024.0 * 1024.0):F1} MB)", ConsoleColor.Green);
            }

            // Add metadata
            var metadataEntry = archive.CreateEntry("metadata.json");
            using var metadataStream = metadataEntry.Open();
            using var writer = new StreamWriter(metadataStream);
            var json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
            writer.Write(json);

            logger("SAVE", $"Backup saved to: {outputPath}", ConsoleColor.Green);
            logger("SAVE", $"Total files: {saves.Length} save(s) + metadata.json", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            logger("SAVE", $"ERROR: Failed to create backup: {ex.Message}", ConsoleColor.Red);
        }
    }
}
