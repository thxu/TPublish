using System.IO;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using TPublish.Common;
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
            _settingInfo = settingInfo ?? new MSettingInfo();

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
            try
            {
                _settingInfo.Authour = this.txtAuthor.Text;
                _settingInfo.ApiIpAdress = this.txtApiAdress.Text;
                _settingInfo.ApiKey = this.txtApiKey.Text;
                _settingInfo.MsBuildExePath = this.txtMsBuildPath.Text;

                var isConnect = ApiHelper.Connect(_settingInfo);
                if (!isConnect)
                {
                    MetroMessageBox.Show(this, "服务器连接失败，请检查服务器地址", "无法连接到服务器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                var isSuccess = SettingHelper.SaveSettingInfo(_settingInfo);
                if (!isSuccess)
                {
                    MetroMessageBox.Show(this, "保存失败，请稍后再试", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Close();
            }
            catch (System.Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "保存设置出错", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            this.txtMsBuildPath.Text = SettingHelper.GetMsBuildPath();
        }

        private void btnTestConnect_Click(object sender, System.EventArgs e)
        {
            try
            {
                var settingTmp = _settingInfo.DeepCopy();
                settingTmp.Authour = this.txtAuthor.Text;
                settingTmp.ApiIpAdress = this.txtApiAdress.Text;
                settingTmp.ApiKey = this.txtApiKey.Text;
                var isConnect = ApiHelper.Connect(settingTmp);
                if (!isConnect)
                {
                    MetroMessageBox.Show(this, "服务器连接失败，请检查服务器地址", "无法连接到服务器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                MetroMessageBox.Show(this, "连接成功", "连接成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "连接到服务器出错", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void metroTile1_Click(object sender, System.EventArgs e)
        {
            _settingInfo.MetroThemeStyle = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Dark.GetHashCode() : MetroThemeStyle.Light.GetHashCode();
            this.metroStyleManager1.Theme = (MetroThemeStyle)_settingInfo.MetroThemeStyle;
            //this.Theme = (MetroThemeStyle)_settingInfo.MetroThemeStyle;

            //this.Refresh();
        }

        private void metroTile2_Click(object sender, System.EventArgs e)
        {
            _settingInfo.MetroColorStyle = (_settingInfo.MetroColorStyle + 1) % 15;
            this.metroStyleManager1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            //this.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            //this.Refresh();
        }

        private void SettingForm_Shown(object sender, System.EventArgs e)
        {
            this.metroStyleManager1.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            //this.Theme = _settingInfo.MetroThemeStyle == 0 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;

            if (_settingInfo.MetroColorStyle < 0 || _settingInfo.MetroColorStyle >= 15)
            {
                _settingInfo.MetroColorStyle = MetroColorStyle.Blue.GetHashCode();
            }
            this.metroStyleManager1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            //this.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;

            this.StyleManager = this.metroStyleManager1;
            this.Refresh();
        }
    }
}
