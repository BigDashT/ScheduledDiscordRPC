// ================================================
// MAINFORM.DESIGNER.cs
namespace ScheduledDiscordRPC
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblClientId;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.CheckBox chkStartup;
        private System.Windows.Forms.DataGridView dgvSchedules;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                // _trayIcon/_trayMenu/_timer/_client (declared in MainForm.cs) aren't added to
                // the designer's component container, so they need explicit cleanup here.
                // Hiding the tray icon before disposing it (rather than relying on process
                // teardown) avoids the classic WinForms bug where a "ghost" tray icon lingers in
                // the taskbar until the user mouses over it.
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
                _trayMenu.Dispose();
                _timer.Stop();
                _timer.Dispose();
                _client?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            lblClientId = new Label();
            txtClientId = new TextBox();
            chkStartup = new CheckBox();
            dgvSchedules = new DataGridView();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvSchedules).BeginInit();
            SuspendLayout();
            //
            // lblClientId
            //
            lblClientId.Location = new Point(24, 26);
            lblClientId.Name = "lblClientId";
            lblClientId.Size = new Size(190, 23);
            lblClientId.TabIndex = 0;
            lblClientId.Text = "Discord Application ID";
            //
            // txtClientId
            //
            txtClientId.Location = new Point(220, 23);
            txtClientId.Name = "txtClientId";
            txtClientId.Size = new Size(420, 26);
            txtClientId.TabIndex = 1;
            txtClientId.Leave += txtClientId_Leave;
            //
            // chkStartup
            //
            chkStartup.Location = new Point(24, 68);
            chkStartup.Name = "chkStartup";
            chkStartup.Size = new Size(300, 26);
            chkStartup.TabIndex = 2;
            chkStartup.Text = "Run on Windows startup";
            chkStartup.CheckedChanged += chkStartup_CheckedChanged;
            //
            // dgvSchedules
            //
            dgvSchedules.AllowUserToAddRows = false;
            dgvSchedules.AllowUserToDeleteRows = false;
            dgvSchedules.Location = new Point(24, 112);
            dgvSchedules.MultiSelect = false;
            dgvSchedules.Name = "dgvSchedules";
            dgvSchedules.ReadOnly = true;
            dgvSchedules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSchedules.Size = new Size(912, 392);
            dgvSchedules.TabIndex = 3;
            //
            // btnAdd
            //
            btnAdd.Location = new Point(24, 518);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(110, 40);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "Add";
            btnAdd.Click += btnAdd_Click;
            //
            // btnEdit
            //
            btnEdit.Location = new Point(146, 518);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(110, 40);
            btnEdit.TabIndex = 5;
            btnEdit.Tag = "secondary";
            btnEdit.Text = "Edit";
            btnEdit.Click += btnEdit_Click;
            //
            // btnDelete
            //
            btnDelete.Location = new Point(268, 518);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(110, 40);
            btnDelete.TabIndex = 6;
            btnDelete.Tag = "danger";
            btnDelete.Text = "Delete";
            btnDelete.Click += btnDelete_Click;
            //
            // btnRefresh
            //
            btnRefresh.Location = new Point(390, 518);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(110, 40);
            btnRefresh.TabIndex = 7;
            btnRefresh.Tag = "secondary";
            btnRefresh.Text = "Refresh";
            btnRefresh.Click += btnRefresh_Click;
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(24, 574);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(39, 15);
            lblStatus.TabIndex = 8;
            lblStatus.Tag = "secondary";
            lblStatus.Text = "Ready";
            //
            // MainForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(960, 612);
            Controls.Add(lblClientId);
            Controls.Add(txtClientId);
            Controls.Add(chkStartup);
            Controls.Add(dgvSchedules);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Controls.Add(lblStatus);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Scheduled Discord RPC";
            ((System.ComponentModel.ISupportInitialize)dgvSchedules).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}