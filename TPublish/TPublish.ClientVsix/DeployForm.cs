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

                lbAppPath.Text = _appViews[0].AppPhysicalPath;
            }
            else
            {
                lbAppType.Text = "Exe";
                AppView view = TPublishService.GetExeAppView(_projModel.LibName);
                cbAppName.Items.Add(view?.AppName ?? string.Empty);
                lbAppPath.Text = view?.AppPhysicalPath ?? string.Empty;
            }
            cbAppName.SelectedIndex = 0;

            //txtId.Text = _projModel.LibName;
            //txtVersion.Text = _projModel.Version;
            //txtAuthors.Text = _projModel.Author;
            //txtOwners.Text = string.Join(",", _projModel.Owners);
            //txtDesc.Text = _projModel.Desc;
        }

        private void cbAppName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lbAppPath.Text = _appViews[cbAppName.SelectedIndex].AppPhysicalPath;
        }
    }


}
