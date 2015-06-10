using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;

namespace ZwiZwit
{
    public partial class LoginForm : Form
    {
        public TwitterAccess Twitter { get; set; }
        public string RequestOauthToken { get; set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            LoginButton.Enabled = false;

            Show();
            try
            {
                //string url = Twitter.PostOauthRequestToken();
                string url = null;
                string token = null;
                Twitter.PostOauthRequestToken(out url, out token);
                UrlText.Text = url;
                RequestOauthToken = token;
            }
            catch (WebException we)
            {
                MessageBox.Show(we.Message);
                Close();
                return;
            }

            Process.Start(UrlText.Text);
            //LoginButton.Enabled = true;
            PinText.Focus();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            PinText.Text = PinText.Text.Trim();
        }

        private void PinText_TextChanged(object sender, EventArgs e)
        {
            LoginButton.Enabled = (PinText.Text.Length > 0);
        }

    }
}
