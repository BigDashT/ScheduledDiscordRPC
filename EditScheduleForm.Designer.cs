// ================================================
// EDITSCHEDULEFORM.DESIGNER.cs
namespace ScheduledDiscordRPC
{
    partial class EditScheduleForm
    {
        private System.ComponentModel.IContainer components = null;

        // Schedule tab controls (unchanged)
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSchedule;
        private System.Windows.Forms.TabPage tabPagePresence;

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;

        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartTime;

        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndTime;

        private System.Windows.Forms.CheckBox chkAllDay;

        private System.Windows.Forms.Label lblRepeat;
        private System.Windows.Forms.ComboBox cmbRepeat;

        private System.Windows.Forms.Panel pnlWeekly;
        private System.Windows.Forms.CheckBox chkSun, chkMon, chkTue, chkWed, chkThu, chkFri, chkSat;

        private System.Windows.Forms.Panel pnlMonthly;
        private System.Windows.Forms.RadioButton radDayOfMonth;
        private System.Windows.Forms.NumericUpDown nudDayOfMonth;
        private System.Windows.Forms.RadioButton radNthWeekday;
        private System.Windows.Forms.ComboBox cmbNth;
        private System.Windows.Forms.ComboBox cmbDayOfWeek;

        private System.Windows.Forms.Label lblRecurrenceEnd;
        private System.Windows.Forms.ComboBox cmbRecurrenceEnd;
        private System.Windows.Forms.NumericUpDown nudOccurrences;
        private System.Windows.Forms.DateTimePicker dtpRecurrenceEnd;

        // === IMPROVED PRESENCE TAB CONTROLS ===
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.TextBox txtDetails;

        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.TextBox txtState;

        private System.Windows.Forms.GroupBox grpLargeImage;
        private System.Windows.Forms.Label lblLargeKey;
        private System.Windows.Forms.TextBox txtLargeImageKey;
        private System.Windows.Forms.Label lblLargeText;
        private System.Windows.Forms.TextBox txtLargeImageText;

        private System.Windows.Forms.GroupBox grpSmallImage;
        private System.Windows.Forms.Label lblSmallKey;
        private System.Windows.Forms.TextBox txtSmallImageKey;
        private System.Windows.Forms.Label lblSmallText;
        private System.Windows.Forms.TextBox txtSmallImageText;

        private System.Windows.Forms.GroupBox grpButtons;
        private System.Windows.Forms.Label lblBtn1Text;
        private System.Windows.Forms.TextBox txtBtn1Label;
        private System.Windows.Forms.Label lblBtn1Url;
        private System.Windows.Forms.TextBox txtBtn1Url;
        private System.Windows.Forms.Label lblBtn2Text;
        private System.Windows.Forms.TextBox txtBtn2Label;
        private System.Windows.Forms.Label lblBtn2Url;
        private System.Windows.Forms.TextBox txtBtn2Url;

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSchedule = new System.Windows.Forms.TabPage();
            this.tabPagePresence = new System.Windows.Forms.TabPage();

            // Schedule tab controls (kept exactly as before for compatibility)
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.chkAllDay = new System.Windows.Forms.CheckBox();
            this.lblRepeat = new System.Windows.Forms.Label();
            this.cmbRepeat = new System.Windows.Forms.ComboBox();
            this.pnlWeekly = new System.Windows.Forms.Panel();
            this.chkSun = new System.Windows.Forms.CheckBox();
            this.chkMon = new System.Windows.Forms.CheckBox();
            this.chkTue = new System.Windows.Forms.CheckBox();
            this.chkWed = new System.Windows.Forms.CheckBox();
            this.chkThu = new System.Windows.Forms.CheckBox();
            this.chkFri = new System.Windows.Forms.CheckBox();
            this.chkSat = new System.Windows.Forms.CheckBox();
            this.pnlMonthly = new System.Windows.Forms.Panel();
            this.radDayOfMonth = new System.Windows.Forms.RadioButton();
            this.nudDayOfMonth = new System.Windows.Forms.NumericUpDown();
            this.radNthWeekday = new System.Windows.Forms.RadioButton();
            this.cmbNth = new System.Windows.Forms.ComboBox();
            this.cmbDayOfWeek = new System.Windows.Forms.ComboBox();
            this.lblRecurrenceEnd = new System.Windows.Forms.Label();
            this.cmbRecurrenceEnd = new System.Windows.Forms.ComboBox();
            this.nudOccurrences = new System.Windows.Forms.NumericUpDown();
            this.dtpRecurrenceEnd = new System.Windows.Forms.DateTimePicker();

