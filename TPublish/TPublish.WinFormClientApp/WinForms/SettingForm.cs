using System.IO;
using System.Windows.Forms;
using MetroFramework.Forms;
using TPublish.WinFormClientApp.Model;
using TPublish.WinFormClientApp.Utils;

namespace TPublish.WinFormClientApp.WinForms
{
    public partial class SettingForm : MetroForm
    {
        private MSettingInfo _settingInfo;
        public SettingForm(MSettingInfo settingInfo)
        {
            InitializeComponent();
            _settingInfo = settingInfo;

            this.txtAuthor.Text = _settingInfo.Authour;
            this.txtApiAdress.Text = _settingInfo.ApiIpAdress;
            this.txtApiKey.Text = _settingInfo.ApiKey;
            this.txtMsBuildPath.Text = _settingInfo.MsBuildExePath;
        }

        private void btnSelectMsBuild_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "Exe文件(*.exe)|*.exe";
            var file = new FileInfo(_settingInfo.MsBuildExePath).Directory;
            fileDialog.InitialDirectory = file.FullName;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtMsBuildPath.Text = _settingInfo.MsBuildExePath;
            }
        }

        private void btnSaveSetting_Click(object sender, System.EventArgs e)
        {
            _settingInfo.Authour = this.txtAuthor.Text;
            _settingInfo.ApiIpAdress = this.txtApiAdress.Text;
            _settingInfo.ApiKey = this.txtApiKey.Text;
            _settingInfo.MsBuildExePath = this.txtMsBuildPath.Text;

            var isConnect = ApiHelper.Connect(_settingInfo);
            if (!isConnect)
            {
                MessageBox.Show("服务器连接失败，请检查服务器地址");
                return;
            }


            var isSuccess = SettingHelper.SaveSettingInfo(_settingInfo);
            if (!isSuccess)
            {
                MessageBox.Show("保存失败，请稍后再试");
                return;
            }
            this.Close();
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            this.txtMsBuildPath.Text = SettingHelper.GetMsBuildPath();
        }

        private void btnTestConnect_Click(object sender, System.EventArgs e)
        {
            var isConnect = ApiHelper.Connect(_settingInfo);
            if (!isConnect)
            {
                MessageBox.Show("服务器连接失败，请检查服务器地址");
                return;
            }
            MessageBox.Show("连接成功");
        }
    }
}
