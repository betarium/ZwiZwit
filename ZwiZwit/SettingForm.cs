using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZwiZwit
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void updateInterval_Validating(object sender, CancelEventArgs e)
        {
            TextBox ctrl = (TextBox)sender;
            int val = 0;
            if (!int.TryParse(ctrl.Text, out val))
            {
                e.Cancel = true;
            }
        }

        private void reloadCount_Validating(object sender, CancelEventArgs e)
        {
            TextBox ctrl = (TextBox)sender;
            int val = 0;
            if (!int.TryParse(ctrl.Text, out val))
            {
                e.Cancel = true;
            }
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

        }
    }
}
