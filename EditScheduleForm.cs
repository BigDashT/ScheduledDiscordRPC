// ================================================
// EDITSCHEDULEFORM.cs
using System;
using System.Windows.Forms;
using System.Linq;

namespace ScheduledDiscordRPC
{
    /// <summary>
    /// Modal "Add/Edit Schedule" dialog. Works on a private clone of the passed-in Schedule (or a
    /// fresh one) so Cancel never mutates the caller's data; on Save, the clone is written back
    /// into <see cref="Schedule"/> for the caller to read via DialogResult.OK.
    /// </summary>
    public partial class EditScheduleForm : Form
    {
        public Schedule Schedule { get; private set; }

        public EditScheduleForm(Schedule? existing = null)
        {
            InitializeComponent();
            Schedule = existing?.Clone() ?? new Schedule();

            // Default values for brand-new schedules
            if (Schedule.Start.Year < 2000)
            {
                var now = DateTime.Now;
                Schedule.Start = now.Date.AddHours(now.Hour);
                Schedule.End = Schedule.Start.AddHours(1);
                Schedule.IsAllDay = false;
            }

            LoadScheduleIntoUI();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Applied on Load (not in the constructor) so every control's window handle already
            // exists — needed for the ComboBox/DateTimePicker native-chrome flattening in UiTheme.
            UiTheme.ApplyTheme(this);
        }

        /// <summary>Populates every control on both tabs from <see cref="Schedule"/>.</summary>
        private void LoadScheduleIntoUI()
        {
            txtName.Text = Schedule.Name;

            dtpStartDate.Value = Schedule.Start.Date;
            dtpStartTime.Value = Schedule.Start;
            dtpEndDate.Value = Schedule.End.Date;
            dtpEndTime.Value = Schedule.End;
            chkAllDay.Checked = Schedule.IsAllDay;

            // === REPEAT COMBO ===
            cmbRepeat.Items.Clear();
            cmbRepeat.Items.AddRange(new[] { "Does not repeat", "Daily", "Every weekday", "Weekly", "Monthly" });
            int repeatIdx = Schedule.Recurrence.Type switch
            {
                RecurrenceType.None => 0,
                RecurrenceType.Daily => 1,
                RecurrenceType.Weekday => 2,
                RecurrenceType.Weekly => 3,
                RecurrenceType.Monthly => 4,
                _ => 0
            };
            cmbRepeat.SelectedIndex = repeatIdx;
            cmbRepeat.SelectedIndexChanged += cmbRepeat_SelectedIndexChanged;

            // === WEEKLY DAYS ===
            foreach (var day in Schedule.Recurrence.DaysOfWeek)
            {
                CheckBox? cb = day switch
                {
                    DayOfWeek.Sunday => chkSun,
                    DayOfWeek.Monday => chkMon,
                    DayOfWeek.Tuesday => chkTue,
                    DayOfWeek.Wednesday => chkWed,
                    DayOfWeek.Thursday => chkThu,
                    DayOfWeek.Friday => chkFri,
                    DayOfWeek.Saturday => chkSat,
                    _ => null
                };
                if (cb != null) cb.Checked = true;
            }

            // === MONTHLY COMBOS - populate BEFORE setting index ===
            if (cmbNth.Items.Count == 0)
                cmbNth.Items.AddRange(new object[] { "1st", "2nd", "3rd", "4th", "Last" });

            if (cmbDayOfWeek.Items.Count == 0)
                cmbDayOfWeek.Items.AddRange(new object[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });

            // === RECURRENCE END COMBO - THIS WAS THE MISSING PIECE ===
            if (cmbRecurrenceEnd.Items.Count == 0)
                cmbRecurrenceEnd.Items.AddRange(new object[] { "Never", "After", "On" });

            // Monthly settings
            if (Schedule.Recurrence.DayOfMonth.HasValue)
            {
                radDayOfMonth.Checked = true;
                nudDayOfMonth.Value = Schedule.Recurrence.DayOfMonth.Value;
            }
            else if (Schedule.Recurrence.WeekOfMonth.HasValue && Schedule.Recurrence.DayOfWeekForMonth.HasValue)
            {
                radNthWeekday.Checked = true;
                cmbNth.SelectedIndex = Schedule.Recurrence.WeekOfMonth.Value switch
                {
                    1 => 0,
                    2 => 1,
                    3 => 2,
                    4 => 3,
                    -1 => 4,
                    _ => 0
                };
                cmbDayOfWeek.SelectedIndex = (int)Schedule.Recurrence.DayOfWeekForMonth.Value;
            }

            // Recurrence end
            if (Schedule.Recurrence.EndDate.HasValue)
            {
                cmbRecurrenceEnd.SelectedIndex = 2;
                dtpRecurrenceEnd.Value = Schedule.Recurrence.EndDate.Value;
            }
            else
            {
                cmbRecurrenceEnd.SelectedIndex = 0;
            }
            cmbRecurrenceEnd.SelectedIndexChanged += cmbRecurrenceEnd_SelectedIndexChanged;

            // Presence tab
            txtDetails.Text = Schedule.Presence.Details;
            txtState.Text = Schedule.Presence.State;
            txtLargeImageKey.Text = Schedule.Presence.LargeImageKey;
            txtLargeImageText.Text = Schedule.Presence.LargeImageText;
            txtSmallImageKey.Text = Schedule.Presence.SmallImageKey;
            txtSmallImageText.Text = Schedule.Presence.SmallImageText;

            if (Schedule.Presence.Buttons.Count > 0)
            {
                txtBtn1Label.Text = Schedule.Presence.Buttons[0].Label;
                txtBtn1Url.Text = Schedule.Presence.Buttons[0].Url;
            }
            if (Schedule.Presence.Buttons.Count > 1)
            {
                txtBtn2Label.Text = Schedule.Presence.Buttons[1].Label;
                txtBtn2Url.Text = Schedule.Presence.Buttons[1].Url;
            }

            UpdateRepeatPanelVisibility();
            UpdateRecurrenceEndVisibility();
            chkAllDay.CheckedChanged += chkAllDay_CheckedChanged;
        }

