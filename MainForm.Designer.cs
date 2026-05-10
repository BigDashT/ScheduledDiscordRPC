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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblClientId = new System.Windows.Forms.Label();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.chkStartup = new System.Windows.Forms.CheckBox();
            this.dgvSchedules = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)this.dgvSchedules).BeginInit();
            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Text = "Scheduled Discord RPC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // Client ID
            this.lblClientId.Text = "Discord Client ID";
            this.lblClientId.Location = new System.Drawing.Point(20, 20);
            this.lblClientId.Size = new System.Drawing.Size(120, 23);

            this.txtClientId.Location = new System.Drawing.Point(150, 18);
            this.txtClientId.Size = new System.Drawing.Size(400, 23);
            this.txtClientId.Leave += new System.EventHandler(this.txtClientId_Leave);

            // Startup checkbox
            this.chkStartup.Text = "Run on Windows startup";
            this.chkStartup.Location = new System.Drawing.Point(20, 60);
            this.chkStartup.Size = new System.Drawing.Size(300, 24);
            this.chkStartup.CheckedChanged += new System.EventHandler(this.chkStartup_CheckedChanged);

            // DataGridView
            this.dgvSchedules.Location = new System.Drawing.Point(20, 100);
            this.dgvSchedules.Size = new System.Drawing.Size(850, 400);
            this.dgvSchedules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSchedules.MultiSelect = false;
            this.dgvSchedules.AllowUserToAddRows = false;
            this.dgvSchedules.AllowUserToDeleteRows = false;
            this.dgvSchedules.ReadOnly = true;

            // Buttons
            this.btnAdd.Text = "Add";
            this.btnAdd.Location = new System.Drawing.Point(20, 520);
            this.btnAdd.Size = new System.Drawing.Size(100, 35);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnEdit.Text = "Edit";
            this.btnEdit.Location = new System.Drawing.Point(130, 520);
            this.btnEdit.Size = new System.Drawing.Size(100, 35);
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new System.Drawing.Point(240, 520);
            this.btnDelete.Size = new System.Drawing.Size(100, 35);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Location = new System.Drawing.Point(350, 520);
            this.btnRefresh.Size = new System.Drawing.Size(100, 35);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // Status label
            this.lblStatus.Text = "Ready";
            this.lblStatus.Location = new System.Drawing.Point(20, 580);
            this.lblStatus.Size = new System.Drawing.Size(850, 23);
            this.lblStatus.AutoSize = true;

            this.Controls.Add(this.lblClientId);
            this.Controls.Add(this.txtClientId);
            this.Controls.Add(this.chkStartup);
            this.Controls.Add(this.dgvSchedules);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblStatus);

            ((System.ComponentModel.ISupportInitialize)this.dgvSchedules).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}