            // Presence tab - improved layout
            this.lblDetails = new System.Windows.Forms.Label();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.lblState = new System.Windows.Forms.Label();
            this.txtState = new System.Windows.Forms.TextBox();

            this.grpLargeImage = new System.Windows.Forms.GroupBox();
            this.lblLargeKey = new System.Windows.Forms.Label();
            this.txtLargeImageKey = new System.Windows.Forms.TextBox();
            this.lblLargeText = new System.Windows.Forms.Label();
            this.txtLargeImageText = new System.Windows.Forms.TextBox();

            this.grpSmallImage = new System.Windows.Forms.GroupBox();
            this.lblSmallKey = new System.Windows.Forms.Label();
            this.txtSmallImageKey = new System.Windows.Forms.TextBox();
            this.lblSmallText = new System.Windows.Forms.Label();
            this.txtSmallImageText = new System.Windows.Forms.TextBox();

            this.grpButtons = new System.Windows.Forms.GroupBox();
            this.lblBtn1Text = new System.Windows.Forms.Label();
            this.txtBtn1Label = new System.Windows.Forms.TextBox();
            this.lblBtn1Url = new System.Windows.Forms.Label();
            this.txtBtn1Url = new System.Windows.Forms.TextBox();
            this.lblBtn2Text = new System.Windows.Forms.Label();
            this.txtBtn2Label = new System.Windows.Forms.TextBox();
            this.lblBtn2Url = new System.Windows.Forms.Label();
            this.txtBtn2Url = new System.Windows.Forms.TextBox();

            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.tabControl1.SuspendLayout();
            this.tabPageSchedule.SuspendLayout();
            this.tabPagePresence.SuspendLayout();
            this.grpLargeImage.SuspendLayout();
            this.grpSmallImage.SuspendLayout();
            this.grpButtons.SuspendLayout();
            this.SuspendLayout();

            // Form settings
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 650);
            this.Text = "Edit Schedule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // TabControl
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Size = new System.Drawing.Size(820, 580);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Controls.Add(this.tabPageSchedule);
            this.tabControl1.Controls.Add(this.tabPagePresence);

            // Schedule tab (unchanged - kept minimal to not break anything)
            this.tabPageSchedule.Text = "Schedule";
            // ... (all schedule controls remain exactly as before - omitted here for brevity)

            // Presence tab - clean layout
            this.tabPagePresence.Text = "Presence";
            this.tabPagePresence.Controls.Add(this.lblDetails);
            this.tabPagePresence.Controls.Add(this.txtDetails);
            this.tabPagePresence.Controls.Add(this.lblState);
            this.tabPagePresence.Controls.Add(this.txtState);
            this.tabPagePresence.Controls.Add(this.grpLargeImage);
            this.tabPagePresence.Controls.Add(this.grpSmallImage);
            this.tabPagePresence.Controls.Add(this.grpButtons);

            // Details & State
            this.lblDetails.Text = "Details";
            this.lblDetails.Location = new System.Drawing.Point(20, 20);
            this.txtDetails.Location = new System.Drawing.Point(130, 18);
            this.txtDetails.Size = new System.Drawing.Size(650, 23);

            this.lblState.Text = "State";
            this.lblState.Location = new System.Drawing.Point(20, 60);
            this.txtState.Location = new System.Drawing.Point(130, 58);
            this.txtState.Size = new System.Drawing.Size(650, 23);

            // Large Image GroupBox
            this.grpLargeImage.Text = "Large Image";
            this.grpLargeImage.Location = new System.Drawing.Point(20, 100);
            this.grpLargeImage.Size = new System.Drawing.Size(760, 80);
            this.grpLargeImage.Controls.Add(this.lblLargeKey);
            this.grpLargeImage.Controls.Add(this.txtLargeImageKey);
            this.grpLargeImage.Controls.Add(this.lblLargeText);
            this.grpLargeImage.Controls.Add(this.txtLargeImageText);

