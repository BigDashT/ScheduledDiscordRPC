// ================================================
// MAINFORM.cs
using DiscordRPC;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;   // Added for Icon support

namespace ScheduledDiscordRPC
{
    public partial class MainForm : Form
    {
        private DiscordRpcClient? _client;
        private AppConfig _config = new();
        private RichPresence? _currentPresence;
        private System.Windows.Forms.Timer _timer = new() { Interval = 30000 };

        private NotifyIcon _trayIcon = new();

        public MainForm()
        {
            InitializeComponent();

            // === CUSTOM ICON SETUP ===
            try
            {
                var appIcon = new Icon("appicon.ico");
                this.Icon = appIcon;                    // Main application window
                _trayIcon.Icon = appIcon;               // System tray icon
            }
            catch (Exception ex)
            {
                // Fallback if icon file is missing
                System.Diagnostics.Debug.WriteLine($"Warning: Could not load appicon.ico → {ex.Message}");
                _trayIcon.Icon = SystemIcons.Application; // Default fallback
            }

            LoadConfig();
            SetupTrayIcon();
            SetupClient();
            _timer.Tick += Timer_Tick;
            _timer.Start();
            UpdateScheduleGrid();
            ApplyCurrentSchedule();
        }

        private void LoadConfig()
        {
            _config = ConfigManager.Load();
            txtClientId.Text = _config.ClientId;
            chkStartup.Checked = _config.RunOnStartup;
            ToggleAutoStart(_config.RunOnStartup);
        }

        private void SaveConfig()
        {
            _config.ClientId = txtClientId.Text.Trim();
            _config.RunOnStartup = chkStartup.Checked;
            ConfigManager.Save(_config);
        }

        private void SetupClient()
        {
            if (!string.IsNullOrWhiteSpace(_config.ClientId))
            {
                _client?.Dispose();
                _client = new DiscordRpcClient(_config.ClientId);
                _client.OnReady += (sender, e) => this.Invoke(() => lblStatus.Text = $"✅ Connected ({e.User.Username})");
                _client.Initialize();
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ApplyCurrentSchedule();
        }

        private void SafeUpdatePresence(RichPresence? newRp)
        {
            try
            {
                if (newRp == null)
                {
                    _client?.ClearPresence();
                    _currentPresence = null;
                    lblStatus.Text = "🧹 Presence cleared";
                }
                else
                {
                    _client?.SetPresence(newRp);
                    _currentPresence = newRp;
                    lblStatus.Text = $"🎯 Active: {(_config.Schedules.FirstOrDefault(s => IsScheduleActive(s, DateTime.Now))?.Name ?? "Default")}";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "⚠️ Discord RPC error (is Discord running?)";
                System.Diagnostics.Debug.WriteLine($"RPC Error: {ex.Message}");
            }
        }

        private void ApplyCurrentSchedule()
        {
            if (_client == null || !_client.IsInitialized) return;

            var now = DateTime.Now;
            Schedule? active = _config.Schedules.FirstOrDefault(s => IsScheduleActive(s, now));
            var target = active?.Presence ?? _config.DefaultPresence;

            if (target == null)
            {
                if (_currentPresence != null) SafeUpdatePresence(null);
            }
            else
            {
                var newRp = new RichPresence
                {
                    Details = target.Details,
                    State = target.State,
                    Assets = new Assets
                    {
                        LargeImageKey = target.LargeImageKey,
                        LargeImageText = target.LargeImageText,
                        SmallImageKey = target.SmallImageKey,
                        SmallImageText = target.SmallImageText
                    },
                    Buttons = target.Buttons.Select(b => new DiscordRPC.Button { Label = b.Label, Url = b.Url }).ToArray()
                };

                if (_currentPresence == null || !PresenceEquals(_currentPresence, newRp))
                    SafeUpdatePresence(newRp);
            }
        }

        private bool IsScheduleActive(Schedule s, DateTime now)
        {
            if (s.Recurrence.EndDate.HasValue && now.Date > s.Recurrence.EndDate.Value.Date)
                return false;

            bool dateMatches = false;

            switch (s.Recurrence.Type)
            {
                case RecurrenceType.None:
                    dateMatches = now >= s.Start && now < s.End;
                    break;

                case RecurrenceType.Daily:
                case RecurrenceType.Weekday:
                case RecurrenceType.Weekly:
                    int daysSinceStart = (now.Date - s.Start.Date).Days;
                    if (daysSinceStart < 0) return false;
                    if (s.Recurrence.Type == RecurrenceType.Weekday)
                    {
                        if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday) return false;
                    }
                    else if (s.Recurrence.Type == RecurrenceType.Weekly)
                    {
                        if (!s.Recurrence.DaysOfWeek.Contains(now.DayOfWeek)) return false;
                        int weeksSinceStart = daysSinceStart / 7;
                        if (weeksSinceStart % s.Recurrence.Interval != 0) return false;
                    }
                    dateMatches = daysSinceStart % s.Recurrence.Interval == 0;
                    break;

                case RecurrenceType.Monthly:
                    if (s.Recurrence.DayOfMonth.HasValue)
                    {
                        dateMatches = now.Day == s.Recurrence.DayOfMonth.Value;
                    }
                    else if (s.Recurrence.WeekOfMonth.HasValue && s.Recurrence.DayOfWeekForMonth.HasValue)
                    {
                        var nthDate = GetNthWeekdayOfMonth(now.Year, now.Month,
                            s.Recurrence.DayOfWeekForMonth.Value, s.Recurrence.WeekOfMonth.Value);
                        dateMatches = now.Date == nthDate.Date;
                    }
                    break;
            }

            if (!dateMatches) return false;

            if (s.IsAllDay) return true;

            var startTime = s.Start.TimeOfDay;
            var endTime = s.End.TimeOfDay;
            var nowTime = now.TimeOfDay;

            if (endTime > startTime)
                return nowTime >= startTime && nowTime < endTime;
            else
                return nowTime >= startTime || nowTime < endTime;
        }

        private DateTime GetNthWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek, int nth)
        {
            if (nth < 0) // Last
            {
                var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                int diff = (int)(lastDay.DayOfWeek - dayOfWeek + 7) % 7;
                return lastDay.AddDays(-diff);
            }

            var first = new DateTime(year, month, 1);
            int offset = (int)(dayOfWeek - first.DayOfWeek + 7) % 7;
            var firstOccurrence = first.AddDays(offset);
            return firstOccurrence.AddDays(7 * (nth - 1));
        }

