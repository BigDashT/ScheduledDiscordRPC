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
    /// <summary>
    /// Main window and application "controller": owns the Discord RPC client, the system tray
    /// icon, and the polling timer that evaluates schedules and pushes presence updates.
    /// The window itself is just an editor for the schedule list — closing it hides it to the
    /// tray rather than exiting (see OnFormClosing); the app keeps running until Exit is chosen
    /// from the tray menu.
    /// </summary>
    public partial class MainForm : Form
    {
        private DiscordRpcClient? _client;
        private AppConfig _config = new();
        private RichPresence? _currentPresence;
        private readonly System.Windows.Forms.Timer _timer = new() { Interval = 30000 };

        private readonly NotifyIcon _trayIcon = new();
        private readonly ContextMenuStrip _trayMenu = new();

        /// <summary>The Client ID the current _client was created with, so we can skip needless
        /// reconnects when the textbox loses focus without the value actually changing.</summary>
        private string? _connectedClientId;

        public MainForm()
        {
            InitializeComponent();

            // === CUSTOM ICON SETUP ===
            // Pull the icon that's already embedded in this executable (via <ApplicationIcon> in
            // the .csproj) rather than loading a loose .ico file by relative path. A relative path
            // depends on the current working directory matching wherever appicon.ico happens to
            // be, which is not guaranteed for a published/installed copy of the app — using the
            // embedded resource works reliably regardless of how or where the exe is run from.
            var appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? SystemIcons.Application;
            this.Icon = appIcon;
            _trayIcon.Icon = appIcon;

            LoadConfig();
            SetupTrayIcon();
            SetupClient();
            _timer.Tick += Timer_Tick;
            _timer.Start();
            UpdateScheduleGrid();
            ApplyCurrentSchedule();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Applied on Load (not in the constructor) so every control's window handle already
            // exists — needed for the ComboBox/DateTimePicker native-chrome flattening in UiTheme.
            UiTheme.ApplyTheme(this);
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

        /// <summary>
        /// (Re)connects the Discord RPC client using the currently configured Client ID. Safe to
        /// call repeatedly — it's a no-op if the ID hasn't actually changed since the last
        /// successful connection, to avoid needlessly dropping and re-establishing the Discord
        /// connection (e.g. every time the Client ID textbox merely loses focus).
        /// </summary>
        private void SetupClient()
        {
            var clientId = _config.ClientId;
            if (string.IsNullOrWhiteSpace(clientId))
            {
                _client?.Dispose();
                _client = null;
                _connectedClientId = null;
                return;
            }

            if (_client != null && clientId == _connectedClientId)
                return; // Already connected (or connecting) with this exact ID — nothing to do.

            _client?.Dispose();
            _connectedClientId = clientId;
            _client = new DiscordRpcClient(clientId);
            _client.OnReady += (sender, e) =>
            {
                this.Invoke(() =>
                {
                    lblStatus.Text = $"✅ Connected ({e.User.Username})";
                    // The 30s timer may not have ticked yet since Discord finished connecting —
                    // apply immediately so the presence doesn't lag behind on startup / ID change.
                    ApplyCurrentSchedule();
                });
            };
            _client.OnError += (sender, e) =>
            {
                this.Invoke(() => lblStatus.Text = $"⚠️ Discord error: {e.Message}");
            };
            _client.Initialize();
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

        /// <summary>
        /// Evaluates all schedules against the current time and pushes the corresponding presence
        /// to Discord if it differs from what's already showing. Called on every timer tick, plus
        /// immediately after startup, connecting, and any schedule/client-ID edit so changes are
        /// reflected without waiting for the next 30s poll.
        /// </summary>
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

        /// <summary>
        /// Whether <paramref name="s"/> should be considered active at <paramref name="now"/>.
        /// Non-recurring schedules are a simple absolute-time range check. Recurring schedules are
        /// split into "does today match the recurrence pattern" (<see cref="IsDateActive"/>) and
        /// "are we currently within the time-of-day window", with special handling for schedules
        /// that wrap past midnight (e.g. 22:00-06:00): if "now" falls in the early-morning tail of
        /// such a window, the day whose recurrence pattern actually matters is *yesterday* — the
        /// day the occurrence started — not today.
        /// </summary>
        private bool IsScheduleActive(Schedule s, DateTime now)
        {
            if (s.Recurrence.Type == RecurrenceType.None)
            {
                // One-off schedule: Start/End are absolute timestamps, so a direct range check
                // already handles all-day and overnight-spanning cases correctly with no extra
                // date/time-of-day splitting needed.
                return now >= s.Start && now < s.End;
            }

            if (s.IsAllDay)
            {
                if (s.Recurrence.EndDate.HasValue && now.Date > s.Recurrence.EndDate.Value.Date)
                    return false;
                return IsDateActive(s, now.Date);
            }

            var startTime = s.Start.TimeOfDay;
            var endTime = s.End.TimeOfDay;
            bool overnightWrap = endTime <= startTime;
            bool inWrapTail = overnightWrap && now.TimeOfDay < endTime;
            DateTime referenceDate = inWrapTail ? now.Date.AddDays(-1) : now.Date;

            if (s.Recurrence.EndDate.HasValue && referenceDate > s.Recurrence.EndDate.Value.Date)
                return false;

            if (!IsDateActive(s, referenceDate))
                return false;

            var nowTime = now.TimeOfDay;
            return overnightWrap
                ? (nowTime >= startTime || nowTime < endTime)
                : (nowTime >= startTime && nowTime < endTime);
        }

        /// <summary>
        /// Whether <paramref name="date"/> (a calendar date, no time-of-day) matches the
        /// schedule's recurrence pattern. Only called for recurring schedules — see
        /// <see cref="IsScheduleActive"/> for how the reference date is chosen.
        /// </summary>
        private bool IsDateActive(Schedule s, DateTime date)
        {
            int daysSinceStart = (date - s.Start.Date).Days;
            if (daysSinceStart < 0) return false;

            switch (s.Recurrence.Type)
            {
                case RecurrenceType.Daily:
                    return daysSinceStart % s.Recurrence.Interval == 0;

                case RecurrenceType.Weekday:
                    return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;

                case RecurrenceType.Weekly:
                    if (!s.Recurrence.DaysOfWeek.Contains(date.DayOfWeek)) return false;
                    // Interval applies in units of whole weeks since the start, not raw days —
                    // using days here would wrongly exclude valid days when DaysOfWeek contains
                    // more than one day and Interval > 1 (raw day-count isn't a multiple of 7).
                    int weeksSinceStart = daysSinceStart / 7;
                    return weeksSinceStart % s.Recurrence.Interval == 0;

                case RecurrenceType.Monthly:
                    if (s.Recurrence.DayOfMonth.HasValue)
                        return date.Day == s.Recurrence.DayOfMonth.Value;
                    if (s.Recurrence.WeekOfMonth.HasValue && s.Recurrence.DayOfWeekForMonth.HasValue)
                    {
                        var nthDate = GetNthWeekdayOfMonth(date.Year, date.Month,
                            s.Recurrence.DayOfWeekForMonth.Value, s.Recurrence.WeekOfMonth.Value);
                        return date.Date == nthDate.Date;
                    }
                    return false;

                default:
                    return false;
            }
        }

        /// <summary>Finds the Nth (or, for nth &lt; 0, the last) occurrence of a weekday in a given month.</summary>
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

        /// <summary>
        /// Compares every field that's actually sent to Discord, so ApplyCurrentSchedule can tell
        /// whether a new presence needs to be pushed. Must stay in sync with the fields populated
        /// in ApplyCurrentSchedule — missing one here (as small image / buttons previously were)
        /// means changes to that field silently fail to reach Discord because the app thinks
        /// nothing changed.
        /// </summary>
        private bool PresenceEquals(RichPresence a, RichPresence b)
        {
            if (a.Details != b.Details || a.State != b.State) return false;
            if (a.Assets?.LargeImageKey != b.Assets?.LargeImageKey) return false;
            if (a.Assets?.LargeImageText != b.Assets?.LargeImageText) return false;
            if (a.Assets?.SmallImageKey != b.Assets?.SmallImageKey) return false;
            if (a.Assets?.SmallImageText != b.Assets?.SmallImageText) return false;

            var aButtons = a.Buttons ?? Array.Empty<DiscordRPC.Button>();
            var bButtons = b.Buttons ?? Array.Empty<DiscordRPC.Button>();
            if (aButtons.Length != bButtons.Length) return false;
            for (int i = 0; i < aButtons.Length; i++)
            {
                if (aButtons[i].Label != bButtons[i].Label || aButtons[i].Url != bButtons[i].Url)
                    return false;
            }
            return true;
        }

        private void SetupTrayIcon()
        {
            _trayIcon.Visible = true;
            _trayIcon.Text = "Scheduled Discord RPC";
            _trayIcon.DoubleClick += (s, e) => ShowMainWindow();

            _trayMenu.Items.Add("Show", null, (s, e) => ShowMainWindow());
            _trayMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
            _trayIcon.ContextMenuStrip = _trayMenu;
        }

        private void ShowMainWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        /// <summary>Adds or removes the app from HKCU\...\Run so it can launch at Windows sign-in.</summary>
        private void ToggleAutoStart(bool enable)
        {
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", writable: true);
                if (key == null) return;
                if (enable)
                    key.SetValue("ScheduledDiscordRPC", Application.ExecutablePath);
                else
                    key.DeleteValue("ScheduledDiscordRPC", throwOnMissingValue: false);
            }
            catch (Exception ex)
            {
                // Registry access can fail under restricted environments/permissions — this is a
                // convenience feature, not core functionality, so log and move on rather than
                // crashing the app over it.
                System.Diagnostics.Debug.WriteLine($"Warning: failed to update startup registry entry → {ex.Message}");
            }
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
    }
}