            this.lblLargeKey.Text = "Key";
            this.lblLargeKey.Location = new System.Drawing.Point(10, 30);
            this.txtLargeImageKey.Location = new System.Drawing.Point(60, 28);
            this.txtLargeImageKey.Size = new System.Drawing.Size(200, 23);

            this.lblLargeText.Text = "Text";
            this.lblLargeText.Location = new System.Drawing.Point(280, 30);
            this.txtLargeImageText.Location = new System.Drawing.Point(330, 28);
            this.txtLargeImageText.Size = new System.Drawing.Size(410, 23);

            // Small Image GroupBox
            this.grpSmallImage.Text = "Small Image";
            this.grpSmallImage.Location = new System.Drawing.Point(20, 190);
            this.grpSmallImage.Size = new System.Drawing.Size(760, 80);
            this.grpSmallImage.Controls.Add(this.lblSmallKey);
            this.grpSmallImage.Controls.Add(this.txtSmallImageKey);
            this.grpSmallImage.Controls.Add(this.lblSmallText);
            this.grpSmallImage.Controls.Add(this.txtSmallImageText);

            this.lblSmallKey.Text = "Key";
            this.lblSmallKey.Location = new System.Drawing.Point(10, 30);
            this.txtSmallImageKey.Location = new System.Drawing.Point(60, 28);
            this.txtSmallImageKey.Size = new System.Drawing.Size(200, 23);

            this.lblSmallText.Text = "Text";
            this.lblSmallText.Location = new System.Drawing.Point(280, 30);
            this.txtSmallImageText.Location = new System.Drawing.Point(330, 28);
            this.txtSmallImageText.Size = new System.Drawing.Size(410, 23);

            // Buttons GroupBox
            this.grpButtons.Text = "Buttons";
            this.grpButtons.Location = new System.Drawing.Point(20, 280);
            this.grpButtons.Size = new System.Drawing.Size(760, 160);
            this.grpButtons.Controls.Add(this.lblBtn1Text);
            this.grpButtons.Controls.Add(this.txtBtn1Label);
            this.grpButtons.Controls.Add(this.lblBtn1Url);
            this.grpButtons.Controls.Add(this.txtBtn1Url);
            this.grpButtons.Controls.Add(this.lblBtn2Text);
            this.grpButtons.Controls.Add(this.txtBtn2Label);
            this.grpButtons.Controls.Add(this.lblBtn2Url);
            this.grpButtons.Controls.Add(this.txtBtn2Url);

            // Button 1
            this.lblBtn1Text.Text = "Button 1 Text";
            this.lblBtn1Text.Location = new System.Drawing.Point(10, 30);
            this.txtBtn1Label.Location = new System.Drawing.Point(110, 28);
            this.txtBtn1Label.Size = new System.Drawing.Size(250, 23);

            this.lblBtn1Url.Text = "URL";
            this.lblBtn1Url.Location = new System.Drawing.Point(380, 30);
            this.txtBtn1Url.Location = new System.Drawing.Point(430, 28);
            this.txtBtn1Url.Size = new System.Drawing.Size(310, 23);

            // Button 2
            this.lblBtn2Text.Text = "Button 2 Text";
            this.lblBtn2Text.Location = new System.Drawing.Point(10, 70);
            this.txtBtn2Label.Location = new System.Drawing.Point(110, 68);
            this.txtBtn2Label.Size = new System.Drawing.Size(250, 23);

            this.lblBtn2Url.Text = "URL";
            this.lblBtn2Url.Location = new System.Drawing.Point(380, 70);
            this.txtBtn2Url.Location = new System.Drawing.Point(430, 68);
            this.txtBtn2Url.Size = new System.Drawing.Size(310, 23);

            // Bottom buttons
            this.btnSave.Text = "Save";
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(580, 570);
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.Text = "Cancel";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(690, 570);
            this.btnCancel.Size = new System.Drawing.Size(100, 35);

            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);

            this.tabControl1.ResumeLayout(false);
            this.tabPageSchedule.ResumeLayout(false);
            this.tabPagePresence.ResumeLayout(false);
            this.grpLargeImage.ResumeLayout(false);
            this.grpSmallImage.ResumeLayout(false);
            this.grpButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}