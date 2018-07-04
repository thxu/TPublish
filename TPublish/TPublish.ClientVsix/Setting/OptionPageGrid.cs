using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace TPublish.ClientVsix.Setting
{
    public class OptionPageGrid : DialogPage
    {
        [Category("TPublishSetting")]
        [DisplayName("Authour")]
        [Description("Authour")]
        public string Authour { get; set; } = "";

        [Category("TPublishSetting")]
        [DisplayName("IpAdress")]
        [Description("IpAdress,eg:http://192.168.10.16:8081")]
        public string IpAdress { get; set; } = "";

        public string GetApiUrl()
        {
            return $"{IpAdress}/ClientApi/UploadZip";
        }

        protected override IWin32Window Window
        {
            get
            {
                TPublishSettingControl page = new TPublishSettingControl { OptionPage = this };
                page.Initialize();
                return page;
            }
        }
    }
}