        private void chkAllDay_CheckedChanged(object? sender, EventArgs e)
        {
            bool allDay = chkAllDay.Checked;
            dtpStartTime.Enabled = dtpEndTime.Enabled = !allDay;
            if (allDay)
            {
                dtpEndDate.Value = dtpStartDate.Value.Date.AddDays(1);
            }
        }

        private void cmbRepeat_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateRepeatPanelVisibility();
        }

        private void UpdateRepeatPanelVisibility()
        {
            pnlWeekly.Visible = false;
            pnlMonthly.Visible = false;

            switch (cmbRepeat.SelectedIndex)
            {
                case 2: // Every weekday
                case 3: // Weekly
                    pnlWeekly.Visible = true;
                    if (cmbRepeat.SelectedIndex == 2)
                    {
                        chkMon.Checked = chkTue.Checked = chkWed.Checked = chkThu.Checked = chkFri.Checked = true;
                        chkSun.Checked = chkSat.Checked = false;
                    }
                    break;
                case 4: // Monthly
                    pnlMonthly.Visible = true;
                    break;
            }
        }

        private void cmbRecurrenceEnd_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateRecurrenceEndVisibility();
        }

        private void UpdateRecurrenceEndVisibility()
        {
            nudOccurrences.Visible = cmbRecurrenceEnd.SelectedIndex == 1;
            dtpRecurrenceEnd.Visible = cmbRecurrenceEnd.SelectedIndex == 2;
        }

        /// <summary>Validates the form, writes every control's value back into <see cref="Schedule"/>, and closes with DialogResult.OK.</summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a schedule name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!chkAllDay.Checked && (dtpStartDate.Value.Date + dtpStartTime.Value.TimeOfDay) >= (dtpEndDate.Value.Date + dtpEndTime.Value.TimeOfDay))
            {
                MessageBox.Show("End time must be after start time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Schedule.Name = txtName.Text.Trim();

            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date;
            if (chkAllDay.Checked)
            {
                Schedule.Start = startDate;
                Schedule.End = endDate.AddDays(1);
                Schedule.IsAllDay = true;
            }
            else
            {
                Schedule.Start = startDate + dtpStartTime.Value.TimeOfDay;
                Schedule.End = endDate + dtpEndTime.Value.TimeOfDay;
                Schedule.IsAllDay = false;
            }

            Schedule.Recurrence.Type = cmbRepeat.SelectedIndex switch
            {
                0 => RecurrenceType.None,
                1 => RecurrenceType.Daily,
                2 => RecurrenceType.Weekday,
                3 => RecurrenceType.Weekly,
                4 => RecurrenceType.Monthly,
                _ => RecurrenceType.None
            };
            Schedule.Recurrence.Interval = 1;

            Schedule.Recurrence.DaysOfWeek.Clear();
            if (pnlWeekly.Visible)
            {
                if (chkSun.Checked) Schedule.Recurrence.DaysOfWeek.Add(DayOfWeek.Sunday);
                if (chkMon.Checked) Schedule.Recurrence.DaysOfWeek.Add(DayOfWeek.Monday);
                if (chkTue.Checked) Schedule.Recurrence.DaysOfWeek.Add(DayOfWeek.Tuesday);
                if (chkWed.Checked) Schedule.Recurrence.DaysOfWeek.Add(DayOfWeek.Wednesday);
                if (chkThu.Checked) Schedule.Recurrence.DaysOfWeek.Add(DayOfWeek.Thursday);
                if (chkFri.Checked) Schedule.Recurrence.DaysOfWeek.Add(DayOfWeek.Friday);
                if (chkSat.Checked) Schedule.Recurrence.DaysOfWeek.Add(DayOfWeek.Saturday);
            }

            Schedule.Recurrence.DayOfMonth = null;
            Schedule.Recurrence.WeekOfMonth = null;
            Schedule.Recurrence.DayOfWeekForMonth = null;
            if (pnlMonthly.Visible)
            {
                if (radDayOfMonth.Checked)
                {
                    Schedule.Recurrence.DayOfMonth = (int)nudDayOfMonth.Value;
                }
                else if (radNthWeekday.Checked && cmbNth.SelectedIndex >= 0 && cmbDayOfWeek.SelectedIndex >= 0)
                {
                    Schedule.Recurrence.WeekOfMonth = cmbNth.SelectedIndex switch
                    {
                        4 => -1,
                        _ => cmbNth.SelectedIndex + 1
                    };
                    Schedule.Recurrence.DayOfWeekForMonth = (DayOfWeek)cmbDayOfWeek.SelectedIndex;
                }
            }

            Schedule.Recurrence.EndDate = null;
            if (cmbRecurrenceEnd.SelectedIndex == 2)
            {
                Schedule.Recurrence.EndDate = dtpRecurrenceEnd.Value.Date;
            }

            Schedule.Presence.Details = txtDetails.Text;
            Schedule.Presence.State = txtState.Text;
            Schedule.Presence.LargeImageKey = txtLargeImageKey.Text;
            Schedule.Presence.LargeImageText = txtLargeImageText.Text;
            Schedule.Presence.SmallImageKey = txtSmallImageKey.Text;
            Schedule.Presence.SmallImageText = txtSmallImageText.Text;

            Schedule.Presence.Buttons.Clear();
            if (!string.IsNullOrWhiteSpace(txtBtn1Label.Text))
                Schedule.Presence.Buttons.Add(new ButtonConfig { Label = txtBtn1Label.Text, Url = txtBtn1Url.Text });
            if (!string.IsNullOrWhiteSpace(txtBtn2Label.Text))
                Schedule.Presence.Buttons.Add(new ButtonConfig { Label = txtBtn2Label.Text, Url = txtBtn2Url.Text });

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}