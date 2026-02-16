using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DirectRelay;

public class ControlPanelWindow : Form
{
    private readonly ManualResetEvent _launchSignal;
    private readonly Action<string, string, ConsoleColor> _logger;
    
    private Label _statusLabel = null!;
    private Label _slot1Label = null!;
    private Label _slot7Label = null!;
    private Label _slot8Label = null!;
    private Button _uploadButton = null!;
    private Button _downloadButton = null!;
    private Button _launchButton = null!;
    private Label _footerLabel = null!;

    public ControlPanelWindow(ManualResetEvent launchSignal, Action<string, string, ConsoleColor> logger)
    {
        _launchSignal = launchSignal;
        _logger = logger;
        InitializeComponents();
        UpdateSaveStatus();
    }

    private void InitializeComponents()
    {
        // Form properties
        this.Text = "DirectRelay Control Panel";
        this.Size = new Size(420, 380);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 240, 240);

        // Title
        var titleLabel = new Label
        {
            Text = "DirectRelay Control Panel (HOST)",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Location = new Point(20, 20),
            Size = new Size(360, 30),
            TextAlign = ContentAlignment.MiddleLeft
        };
        this.Controls.Add(titleLabel);

        // Separator
        var separator1 = new Panel
        {
            Location = new Point(20, 55),
            Size = new Size(360, 2),
            BackColor = Color.FromArgb(200, 200, 200)
        };
        this.Controls.Add(separator1);

        // Save Management Section
        var saveLabel = new Label
        {
            Text = "📁 Save Management",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Location = new Point(20, 70),
            Size = new Size(360, 25),
            TextAlign = ContentAlignment.MiddleLeft
        };
        this.Controls.Add(saveLabel);

        // Current Save Status
        var statusTitleLabel = new Label
        {
            Text = "Current Save Status:",
            Font = new Font("Segoe UI", 9),
            Location = new Point(40, 100),
            Size = new Size(360, 20)
        };
        this.Controls.Add(statusTitleLabel);

        _slot1Label = new Label
        {
            Text = "  ? Slot 1: Unknown",
            Font = new Font("Consolas", 9),
            Location = new Point(40, 125),
            Size = new Size(360, 20),
            ForeColor = Color.Gray
        };
        this.Controls.Add(_slot1Label);

        _slot7Label = new Label
        {
            Text = "  ? Slot 7: Unknown",
            Font = new Font("Consolas", 9),
            Location = new Point(40, 145),
            Size = new Size(360, 20),
            ForeColor = Color.Gray
        };
        this.Controls.Add(_slot7Label);

        _slot8Label = new Label
        {
            Text = "  ? Slot 8: Unknown",
            Font = new Font("Consolas", 9),
            Location = new Point(40, 165),
            Size = new Size(360, 20),
            ForeColor = Color.Gray
        };
        this.Controls.Add(_slot8Label);

