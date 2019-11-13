using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZH_Controls.Controls;
using TPublish.WinFormClient.Utils;

namespace TPublish.WinFormClient.WinForms
{
    public partial class SelectFilesForm : HZH_Controls.Forms.FrmWithOKCancel2
    {
        private List<string> _selectedFiels = new List<string>();
        private Dictionary<string, bool> extList = new Dictionary<string, bool>();

        private string _zipName = string.Empty;
        private string _basePath = string.Empty;
        private bool _isChkAllChanging = false;
        private bool _isChildChkChanging = false;

        public SelectFilesForm()
        {
            InitializeComponent();
        }

        public void Ini(string basePath, List<string> selectedFiles, string zipName)
        {
            _basePath = basePath;
            _basePath = @"E:\Git-10.9\Go.WeiXinShop.UserService\Go.WeiXinShop.UserService\Go.WeiXinShop.UserService.Host.Exe\bin\Debug";
            if (string.IsNullOrWhiteSpace(_basePath))
            {
                MessageBox.Show("请选择文件所在目录");
                return;
            }

            _selectedFiels = selectedFiles ?? new List<string>();
            _zipName = zipName;

            RefreshTreeView();

            foreach (var extension in extList)
            {
                var chk = new UCCheckBox() { TextValue = extension.Key, Width = 100 };
                chk.Checked = extension.Value;
                chk.CheckedChangeEvent += (sender, args) =>
                {
                    if (_isChkAllChanging)
                    {
                        return;
                    }
                    var control = (UCCheckBox) sender;
                    extList[control.TextValue] = control.Checked;
                    _isChildChkChanging = true;
                    this.ucCheckBox_ChkAll.Checked = extList.ContainsValue(true);
                    _isChildChkChanging = false;

                    RefreshTreeView();
                };
                this.pannel_ChkList.Controls.Add(chk);
            }
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

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeViewCheck.CheckControl(e);
        }

        private bool AddAllFiles(DirectoryInfo root, TreeNodeCollection nodes)
        {
            bool res = false;
            foreach (FileInfo file in root.GetFiles("*.*")
                .Where(n => !n.Name.ToLower().EndsWith("xml")
                            && !n.Name.ToLower().EndsWith("vshost.exe")
                            && !n.Name.ToLower().EndsWith("pdb")
                            && !n.Name.Equals("TPublish.setting")
                            && !n.Name.Equals("Publish.setting")
                            && !n.Name.Equals(_zipName)))
            {
                TreeNode nodeTmp = new TreeNode
                {
                    Text = $" {file.Name}",
                    Tag = file.FullName,
                    Checked = _selectedFiels.Exists(n => n == file.FullName)
                };
                var extName = file.Extension.ToLower();
                if (!extList.ContainsKey(extName))
                {
                    extList.Add(extName, false);
                }
                if (file.Extension.ToLower() == ".config" || file.Extension.ToLower() == ".manifest" || file.Extension.ToLower() == ".asax")
                {
                    nodeTmp.ForeColor = Color.Red;
                }
                if (nodeTmp.Checked)
                {
                    res = true;
                }
                //if (!chk_ShowConfig.Checked && nodeTmp.ForeColor == Color.Red)
                //{
                //    // 当前节点是配置文件，但未勾选显示配置文件，不加载当前节点
                //    continue;
                //}
                nodes.Add(nodeTmp);
            }

            return res;
        }

        private bool AddAllDirs(IEnumerable<DirectoryInfo> dirs, TreeNodeCollection nodes)
        {
            bool res = false;
            foreach (DirectoryInfo directory in dirs)
            {
                if (directory.Name.ToLower().Contains("log"))
                {
                    continue;
                }
                TreeNode nodeTmp = new TreeNode
                {
                    Text = $" {directory.Name}",
                    Tag = null,
                    ForeColor = Color.Green
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

        private void ucCheckBox_ChkAll_CheckedChangeEvent(object sender, EventArgs e)
        {
            if (_isChildChkChanging)
            {
                return;
            }
            var chkAll = (UCCheckBox)sender;
            _isChkAllChanging = true;
            foreach (UCCheckBox chk in this.pannel_ChkList.Controls)
            {
                extList[chk.TextValue] = chkAll.Checked;
                chk.Checked = chkAll.Checked;
            }
            _isChkAllChanging = false;
        }
    }
}
