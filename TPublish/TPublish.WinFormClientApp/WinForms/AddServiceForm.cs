using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using TPublish.Common;
using TPublish.WinFormClientApp.Model;
using TPublish.WinFormClientApp.Utils;

namespace TPublish.WinFormClientApp.WinForms
{
    public partial class AddServiceForm : MetroForm
    {
        private MSettingInfo _settingInfo;

        public static Action<ServiceInfo> ServiceSaveEvent;

        public AddServiceForm(MSettingInfo settingInfo)
        {
            InitializeComponent();
            this._settingInfo = settingInfo.DeepCopy();
        }

        private void AddServiceForm_Shown(object sender, EventArgs e)
        {
            try
            {

                this.metroStyleManager1.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;

                if (_settingInfo.MetroColorStyle < 0 || _settingInfo.MetroColorStyle >= 15)
                {
                    _settingInfo.MetroColorStyle = MetroColorStyle.Blue.GetHashCode();
                }
                this.metroStyleManager1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;

                this.StyleManager = this.metroStyleManager1;

                this.Refresh();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "添加服务器页面初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestConnect_Click(object sender, EventArgs e)
        {
            try
            {
                var isConnect = ApiHelper.Connect(new ServiceInfo() { ApiIpAdress = this.txtApiAdress.Text.Trim() });
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtApiAdress.Text.IsNullOrEmpty())
                {
                    lbApiAdressErr.Text = "服务器地址不能为空";
                    lbApiAdressErr.Visible = true;
                    return;
                }
                if (this.txtApiAlias.Text.IsNullOrEmpty())
                {
                    lbApiAliasErr.Text = "服务器别名不能为空";
                    lbApiAliasErr.Visible = true;
                    return;
                }

                if (_settingInfo.ServiceInfos != null && _settingInfo.ServiceInfos.Any(n => n.ApiIpAdress.Trim() == this.txtApiAdress.Text.Trim()))
                {
                    lbApiAdressErr.Text = "此服务器地址已经添加过了";
                    lbApiAdressErr.Visible = true;
                    return;
                }

                ServiceInfo info = new ServiceInfo()
                {
                    Alias = this.txtApiAlias.Text.Trim(),
                    ApiIpAdress = this.txtApiAdress.Text.Trim(),
                    ApiKey = string.Empty,
                    IsDefault = false,
                };

                ServiceSaveEvent?.BeginInvoke(info, null, null);
                this.Close();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "保存服务器信息出错", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
