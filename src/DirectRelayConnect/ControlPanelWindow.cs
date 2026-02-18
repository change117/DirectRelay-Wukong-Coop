using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DirectRelayConnect;

public class ControlPanelWindow : Form
{
    private readonly ManualResetEvent _launchSignal;
    private readonly Action<string, string, ConsoleColor> _logger;
    
    private Button _launchButton = null!;
    private Label _footerLabel = null!;

    public ControlPanelWindow(ManualResetEvent launchSignal, Action<string, string, ConsoleColor> logger)
    {
        _launchSignal = launchSignal;
        _logger = logger;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "DirectRelay Connect Control Panel";
        this.Size = new Size(420, 250);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 240, 240);

        var titleLabel = new Label
        {
            Text = "DirectRelay Control Panel (CLIENT)",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Location = new Point(20, 20),
            Size = new Size(360, 30),
            TextAlign = ContentAlignment.MiddleLeft
        };
        this.Controls.Add(titleLabel);

        var separator1 = new Panel
        {
            Location = new Point(20, 55),
            Size = new Size(360, 2),
            BackColor = Color.FromArgb(200, 200, 200)
        };
        this.Controls.Add(separator1);

        var infoLabel = new Label
        {
            Text = "Saves are synced automatically through the relay.\nClick Launch to install mods, connect to the host, and open the game.",
            Font = new Font("Segoe UI", 9),
            Location = new Point(20, 70),
            Size = new Size(360, 50),
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.FromArgb(80, 80, 80)
        };
        this.Controls.Add(infoLabel);

        _launchButton = new Button
        {
            Text = "Connect & Launch Game",
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            Location = new Point(40, 135),
            Size = new Size(320, 45),
            BackColor = Color.FromArgb(16, 124, 16),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        _launchButton.FlatAppearance.BorderSize = 0;
        _launchButton.Click += OnLaunchClick;
        this.Controls.Add(_launchButton);

        _footerLabel = new Label
        {
            Text = "Status: Ready",
            Font = new Font("Segoe UI", 9),
            Location = new Point(20, 190),
            Size = new Size(360, 20),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.Gray
        };
        this.Controls.Add(_footerLabel);

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
                    e.Cancel = true;
                else
                    Environment.Exit(0);
            }
        };
    }

    private void OnLaunchClick(object? sender, EventArgs e)
    {
        _logger("GUI", "User clicked Connect & Launch Game", ConsoleColor.Green);
        _footerLabel.Text = "Status: Launching game...";
        _footerLabel.ForeColor = Color.Green;
        Application.DoEvents();
        _launchSignal.Set();
        this.Close();
    }
}
