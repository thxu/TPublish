using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using TPublish.Common;
using TPublish.WinFormClientApp.Model;
using TPublish.WinFormClientApp.Utils;

namespace TPublish.WinFormClientApp.WinForms
{
    public partial class SelectProjectForm : MetroForm
    {
        private MSettingInfo _settingInfo;
        public static Action<MSelectedItem> ProjSelectedEvent;

        public SelectProjectForm(MSettingInfo settingInfo)
        {
            InitializeComponent();
            _settingInfo = settingInfo;
        }

        private void SelectProjectForm_Shown(object sender, EventArgs e)
        {
            _settingInfo.SelectedItems = _settingInfo.SelectedItems.OrderByDescending(n => n.CreateTime).ToList();
            this.ListSelectedRecords.View = View.Details;

            if (_settingInfo.SelectedItems != null && _settingInfo.SelectedItems.Any())
            {
                ListViewGroup folderGroup = new ListViewGroup();
                folderGroup.Header = "文件夹";
                folderGroup.HeaderAlignment = HorizontalAlignment.Left;

                ListViewGroup projGroup = new ListViewGroup();
                projGroup.Header = "csproj项目";
                projGroup.HeaderAlignment = HorizontalAlignment.Left;

                this.ListSelectedRecords.Groups.Add(folderGroup);
                this.ListSelectedRecords.Groups.Add(projGroup);

                foreach (MSelectedItem selectedItem in _settingInfo.SelectedItems)
                {
                    ListViewItem item = new ListViewItem() { Text = selectedItem.Name, SubItems = { selectedItem.Path }, Tag = selectedItem.Guid };

                    if (selectedItem.Type == 2)
                    {
                        projGroup.Items.Add(item);
                    }
                    else
                    {
                        folderGroup.Items.Add(item);
                    }

                    this.ListSelectedRecords.Items.Add(item);
                }
            }

            ListSelectedRecords.Columns[0].Width = -2;
            ListSelectedRecords.Columns[1].Width = -2;
            ListSelectedRecords.Columns[0].TextAlign = HorizontalAlignment.Left;
            ListSelectedRecords.Columns[1].TextAlign = HorizontalAlignment.Left;
        }

        private void btnSelectProj_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "请选择C#项目文件";
            dlg.Filter = "C#项目文件(*.csproj)|*.csproj";
            if (dlg.ShowDialog() == DialogResult.OK && !dlg.SafeFileName.IsNullOrEmpty())
            {
                MSelectedItem item = _settingInfo.SelectedItems.FirstOrDefault(n => n.Path == dlg.FileName);
                if (item == null)
                {
                    item = new MSelectedItem()
                    {
                        Type = 2,
                        Name = Path.GetFileNameWithoutExtension(dlg.SafeFileName),
                        Path = dlg.FileName,
                        Guid = Guid.NewGuid().ToString(),
                        CreateTime = DateTime.Now
                    };
                    _settingInfo.SelectedItems.Add(item);
                    SettingHelper.SaveSettingInfo(_settingInfo);
                }
                //ProjSelectedEvent?.Invoke(item);
                ProjSelectedEvent?.BeginInvoke(item, null, null);
                this.Close();
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var fsd = new FolderSelectDialog())
            {
                fsd.Title = "选择文件夹";
                if (fsd.ShowDialog(this.Handle))
                {
                    var folder = fsd.FileName;
                    if (!string.IsNullOrWhiteSpace(folder) && Directory.Exists(folder))
                    {
                        this.DialogResult = DialogResult.OK;

                        DirectoryInfo dir = new DirectoryInfo(folder);
                        MSelectedItem item = _settingInfo.SelectedItems.FirstOrDefault(n => n.Path == dir.FullName);
                        if (item == null)
                        {
                            item = new MSelectedItem() { Type = 3, Name = dir.Name, Path = dir.FullName, Guid = Guid.NewGuid().ToString(), CreateTime = DateTime.Now };
                            _settingInfo.SelectedItems.Add(item);
                            SettingHelper.SaveSettingInfo(_settingInfo);
                        }
                        ProjSelectedEvent?.BeginInvoke(item, null, null);
                        this.Close();
                    }
                }
            }


            //FolderBrowserDialog dlg = new FolderBrowserDialog();
            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    DirectoryInfo dir = new DirectoryInfo(dlg.SelectedPath);
            //    MSelectedItem item = _settingInfo.SelectedItems.FirstOrDefault(n => n.Path == dir.FullName);
            //    if (item == null)
            //    {
            //        item = new MSelectedItem() { Type = 3, Name = dir.Name, Path = dir.FullName, Guid = Guid.NewGuid().ToString(), CreateTime = DateTime.Now };
            //        _settingInfo.SelectedItems.Add(item);
            //        SettingHelper.SaveSettingInfo(_settingInfo);
            //    }
            //    ProjSelectedEvent?.Invoke(item);
            //    this.Close();
            //}
        }

        private void ListSelectedRecords_DoubleClick(object sender, EventArgs e)
        {
            ListView listView = (ListView)sender;
            var guid = listView.FocusedItem.Tag.ToString();
            var item = _settingInfo.SelectedItems.First(n => n.Guid == guid);
            item.CreateTime = DateTime.Now;
            SettingHelper.SaveSettingInfo(_settingInfo);
            ProjSelectedEvent?.BeginInvoke(item, null, null);
            this.Close();
        }
    }
}
