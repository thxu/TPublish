using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace TPublish.VsixClient2019.Settings
{
    [Guid("fdc69487-0b5a-4fba-8ee2-526d24db9494")]
    public class OptionPageGrid : DialogPage
    {
        [Category("PublishSetting")]
        [DisplayName("Authour")]
        [Description("Authour")]
        public string Authour { get; set; } = "";

        [Category("PublishSetting")]
        [DisplayName("IpAdress")]
        [Description("IpAdress,eg:http://192.168.10.19:8081")]
        public string IpAdress { get; set; } = "";

        public string GetApiUrl()
        {
            return $"{IpAdress}/ClientApi";
        }

        protected override IWin32Window Window
        {
            get
            {
                PublishSettingControl page = new PublishSettingControl { OptionPage = this };
                page.Initialize();
                return page;
            }
        }
    }
}
