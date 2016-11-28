using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FogBugzForVisualStudio
{
    public partial class frmLogOn : Form
    {
        public frmLogOn()
        {
            InitializeComponent();
        }

        public void ShowError(string sError)
        {
            lblError.Visible = true;
            lblError.Text = sError;
            lblError.Size = lblError.GetPreferredSize(new Size(lblError.Size.Width, 0));
            panelMain.Location = new Point(panelMain.Location.X, lblError.Location.Y + lblError.Size.Height + 6);
            this.Size = new Size(this.Size.Width, panelMain.Location.Y + panelMain.Size.Height + 38);
        }

        private void frmLogOn_Shown(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(fldURL.Text))
            {
                fldURL.Focus();
            }
            else if (cmbUser.Visible)
            {
                cmbUser.Focus();
            }
            else if (String.IsNullOrEmpty(fldUser.Text))
            {
                fldUser.Focus();
            }
            else
            {
                fldPassword.Focus();
            }
        }
    }
}
