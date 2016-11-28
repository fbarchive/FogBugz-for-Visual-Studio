namespace FogBugzForVisualStudio
{
    partial class frmLogOn
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fldURL = new System.Windows.Forms.TextBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.fldUser = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.fldPassword = new System.Windows.Forms.TextBox();
            this.btnLogOn = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbUser = new System.Windows.Forms.ComboBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.lblError = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // fldURL
            // 
            this.fldURL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldURL.Location = new System.Drawing.Point(102, 15);
            this.fldURL.Name = "fldURL";
            this.fldURL.Size = new System.Drawing.Size(198, 23);
            this.fldURL.TabIndex = 1;
            // 
            // lblURL
            // 
            this.lblURL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblURL.Location = new System.Drawing.Point(12, 16);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(86, 18);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "FogBugz URL:";
            this.lblURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUser
            // 
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(12, 44);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(86, 18);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "User:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fldUser
            // 
            this.fldUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldUser.Location = new System.Drawing.Point(102, 43);
            this.fldUser.Name = "fldUser";
            this.fldUser.Size = new System.Drawing.Size(198, 23);
            this.fldUser.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(12, 72);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(86, 18);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fldPassword
            // 
            this.fldPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldPassword.Location = new System.Drawing.Point(102, 71);
            this.fldPassword.Name = "fldPassword";
            this.fldPassword.Size = new System.Drawing.Size(198, 23);
            this.fldPassword.TabIndex = 6;
            this.fldPassword.UseSystemPasswordChar = true;
            // 
            // btnLogOn
            // 
            this.btnLogOn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLogOn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOn.Location = new System.Drawing.Point(225, 101);
            this.btnLogOn.Name = "btnLogOn";
            this.btnLogOn.Size = new System.Drawing.Size(75, 23);
            this.btnLogOn.TabIndex = 7;
            this.btnLogOn.Text = "Log On";
            this.btnLogOn.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(144, 101);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cmbUser
            // 
            this.cmbUser.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbUser.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(102, 43);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(198, 23);
            this.cmbUser.TabIndex = 4;
            this.cmbUser.Visible = false;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.fldUser);
            this.panelMain.Controls.Add(this.cmbUser);
            this.panelMain.Controls.Add(this.btnCancel);
            this.panelMain.Controls.Add(this.lblUser);
            this.panelMain.Controls.Add(this.fldURL);
            this.panelMain.Controls.Add(this.lblURL);
            this.panelMain.Controls.Add(this.lblPassword);
            this.panelMain.Controls.Add(this.btnLogOn);
            this.panelMain.Controls.Add(this.fldPassword);
            this.panelMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(308, 134);
            this.panelMain.TabIndex = 9;
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(12, 14);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(284, 29);
            this.lblError.TabIndex = 9;
            this.lblError.Text = "label1";
            this.lblError.Visible = false;
            // 
            // frmLogOn
            // 
            this.AcceptButton = this.btnLogOn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(308, 134);
            this.ControlBox = false;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.lblError);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogOn";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log On to FogBugz";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.frmLogOn_Shown);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnLogOn;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox fldURL;
        public System.Windows.Forms.TextBox fldUser;
        public System.Windows.Forms.TextBox fldPassword;
        public System.Windows.Forms.ComboBox cmbUser;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblError;

    }
}