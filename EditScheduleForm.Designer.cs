// ================================================
// EDITSCHEDULEFORM.DESIGNER.cs
namespace ScheduledDiscordRPC
{
    partial class EditScheduleForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSchedule;
        private System.Windows.Forms.TabPage tabPagePresence;

        // Schedule tab controls
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

        // Presence tab controls
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
            tabControl1 = new TabControl();
            tabPageSchedule = new TabPage();
            lblName = new Label();
            txtName = new TextBox();
            lblStart = new Label();
            dtpStartDate = new DateTimePicker();
            dtpStartTime = new DateTimePicker();
            lblTo = new Label();
            dtpEndDate = new DateTimePicker();
            dtpEndTime = new DateTimePicker();
            chkAllDay = new CheckBox();
            lblRepeat = new Label();
            cmbRepeat = new ComboBox();
            pnlWeekly = new Panel();
            chkSun = new CheckBox();
            chkMon = new CheckBox();
            chkTue = new CheckBox();
            chkWed = new CheckBox();
            chkThu = new CheckBox();
            chkFri = new CheckBox();
            chkSat = new CheckBox();
            pnlMonthly = new Panel();
            radDayOfMonth = new RadioButton();
            nudDayOfMonth = new NumericUpDown();
            radNthWeekday = new RadioButton();
            cmbNth = new ComboBox();
            cmbDayOfWeek = new ComboBox();
            lblRecurrenceEnd = new Label();
            cmbRecurrenceEnd = new ComboBox();
            nudOccurrences = new NumericUpDown();
            dtpRecurrenceEnd = new DateTimePicker();
            tabPagePresence = new TabPage();
            lblDetails = new Label();
            txtDetails = new TextBox();
            lblState = new Label();
            txtState = new TextBox();
            grpLargeImage = new GroupBox();
            lblLargeKey = new Label();
            txtLargeImageKey = new TextBox();
            lblLargeText = new Label();
            txtLargeImageText = new TextBox();
            grpSmallImage = new GroupBox();
            lblSmallKey = new Label();
            txtSmallImageKey = new TextBox();
            lblSmallText = new Label();
            txtSmallImageText = new TextBox();
            grpButtons = new GroupBox();
            lblBtn1Text = new Label();
            txtBtn1Label = new TextBox();
            lblBtn1Url = new Label();
            txtBtn1Url = new TextBox();
            lblBtn2Text = new Label();
            txtBtn2Label = new TextBox();
            lblBtn2Url = new Label();
            txtBtn2Url = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            tabControl1.SuspendLayout();
            tabPageSchedule.SuspendLayout();
            pnlWeekly.SuspendLayout();
            pnlMonthly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDayOfMonth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudOccurrences).BeginInit();
            tabPagePresence.SuspendLayout();
            grpLargeImage.SuspendLayout();
            grpSmallImage.SuspendLayout();
            grpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageSchedule);
            tabControl1.Controls.Add(tabPagePresence);
            tabControl1.Dock = DockStyle.Top;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1040, 680);
            tabControl1.TabIndex = 0;
            // 
            // tabPageSchedule
            // 
            tabPageSchedule.Controls.Add(lblName);
            tabPageSchedule.Controls.Add(txtName);
            tabPageSchedule.Controls.Add(lblStart);
            tabPageSchedule.Controls.Add(dtpStartDate);
            tabPageSchedule.Controls.Add(dtpStartTime);
            tabPageSchedule.Controls.Add(lblTo);
            tabPageSchedule.Controls.Add(dtpEndDate);
            tabPageSchedule.Controls.Add(dtpEndTime);
            tabPageSchedule.Controls.Add(chkAllDay);
            tabPageSchedule.Controls.Add(lblRepeat);
            tabPageSchedule.Controls.Add(cmbRepeat);
            tabPageSchedule.Controls.Add(pnlWeekly);
            tabPageSchedule.Controls.Add(pnlMonthly);
            tabPageSchedule.Controls.Add(lblRecurrenceEnd);
            tabPageSchedule.Controls.Add(cmbRecurrenceEnd);
            tabPageSchedule.Controls.Add(nudOccurrences);
            tabPageSchedule.Controls.Add(dtpRecurrenceEnd);
            tabPageSchedule.Location = new Point(4, 24);
            tabPageSchedule.Name = "tabPageSchedule";
            tabPageSchedule.Size = new Size(1032, 652);
            tabPageSchedule.TabIndex = 0;
            tabPageSchedule.Text = "Schedule";
            // 
            // lblName
            // 
            lblName.Location = new Point(20, 20);
            lblName.Name = "lblName";
            lblName.Size = new Size(100, 23);
            lblName.TabIndex = 0;
            lblName.Text = "Subject";
            // 
            // txtName
            // 
            txtName.Location = new Point(140, 18);
            txtName.Name = "txtName";
            txtName.Size = new Size(860, 23);
            txtName.TabIndex = 1;
            // 
            // lblStart
            // 
            lblStart.Location = new Point(20, 60);
            lblStart.Name = "lblStart";
            lblStart.Size = new Size(100, 23);
            lblStart.TabIndex = 2;
            lblStart.Text = "Start";
            // 
            // dtpStartDate
            // 
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Location = new Point(140, 58);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(160, 23);
            dtpStartDate.TabIndex = 3;
            // 
            // dtpStartTime
            // 
            dtpStartTime.Format = DateTimePickerFormat.Time;
            dtpStartTime.Location = new Point(310, 58);
            dtpStartTime.Name = "dtpStartTime";
            dtpStartTime.ShowUpDown = true;
            dtpStartTime.Size = new Size(110, 23);
            dtpStartTime.TabIndex = 4;
            // 
            // lblTo
            // 
            lblTo.Location = new Point(430, 60);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(30, 23);
            lblTo.TabIndex = 5;
            lblTo.Text = "to";
            // 
            // dtpEndDate
            // 
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Location = new Point(470, 58);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Size = new Size(160, 23);
            dtpEndDate.TabIndex = 6;
            // 
            // dtpEndTime
            // 
            dtpEndTime.Format = DateTimePickerFormat.Time;
            dtpEndTime.Location = new Point(640, 58);
            dtpEndTime.Name = "dtpEndTime";
            dtpEndTime.ShowUpDown = true;
            dtpEndTime.Size = new Size(110, 23);
            dtpEndTime.TabIndex = 7;
            // 
            // chkAllDay
            // 
            chkAllDay.Location = new Point(140, 90);
            chkAllDay.Name = "chkAllDay";
            chkAllDay.Size = new Size(100, 23);
            chkAllDay.TabIndex = 8;
            chkAllDay.Text = "All day";
            // 
            // lblRepeat
            // 
            lblRepeat.Location = new Point(20, 125);
            lblRepeat.Name = "lblRepeat";
            lblRepeat.Size = new Size(100, 23);
            lblRepeat.TabIndex = 9;
            lblRepeat.Text = "Repeat";
            // 
            // cmbRepeat
            // 
            cmbRepeat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRepeat.Location = new Point(140, 123);
            cmbRepeat.Name = "cmbRepeat";
            cmbRepeat.Size = new Size(220, 23);
            cmbRepeat.TabIndex = 10;
            // 
            // pnlWeekly
            // 
            pnlWeekly.Controls.Add(chkSun);
            pnlWeekly.Controls.Add(chkMon);
            pnlWeekly.Controls.Add(chkTue);
            pnlWeekly.Controls.Add(chkWed);
            pnlWeekly.Controls.Add(chkThu);
            pnlWeekly.Controls.Add(chkFri);
            pnlWeekly.Controls.Add(chkSat);
            pnlWeekly.Location = new Point(140, 155);
            pnlWeekly.Name = "pnlWeekly";
            pnlWeekly.Size = new Size(860, 40);
            pnlWeekly.TabIndex = 11;
            // 
            // chkSun
            // 
            chkSun.Location = new Point(0, 8);
            chkSun.Name = "chkSun";
            chkSun.Size = new Size(50, 23);
            chkSun.TabIndex = 0;
            chkSun.Text = "Sun";
            // 
            // chkMon
            // 
            chkMon.Location = new Point(60, 8);
            chkMon.Name = "chkMon";
            chkMon.Size = new Size(50, 23);
            chkMon.TabIndex = 1;
            chkMon.Text = "Mon";
            // 
            // chkTue
            // 
            chkTue.Location = new Point(120, 8);
            chkTue.Name = "chkTue";
            chkTue.Size = new Size(50, 23);
            chkTue.TabIndex = 2;
            chkTue.Text = "Tue";
            // 
            // chkWed
            // 
            chkWed.Location = new Point(180, 8);
            chkWed.Name = "chkWed";
            chkWed.Size = new Size(50, 23);
            chkWed.TabIndex = 3;
            chkWed.Text = "Wed";
            // 
            // chkThu
            // 
            chkThu.Location = new Point(240, 8);
            chkThu.Name = "chkThu";
            chkThu.Size = new Size(50, 23);
            chkThu.TabIndex = 4;
            chkThu.Text = "Thu";
            // 
            // chkFri
            // 
            chkFri.Location = new Point(300, 8);
            chkFri.Name = "chkFri";
            chkFri.Size = new Size(50, 23);
            chkFri.TabIndex = 5;
            chkFri.Text = "Fri";
            // 
            // chkSat
            // 
            chkSat.Location = new Point(360, 8);
            chkSat.Name = "chkSat";
            chkSat.Size = new Size(50, 23);
            chkSat.TabIndex = 6;
            chkSat.Text = "Sat";
            // 
            // pnlMonthly
            // 
            pnlMonthly.Controls.Add(radDayOfMonth);
            pnlMonthly.Controls.Add(nudDayOfMonth);
            pnlMonthly.Controls.Add(radNthWeekday);
            pnlMonthly.Controls.Add(cmbNth);
            pnlMonthly.Controls.Add(cmbDayOfWeek);
            pnlMonthly.Location = new Point(140, 200);
            pnlMonthly.Name = "pnlMonthly";
            pnlMonthly.Size = new Size(860, 60);
            pnlMonthly.TabIndex = 12;
            // 
            // radDayOfMonth
            // 
            radDayOfMonth.Location = new Point(0, 8);
            radDayOfMonth.Name = "radDayOfMonth";
            radDayOfMonth.Size = new Size(80, 23);
            radDayOfMonth.TabIndex = 0;
            radDayOfMonth.Text = "On day";
            // 
            // nudDayOfMonth
            // 
            nudDayOfMonth.Location = new Point(90, 8);
            nudDayOfMonth.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            nudDayOfMonth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudDayOfMonth.Name = "nudDayOfMonth";
            nudDayOfMonth.Size = new Size(60, 23);
            nudDayOfMonth.TabIndex = 1;
            nudDayOfMonth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // radNthWeekday
            // 
            radNthWeekday.Location = new Point(0, 35);
            radNthWeekday.Name = "radNthWeekday";
            radNthWeekday.Size = new Size(60, 23);
            radNthWeekday.TabIndex = 2;
            radNthWeekday.Text = "On the";
            // 
            // cmbNth
            // 
            cmbNth.Items.AddRange(new object[] { "1st", "2nd", "3rd", "4th", "Last" });
            cmbNth.Location = new Point(70, 35);
            cmbNth.Name = "cmbNth";
            cmbNth.Size = new Size(80, 23);
            cmbNth.TabIndex = 3;
            // 
            // cmbDayOfWeek
            // 
            cmbDayOfWeek.Items.AddRange(new object[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });
            cmbDayOfWeek.Location = new Point(160, 35);
            cmbDayOfWeek.Name = "cmbDayOfWeek";
            cmbDayOfWeek.Size = new Size(110, 23);
            cmbDayOfWeek.TabIndex = 4;
            // 
            // lblRecurrenceEnd
            // 
            lblRecurrenceEnd.Location = new Point(20, 270);
            lblRecurrenceEnd.Name = "lblRecurrenceEnd";
            lblRecurrenceEnd.Size = new Size(100, 23);
            lblRecurrenceEnd.TabIndex = 13;
            lblRecurrenceEnd.Text = "End";
            // 
            // cmbRecurrenceEnd
            // 
            cmbRecurrenceEnd.Items.AddRange(new object[] { "Never", "After", "On" });
            cmbRecurrenceEnd.Location = new Point(140, 268);
            cmbRecurrenceEnd.Name = "cmbRecurrenceEnd";
            cmbRecurrenceEnd.Size = new Size(130, 23);
            cmbRecurrenceEnd.TabIndex = 14;
            // 
            // nudOccurrences
            // 
            nudOccurrences.Location = new Point(280, 268);
            nudOccurrences.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudOccurrences.Name = "nudOccurrences";
            nudOccurrences.Size = new Size(80, 23);
            nudOccurrences.TabIndex = 15;
            nudOccurrences.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudOccurrences.Visible = false;
            // 
            // dtpRecurrenceEnd
            // 
            dtpRecurrenceEnd.Location = new Point(280, 268);
            dtpRecurrenceEnd.Name = "dtpRecurrenceEnd";
            dtpRecurrenceEnd.Size = new Size(160, 23);
            dtpRecurrenceEnd.TabIndex = 16;
            dtpRecurrenceEnd.Visible = false;
            // 
            // tabPagePresence
            // 
            tabPagePresence.Controls.Add(lblDetails);
            tabPagePresence.Controls.Add(txtDetails);
            tabPagePresence.Controls.Add(lblState);
            tabPagePresence.Controls.Add(txtState);
            tabPagePresence.Controls.Add(grpLargeImage);
            tabPagePresence.Controls.Add(grpSmallImage);
            tabPagePresence.Controls.Add(grpButtons);
            tabPagePresence.Location = new Point(4, 24);
            tabPagePresence.Name = "tabPagePresence";
            tabPagePresence.Size = new Size(1032, 652);
            tabPagePresence.TabIndex = 1;
            tabPagePresence.Text = "Presence";
            // 
            // lblDetails
            // 
            lblDetails.Location = new Point(30, 20);
            lblDetails.Name = "lblDetails";
            lblDetails.Size = new Size(100, 23);
            lblDetails.TabIndex = 0;
            lblDetails.Text = "Details";
            // 
            // txtDetails
            // 
            txtDetails.Location = new Point(160, 18);
            txtDetails.Name = "txtDetails";
            txtDetails.Size = new Size(830, 23);
            txtDetails.TabIndex = 1;
            // 
            // lblState
            // 
            lblState.Location = new Point(30, 60);
            lblState.Name = "lblState";
            lblState.Size = new Size(100, 23);
            lblState.TabIndex = 2;
            lblState.Text = "State";
            //
            // txtState
            // 
            txtState.Location = new Point(160, 58);
            txtState.Name = "txtState";
            txtState.Size = new Size(830, 23);
            txtState.TabIndex = 3;
            // 
            // grpLargeImage
            // 
            grpLargeImage.Controls.Add(lblLargeKey);
            grpLargeImage.Controls.Add(txtLargeImageKey);
            grpLargeImage.Controls.Add(lblLargeText);
            grpLargeImage.Controls.Add(txtLargeImageText);
            grpLargeImage.Location = new Point(30, 100);
            grpLargeImage.Name = "grpLargeImage";
            grpLargeImage.Size = new Size(950, 85);
            grpLargeImage.TabIndex = 4;
            grpLargeImage.TabStop = false;
            grpLargeImage.Text = "Large Image";
            // 
            // lblLargeKey
            // 
            lblLargeKey.Location = new Point(10, 30);
            lblLargeKey.Name = "lblLargeKey";
            lblLargeKey.Size = new Size(100, 23);
            lblLargeKey.TabIndex = 0;
            lblLargeKey.Text = "Key";
            // 
            // txtLargeImageKey
            // 
            txtLargeImageKey.Location = new Point(120, 28);
            txtLargeImageKey.Name = "txtLargeImageKey";
            txtLargeImageKey.Size = new Size(340, 23);
            txtLargeImageKey.TabIndex = 1;
            // 
            // lblLargeText
            // 
            lblLargeText.Location = new Point(470, 30);
            lblLargeText.Name = "lblLargeText";
            lblLargeText.Size = new Size(33, 23);
            lblLargeText.TabIndex = 2;
            lblLargeText.Text = "Text";
            // 
            // txtLargeImageText
            // 
            txtLargeImageText.Location = new Point(509, 28);
            txtLargeImageText.Name = "txtLargeImageText";
            txtLargeImageText.Size = new Size(421, 23);
            txtLargeImageText.TabIndex = 3;
            // 
            // grpSmallImage
            // 
            grpSmallImage.Controls.Add(lblSmallKey);
            grpSmallImage.Controls.Add(txtSmallImageKey);
            grpSmallImage.Controls.Add(lblSmallText);
            grpSmallImage.Controls.Add(txtSmallImageText);
            grpSmallImage.Location = new Point(30, 195);
            grpSmallImage.Name = "grpSmallImage";
            grpSmallImage.Size = new Size(950, 85);
            grpSmallImage.TabIndex = 5;
            grpSmallImage.TabStop = false;
            grpSmallImage.Text = "Small Image";
            // 
            // lblSmallKey
            // 
            lblSmallKey.Location = new Point(10, 30);
            lblSmallKey.Name = "lblSmallKey";
            lblSmallKey.Size = new Size(100, 23);
            lblSmallKey.TabIndex = 0;
            lblSmallKey.Text = "Key";
            // 
            // txtSmallImageKey
            // 
            txtSmallImageKey.Location = new Point(120, 28);
            txtSmallImageKey.Name = "txtSmallImageKey";
            txtSmallImageKey.Size = new Size(340, 23);
            txtSmallImageKey.TabIndex = 1;
            // 
            // lblSmallText
            // 
            lblSmallText.Location = new Point(470, 30);
            lblSmallText.Name = "lblSmallText";
            lblSmallText.Size = new Size(33, 23);
            lblSmallText.TabIndex = 2;
            lblSmallText.Text = "Text";
            // 
            // txtSmallImageText
            // 
            txtSmallImageText.Location = new Point(509, 28);
            txtSmallImageText.Name = "txtSmallImageText";
            txtSmallImageText.Size = new Size(421, 23);
            txtSmallImageText.TabIndex = 3;
            // 
            // grpButtons
            // 
            grpButtons.Controls.Add(lblBtn1Text);
            grpButtons.Controls.Add(txtBtn1Label);
            grpButtons.Controls.Add(lblBtn1Url);
            grpButtons.Controls.Add(txtBtn1Url);
            grpButtons.Controls.Add(lblBtn2Text);
            grpButtons.Controls.Add(txtBtn2Label);
            grpButtons.Controls.Add(lblBtn2Url);
            grpButtons.Controls.Add(txtBtn2Url);
            grpButtons.Location = new Point(30, 290);
            grpButtons.Name = "grpButtons";
            grpButtons.Size = new Size(950, 170);
            grpButtons.TabIndex = 6;
            grpButtons.TabStop = false;
            grpButtons.Text = "Buttons";
            // 
            // lblBtn1Text
            // 
            lblBtn1Text.Location = new Point(10, 30);
            lblBtn1Text.Name = "lblBtn1Text";
            lblBtn1Text.Size = new Size(100, 23);
            lblBtn1Text.TabIndex = 0;
            lblBtn1Text.Text = "Button 1 Text";
            // 
            // txtBtn1Label
            // 
            txtBtn1Label.Location = new Point(130, 28);
            txtBtn1Label.Name = "txtBtn1Label";
            txtBtn1Label.Size = new Size(300, 23);
            txtBtn1Label.TabIndex = 1;
            // 
            // lblBtn1Url
            // 
            lblBtn1Url.Location = new Point(450, 30);
            lblBtn1Url.Name = "lblBtn1Url";
            lblBtn1Url.Size = new Size(35, 23);
            lblBtn1Url.TabIndex = 2;
            lblBtn1Url.Text = "URL";
            // 
            // txtBtn1Url
            // 
            txtBtn1Url.Location = new Point(491, 27);
            txtBtn1Url.Name = "txtBtn1Url";
            txtBtn1Url.Size = new Size(439, 23);
            txtBtn1Url.TabIndex = 3;
            // 
            // lblBtn2Text
            // 
            lblBtn2Text.Location = new Point(10, 70);
            lblBtn2Text.Name = "lblBtn2Text";
            lblBtn2Text.Size = new Size(100, 23);
            lblBtn2Text.TabIndex = 4;
            lblBtn2Text.Text = "Button 2 Text";
            // 
            // txtBtn2Label
            // 
            txtBtn2Label.Location = new Point(130, 68);
            txtBtn2Label.Name = "txtBtn2Label";
            txtBtn2Label.Size = new Size(300, 23);
            txtBtn2Label.TabIndex = 5;
            // 
            // lblBtn2Url
            // 
            lblBtn2Url.Location = new Point(450, 70);
            lblBtn2Url.Name = "lblBtn2Url";
            lblBtn2Url.Size = new Size(35, 23);
            lblBtn2Url.TabIndex = 6;
            lblBtn2Url.Text = "URL";
            // 
            // txtBtn2Url
            // 
            txtBtn2Url.Location = new Point(491, 67);
            txtBtn2Url.Name = "txtBtn2Url";
            txtBtn2Url.Size = new Size(439, 23);
            txtBtn2Url.TabIndex = 7;
            // 
            // btnSave
            // 
            btnSave.DialogResult = DialogResult.OK;
            btnSave.Location = new Point(820, 680);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 35);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(930, 680);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 35);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            // 
            // EditScheduleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1040, 760);
            Controls.Add(tabControl1);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "EditScheduleForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Schedule";
            tabControl1.ResumeLayout(false);
            tabPageSchedule.ResumeLayout(false);
            tabPageSchedule.PerformLayout();
            pnlWeekly.ResumeLayout(false);
            pnlMonthly.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudDayOfMonth).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudOccurrences).EndInit();
            tabPagePresence.ResumeLayout(false);
            tabPagePresence.PerformLayout();
            grpLargeImage.ResumeLayout(false);
            grpLargeImage.PerformLayout();
            grpSmallImage.ResumeLayout(false);
            grpSmallImage.PerformLayout();
            grpButtons.ResumeLayout(false);
            grpButtons.PerformLayout();
            ResumeLayout(false);
        }
    }
}