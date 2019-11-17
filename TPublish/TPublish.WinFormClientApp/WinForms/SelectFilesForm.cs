using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using TPublish.WinFormClientApp.Model;
using TPublish.WinFormClientApp.Utils;

namespace TPublish.WinFormClientApp.WinForms
{
    public partial class SelectFilesForm : MetroForm
    {
        private MProjectSettingInfo _projectSetting;
        private string _basePath = string.Empty;
        private bool _isChkAllChanging = false;
        private bool _isChildChkChanging = false;
        private Dictionary<string, bool> extList = new Dictionary<string, bool>();
        private int _selectType = 1;

        public static Action<List<string>> FileSaveEvent;

        public SelectFilesForm(MProjectSettingInfo projectSetting, string basePath)
        {
            InitializeComponent();
            _projectSetting = projectSetting;
            _basePath = basePath;
        }

        private void tvFiles_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeViewCheck.CheckControl(e);
        }

        private void RefreshTreeView()
        {
            tvFiles.Nodes.Clear();
            DirectoryInfo root = new DirectoryInfo(_basePath);
            TreeNode rootNode = new TreeNode { Text = " 全选", Tag = null };
            tvFiles.Nodes.Add(rootNode);
            bool isDirExist = AddAllDirs(root.GetDirectories(), rootNode.Nodes);
            bool isFileExist = AddAllFiles(root, rootNode.Nodes);
            rootNode.Checked = isDirExist || isFileExist;
            rootNode.Expand();
        }

        private bool AddAllFiles(DirectoryInfo root, TreeNodeCollection nodes)
        {
            bool res = false;
            foreach (FileInfo file in root.GetFiles("*.*")
                .Where(n => !n.Name.ToLower().EndsWith("xml")
                            && !n.Name.ToLower().EndsWith("vshost.exe")
                            && !n.Name.ToLower().EndsWith("pdb")))
            {
                TreeNode nodeTmp = new TreeNode
                {
                    Text = $" {file.Name}",
                    Tag = file.FullName,
                    Checked = _projectSetting.SelectedFiles.Exists(n => n == file.FullName)
                };

                var extName = file.Extension.ToLower();
                if (!extList.ContainsKey(extName))
                {
                    extList.Add(extName, false);
                }
                if (_selectType == 2)
                {
                    nodeTmp.Checked = extList[extName];
                }
                if (file.Extension.ToLower() == ".config"
                    || file.Extension.ToLower() == ".manifest"
                    || file.Extension.ToLower() == ".asax"
                    || file.Extension.ToLower() == ".json")
                {
                    nodeTmp.ForeColor = Color.Red;
                }
                if (nodeTmp.Checked)
                {
                    res = true;
                }
                nodes.Add(nodeTmp);
            }

            return res;
        }

        private bool AddAllDirs(IEnumerable<DirectoryInfo> dirs, TreeNodeCollection nodes)
        {
            bool res = false;
            foreach (DirectoryInfo directory in dirs)
            {
                TreeNode nodeTmp = new TreeNode
                {
                    Text = $" {directory.Name}",
                    Tag = null,
                    ForeColor = Color.Goldenrod
                };
                bool isDirExist = AddAllDirs(directory.GetDirectories(), nodeTmp.Nodes);
                bool isFileExist = AddAllFiles(directory, nodeTmp.Nodes);
                nodeTmp.Checked = isDirExist || isFileExist;
                if (nodeTmp.Checked)
                {
                    res = true;
                }
                nodes.Add(nodeTmp);
            }

            return res;
        }

        private void SelectFilesForm_Shown(object sender, EventArgs e)
        {
            RefreshTreeView();
            foreach (var extension in extList)
            {
                var chk = new MetroCheckBox() { Text = extension.Key };
                chk.Checked = extension.Value;
                chk.CheckedChanged += (controlObj, args) =>
                {
                    if (_isChkAllChanging)
                    {
                        return;
                    }
                    var control = (MetroCheckBox)controlObj;
                    extList[control.Text] = control.Checked;
                    _isChildChkChanging = true;
                    this.ChkAll.Checked = extList.ContainsValue(true);
                    _isChildChkChanging = false;

                    _selectType = 2;
                    RefreshTreeView();
                };
                this.pannel_ChkList.Controls.Add(chk);
            }
        }

        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_isChildChkChanging)
            {
                return;
            }
            var chkAll = (MetroCheckBox)sender;
            _isChkAllChanging = true;
            foreach (MetroCheckBox chk in this.pannel_ChkList.Controls)
            {
                extList[chk.Text] = chkAll.Checked;
                chk.Checked = chkAll.Checked;
            }
            _isChkAllChanging = false;

            _selectType = 2;
            RefreshTreeView();
        }

        private void GetAllTreeNode(TreeNodeCollection nodes, List<string> paths)
        {
            foreach (TreeNode treeNode in nodes)
            {
                if (treeNode.Checked)
                {
                    if (treeNode.Tag != null)
                    {
                        paths.Add(treeNode.Tag.ToString());
                        if (treeNode.Tag.ToString().EndsWith("dll"))
                        {
                            string tmp = treeNode.Tag.ToString().Replace(".dll", ".pdb");
                            FileInfo file = new FileInfo(tmp);
                            if (file.Exists)
                            {
                                paths.Add(tmp);
                            }
                        }
                    }

                    GetAllTreeNode(treeNode.Nodes, paths);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            List<string> selectedFiles = new List<string>();
            GetAllTreeNode(tvFiles.Nodes, selectedFiles);
            if (!selectedFiles.Any())
            {
                MessageBox.Show("请选择要发布的文件");
                return;
            }
            FileSaveEvent?.Invoke(selectedFiles);
            this.Close();
        }
    }
}
