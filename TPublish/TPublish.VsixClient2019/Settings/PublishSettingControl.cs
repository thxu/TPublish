using System;
using System.Windows.Forms;

namespace TPublish.VsixClient2019.Settings
{
    public partial class PublishSettingControl : UserControl
    {
        internal OptionPageGrid OptionPage;

        public PublishSettingControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            txtAuthour.Text = OptionPage.Authour;
            txtIpAdress.Text = OptionPage.IpAdress;
        }

        private void txtAuthour_TextChanged(object sender, EventArgs e)
        {
            OptionPage.Authour = txtAuthour.Text;
        }

        private void txtIpAdress_TextChanged(object sender, EventArgs e)
        {
            if (!txtIpAdress.Text.StartsWith("http://"))
            {
                txtIpAdress.Text = "http://" + txtIpAdress.Text;
            }
            OptionPage.IpAdress = txtIpAdress.Text.TrimEnd('/');
            txtIpAdress.SelectionStart = txtIpAdress.TextLength;
            txtIpAdress.Focus();
        }
    }
}
