using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using TPublish.Common.Model;
using TPublish.WinFormClientApp.Model;
using TPublish.WinFormClientApp.Utils;

namespace TPublish.WinFormClientApp.WinForms
{
    public partial class ServiceForm : MetroForm
    {
        private ProjectModel _projectModel = null;
        private MProjectSettingInfo _projectSetting;
        private MSettingInfo _settingInfo = new MSettingInfo();
        private List<AppView> _appViews = new List<AppView>();
        private bool _isInit = false;

        public static Action<string, string> ServiceSelectedEvent;

        public ServiceForm(ProjectModel projectModel, MProjectSettingInfo projectSetting, MSettingInfo settingInfo)
        {
            InitializeComponent();
            _projectModel = projectModel ?? new ProjectModel();
            _projectSetting = projectSetting ?? new MProjectSettingInfo();
            _settingInfo = settingInfo ?? new MSettingInfo();
        }

        private void showLbText(MetroLabel lb, string text)
        {
            toolTip1.InitialDelay = 300;
            toolTip1.SetToolTip(lb, text);
            if (text.Length >= 100)
            {
                text = new string(text.Take(15).ToArray()) + "....." + new string(text.Skip(20).ToArray());
            }
            if (text.Length >= 50)
            {
                text = new string(text.Take(50).ToArray()) + Environment.NewLine + new string(text.Skip(50).ToArray());
            }
            lb.Text = text;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void cbProjType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            MetroComboBox comboBox = (MetroComboBox)sender;
            _appViews = comboBox.SelectedIndex == 0
                ? ApiHelper.GetAllIISAppNames(_settingInfo)
                : ApiHelper.GetExeAppView(_settingInfo, _projectModel.ProjType == 3 ? "" : _projectModel.ProjName);
            this.cbServiceName.DataSource = _appViews;
            this.cbServiceName.DisplayMember = "AppAlias";
            this.cbServiceName.ValueMember = "AppPhysicalPath";
            this.cbServiceName.SelectedIndex = _appViews.FindIndex(n => n.Id == _projectSetting.LastChooseAppName);
            showLbText(this.lbSerPath, (this.cbServiceName.SelectedItem as AppView)?.AppPhysicalPath ?? string.Empty);
        }

        private void ServiceForm_Shown(object sender, EventArgs e)
        {
            try
            {
                _isInit = true;
                if (_projectModel.OutPutType == "Library")
                {
                    this.cbProjType.SelectedIndex = 0;
                }
                else if (_projectModel.OutPutType == "Exe")
                {
                    this.cbProjType.SelectedIndex = 1;
                }
                else
                {
                    if (_projectSetting.Type == "Library")
                    {
                        this.cbProjType.SelectedIndex = 0;
                    }
                    else if (_projectSetting.Type == "Exe")
                    {
                        this.cbProjType.SelectedIndex = 1;
                    }
                }

                _isInit = false;
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "部署服务器窗体初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbServiceName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInit)
            {
                return;
            }
            showLbText(this.lbSerPath, (this.cbServiceName.SelectedItem as AppView)?.AppPhysicalPath ?? string.Empty);
            _projectSetting.LastChooseAppName = (this.cbServiceName.SelectedItem as AppView)?.Id;

        }

        private void btnDeploy_Click(object sender, EventArgs e)
        {
            try
            {
                _projectSetting.Type = this.cbProjType.SelectedIndex == 0 ? "Library" : "Exe";
                ProjectHelper.SaveProjectSettingInfo(_projectSetting);

                var view = this.cbServiceName.SelectedItem as AppView;
                if (string.IsNullOrWhiteSpace(view?.AppAlias))
                {
                    MetroMessageBox.Show(this, "请选择要发布的项目", "未选择项目", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ServiceSelectedEvent?.BeginInvoke(_projectSetting.Type, view.Id, null, null);
                this.Close();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "部署处理错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
