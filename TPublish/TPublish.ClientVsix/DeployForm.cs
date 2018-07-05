using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TPublish.ClientVsix.Model;
using TPublish.ClientVsix.Service;

namespace TPublish.ClientVsix
{
    public partial class DeployForm : Form
    {
        protected ProjModel _projModel;
        private List<AppView> _appViews;

        public DeployForm()
        {
            InitializeComponent();
        }
        public void Ini(ProjModel projModel)
        {
            _projModel = projModel;

            if (_projModel.ProjType == "Library")
            {
                lbAppType.Text = "IIS";
                _appViews = TPublishService.GetAllIISAppNames();
                foreach (AppView appView in _appViews)
                {
                    cbAppName.Items.Add(appView.AppName);
                }

                //lbAppPath.Text = _appViews[0].AppPhysicalPath;
                showAppPath(_appViews[0].AppPhysicalPath);
            }
            else
            {
                lbAppType.Text = "Exe";
                AppView view = TPublishService.GetExeAppView(_projModel.LibName);
                cbAppName.Items.Add(view?.AppName ?? string.Empty);
                //lbAppPath.Text = view?.AppPhysicalPath ?? string.Empty;
                showAppPath(view?.AppPhysicalPath ?? string.Empty);
            }
            cbAppName.SelectedIndex = 0;
        }

        private void showAppPath(string path)
        {
            int rowNum = 10;
            float fontWidth = (float)lbAppPath.Width / lbAppPath.Text.Length;
            int RowHeight = 15;
            int colNum = (path.Length - (path.Length / rowNum) * rowNum) == 0 ? (path.Length / rowNum) : (path.Length / rowNum) + 1;
            lbAppPath.AutoSize = false;
            lbAppPath.Width = (int)(fontWidth * 10.0);
            lbAppPath.Height = RowHeight * colNum;
        }

        private void cbAppName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //lbAppPath.Text = _appViews[cbAppName.SelectedIndex].AppPhysicalPath;
            showAppPath(_appViews[cbAppName.SelectedIndex].AppPhysicalPath);
        }

        private void btnDeploy_Click(object sender, System.EventArgs e)
        {
            try
            {
                string appName = cbAppName.SelectedItem.ToString();
                if (string.IsNullOrWhiteSpace(appName))
                {
                    MessageBox.Show("请选择要发布的项目");
                    return;
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