        // Upload Button
        _uploadButton = new Button
        {
            Text = "📤 Upload Save",
            Font = new Font("Segoe UI", 10),
            Location = new Point(40, 200),
            Size = new Size(320, 35),
            BackColor = Color.FromArgb(0, 120, 215),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        _uploadButton.FlatAppearance.BorderSize = 0;
        _uploadButton.Click += OnUploadClick;
        this.Controls.Add(_uploadButton);

        // Download Button
        _downloadButton = new Button
        {
            Text = "📥 Download Save",
            Font = new Font("Segoe UI", 10),
            Location = new Point(40, 245),
            Size = new Size(320, 35),
            BackColor = Color.FromArgb(0, 120, 215),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        _downloadButton.FlatAppearance.BorderSize = 0;
        _downloadButton.Click += OnDownloadClick;
        this.Controls.Add(_downloadButton);

        // Launch Button
        _launchButton = new Button
        {
            Text = "▶️ Launch Game & Start Server",
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            Location = new Point(40, 290),
            Size = new Size(320, 40),
            BackColor = Color.FromArgb(16, 124, 16),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        _launchButton.FlatAppearance.BorderSize = 0;
        _launchButton.Click += OnLaunchClick;
        this.Controls.Add(_launchButton);

        // Status Footer
        _footerLabel = new Label
        {
            Text = "Status: Ready",
            Font = new Font("Segoe UI", 9),
            Location = new Point(20, 340),
            Size = new Size(360, 20),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.Gray
        };
        this.Controls.Add(_footerLabel);

        // Handle form closing
        this.FormClosing += (s, e) =>
        {
            if (!_launchSignal.WaitOne(0))
            {
                var result = MessageBox.Show(
                    "Are you sure you want to exit without launching the game?",
                    "Confirm Exit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        };
    }

    private void UpdateSaveStatus()
    {
        try
        {
            var info = SaveManager.GetSaveStatus();

            if (info.Slot1Exists && info.Slot1Modified.HasValue)
            {
                var timeAgo = GetTimeAgo(info.Slot1Modified.Value);
                _slot1Label.Text = $"  ✓ Slot 1: {timeAgo}";
                _slot1Label.ForeColor = Color.Green;
            }
            else
            {
                _slot1Label.Text = "  ✗ Slot 1: Not found";
                _slot1Label.ForeColor = Color.Gray;
            }

            if (info.Slot7Exists && info.Slot7Modified.HasValue)
            {
                var timeAgo = GetTimeAgo(info.Slot7Modified.Value);
                _slot7Label.Text = $"  ✓ Slot 7: {timeAgo}";
                _slot7Label.ForeColor = Color.Green;
            }
            else
            {
                _slot7Label.Text = "  ✗ Slot 7: Not found";
                _slot7Label.ForeColor = Color.Gray;
            }

            if (info.Slot8Exists && info.Slot8Modified.HasValue)
            {
                var timeAgo = GetTimeAgo(info.Slot8Modified.Value);
                _slot8Label.Text = $"  ✓ Slot 8: {timeAgo}";
                _slot8Label.ForeColor = Color.Green;
            }
            else
            {
                _slot8Label.Text = "  ✗ Slot 8: Not found";
                _slot8Label.ForeColor = Color.Gray;
            }

            _downloadButton.Enabled = info.Slot7Exists || info.Slot8Exists || info.Slot1Exists;
        }
        catch (Exception ex)
        {
            _logger("GUI", $"Error updating save status: {ex.Message}", ConsoleColor.Red);
        }
    }

    private string GetTimeAgo(DateTime time)
    {
        var span = DateTime.Now - time;
        if (span.TotalMinutes < 1)
            return "just now";
        if (span.TotalMinutes < 60)
            return $"{(int)span.TotalMinutes} min ago";
        if (span.TotalHours < 24)
            return $"{(int)span.TotalHours} hr ago";
        return $"{(int)span.TotalDays} day(s) ago";
    }

    private void OnUploadClick(object? sender, EventArgs e)
    {
        _logger("GUI", "User clicked Upload Save", ConsoleColor.Cyan);

        using var dialog = new OpenFileDialog
        {
            Title = "Select Wukong Save Backup",
            Filter = "Wukong Save Files (*.wksave)|*.wksave|All Files (*.*)|*.*",
            FilterIndex = 1
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _logger("SAVE", $"Opening file picker...", ConsoleColor.Yellow);
            _logger("SAVE", $"Selected: {dialog.FileName}", ConsoleColor.Cyan);
            
            _footerLabel.Text = "Status: Uploading save...";
            _footerLabel.ForeColor = Color.Orange;
            Application.DoEvents();

            SaveManager.UploadSave(dialog.FileName, _logger);
            
            _footerLabel.Text = "Status: Save uploaded successfully!";
            _footerLabel.ForeColor = Color.Green;
            
            UpdateSaveStatus();
        }
        else
        {
            _logger("SAVE", "Upload cancelled by user", ConsoleColor.Gray);
        }
    }

    private void OnDownloadClick(object? sender, EventArgs e)
    {
        _logger("GUI", "User clicked Download Save", ConsoleColor.Cyan);

        var defaultFileName = $"WukongSave_{DateTime.Now:yyyy-MM-dd_HH-mm}.wksave";
        
        using var dialog = new SaveFileDialog
        {
            Title = "Save Backup File",
            Filter = "Wukong Save Files (*.wksave)|*.wksave|All Files (*.*)|*.*",
            FilterIndex = 1,
            FileName = defaultFileName
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _logger("SAVE", $"Saving to: {dialog.FileName}", ConsoleColor.Cyan);
            
            _footerLabel.Text = "Status: Creating backup...";
            _footerLabel.ForeColor = Color.Orange;
            Application.DoEvents();

            SaveManager.DownloadSave(dialog.FileName, _logger);
            
            _footerLabel.Text = "Status: Backup saved successfully!";
            _footerLabel.ForeColor = Color.Green;
        }
        else
        {
            _logger("SAVE", "Download cancelled by user", ConsoleColor.Gray);
        }
    }

    private void OnLaunchClick(object? sender, EventArgs e)
    {
        _logger("GUI", "User clicked Launch Game", ConsoleColor.Green);
        
        _footerLabel.Text = "Status: Launching game...";
        _footerLabel.ForeColor = Color.Green;
        Application.DoEvents();

        _launchSignal.Set();
        this.Close();
    }
}
