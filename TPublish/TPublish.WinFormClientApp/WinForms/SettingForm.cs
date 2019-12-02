using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
                _settingInfo.MsBuildExePath = this.txtMsBuildPath.Text;

                var isSuccess = SettingHelper.SaveSettingInfo(_settingInfo);
                if (!isSuccess)
                {
                    MetroMessageBox.Show(this, "保存失败，请稍后再试", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.DialogResult = DialogResult.OK;
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

        private void metroTile1_Click(object sender, System.EventArgs e)
        {
            _settingInfo.MetroThemeStyle = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Dark.GetHashCode() : MetroThemeStyle.Light.GetHashCode();
            this.metroStyleManager1.Theme = (MetroThemeStyle)_settingInfo.MetroThemeStyle;
            this.metroToolTip1.Theme = (MetroThemeStyle)_settingInfo.MetroThemeStyle;
        }

        private void metroTile2_Click(object sender, System.EventArgs e)
        {
            _settingInfo.MetroColorStyle = (_settingInfo.MetroColorStyle + 1) % 15;
            this.metroStyleManager1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.metroToolTip1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
        }

        private void SettingForm_Shown(object sender, System.EventArgs e)
        {
            try
            {
                RefreshServiceList();

                this.metroStyleManager1.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;

                if (_settingInfo.MetroColorStyle < 0 || _settingInfo.MetroColorStyle >= 15)
                {
                    _settingInfo.MetroColorStyle = MetroColorStyle.Blue.GetHashCode();
                }
                this.metroStyleManager1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;

                this.StyleManager = this.metroStyleManager1;
                this.metroToolTip1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
                this.metroToolTip1.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;

                this.Refresh();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "设置页面初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshServiceList()
        {
            if (_settingInfo.ServiceInfos != null && _settingInfo.ServiceInfos.Any())
            {
                this.gridServices.Rows.Clear();
                if (!_settingInfo.ServiceInfos.Any(n=>n.IsDefault))
                {
                    var first = _settingInfo.ServiceInfos.First();
                    first.IsDefault = true;
                }
                foreach (ServiceInfo serviceInfo in _settingInfo.ServiceInfos)
                {
                    var index = this.gridServices.Rows.Add();
                    this.gridServices.Rows[index].Cells[0] = new DataGridViewTextBoxCell()
                    {
                        Value = $"{serviceInfo.Alias}{(serviceInfo.IsDefault ? "(默认)" : "")}",
                        Style = new DataGridViewCellStyle()
                        {
                            Font = serviceInfo.IsDefault ? new Font(DefaultFont, FontStyle.Italic | FontStyle.Bold) : DefaultFont,
                        }
                    };
                    this.gridServices.Rows[index].Cells[1] = new DataGridViewTextBoxCell()
                    {
                        Value = serviceInfo.ApiIpAdress,
                        Style = new DataGridViewCellStyle()
                        {
                            Font = serviceInfo.IsDefault ? new Font(DefaultFont, FontStyle.Italic | FontStyle.Bold) : DefaultFont,
                        }
                    };
                }
                this.gridServices.Refresh();
            }
        }

        private void gridServices_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (gridServices.Rows[e.RowIndex].Selected == false)
                    {
                        gridServices.ClearSelection();
                        gridServices.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (gridServices.SelectedRows.Count == 1)
                    {
                        gridServices.CurrentCell = gridServices.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    menuOfServiceGrid.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddServiceForm form = new AddServiceForm(this._settingInfo);
            AddServiceForm.ServiceSaveEvent = info =>
            {
                if (_settingInfo.ServiceInfos == null)
                {
                    _settingInfo.ServiceInfos = new List<ServiceInfo>();
                }

                if (_settingInfo.ServiceInfos.Count == 0)
                {
                    info.IsDefault = true;
                }
                this._settingInfo.ServiceInfos.Add(info);
            };
            form.Activate();
            form.ShowDialog();
            RefreshServiceList();
        }

        private void DelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currRow = this.gridServices.CurrentRow;
            if (currRow == null)
            {
                return;
            }

            var apiService = currRow.Cells[1].Value.ToString().Trim();
            if (apiService.IsNullOrEmpty())
            {
                return;
            }

            if (_settingInfo.ServiceInfos != null && _settingInfo.ServiceInfos.Any())
            {
                for (int i = _settingInfo.ServiceInfos.Count - 1; i >= 0; i--)
                {
                    if (_settingInfo.ServiceInfos[i].ApiIpAdress.Trim() == apiService)
                    {
                        _settingInfo.ServiceInfos.RemoveAt(i);
                        break;
                    }
                }
            }
            this.RefreshServiceList();
        }

        private void SetDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currRow = this.gridServices.CurrentRow;
            if (currRow == null)
            {
                return;
            }
            var apiService = currRow.Cells[1].Value.ToString().Trim();
            if (apiService.IsNullOrEmpty())
            {
                return;
            }
            if (_settingInfo.ServiceInfos != null && _settingInfo.ServiceInfos.Any())
            {
                for (int i = _settingInfo.ServiceInfos.Count - 1; i >= 0; i--)
                {
                    _settingInfo.ServiceInfos[i].IsDefault = _settingInfo.ServiceInfos[i].ApiIpAdress.Trim() == apiService;
                }
            }
            this.RefreshServiceList();
        }
    }
}