        private bool PresenceEquals(RichPresence a, RichPresence b)
        {
            return a.Details == b.Details && a.State == b.State &&
                   a.Assets?.LargeImageKey == b.Assets?.LargeImageKey &&
                   a.Assets?.LargeImageText == b.Assets?.LargeImageText;
        }

        private void SetupTrayIcon()
        {
            _trayIcon.Visible = true;
            _trayIcon.Text = "Scheduled Discord RPC";
            _trayIcon.DoubleClick += (s, e) => ShowMainWindow();

            var menu = new ContextMenuStrip();
            menu.Items.Add("Show", null, (s, e) => ShowMainWindow());
            menu.Items.Add("Exit", null, (s, e) => Application.Exit());
            _trayIcon.ContextMenuStrip = menu;
        }

        private void ShowMainWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void ToggleAutoStart(bool enable)
        {
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (key == null) return;
            if (enable)
                key.SetValue("ScheduledDiscordRPC", Application.ExecutablePath);
            else
                key.DeleteValue("ScheduledDiscordRPC", false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using var form = new EditScheduleForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _config.Schedules.Add(form.Schedule);
                SaveConfig();
                UpdateScheduleGrid();
                ApplyCurrentSchedule();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSchedules.SelectedRows.Count == 0) return;
            int index = dgvSchedules.SelectedRows[0].Index;
            var schedule = _config.Schedules[index];

            using var form = new EditScheduleForm(schedule);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _config.Schedules[index] = form.Schedule;
                SaveConfig();
                UpdateScheduleGrid();
                ApplyCurrentSchedule();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSchedules.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Delete this schedule permanently?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int index = dgvSchedules.SelectedRows[0].Index;
                _config.Schedules.RemoveAt(index);
                SaveConfig();
                UpdateScheduleGrid();
                ApplyCurrentSchedule();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ApplyCurrentSchedule();
        }

        private void chkStartup_CheckedChanged(object sender, EventArgs e)
        {
            SaveConfig();
            ToggleAutoStart(chkStartup.Checked);
        }

        private void txtClientId_Leave(object sender, EventArgs e)
        {
            SaveConfig();
            SetupClient();
        }

        private void UpdateScheduleGrid()
        {
            dgvSchedules.DataSource = null;
            dgvSchedules.DataSource = _config.Schedules.Select(s => new
            {
                s.Name,
                Recurrence = s.RecurrenceSummary,
                Time = s.IsAllDay ? "All day" : $"{s.Start:HH:mm} – {s.End:HH:mm}",
                Details = s.Presence.Details
            }).ToList();

            if (dgvSchedules.Columns.Count > 0)
            {
                dgvSchedules.Columns[0].HeaderText = "Schedule Name";
                dgvSchedules.Columns[1].HeaderText = "Repeats";
                dgvSchedules.Columns[2].HeaderText = "Time";
                dgvSchedules.Columns[3].HeaderText = "Presence";
                dgvSchedules.AutoResizeColumns();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                _trayIcon.ShowBalloonTip(3000, "Scheduled Discord RPC", "Still running in the system tray", ToolTipIcon.Info);
            }
            base.OnFormClosing(e);
        }

        private void lblClientId_Click(object sender, EventArgs e)
        {

        }
    }
}