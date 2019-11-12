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
using TPublish.WinFormClient.Utils;

namespace TPublish.WinFormClient.WinForms
{
    public partial class SelectFilesForm : HZH_Controls.Forms.FrmWithOKCancel2
    {
        private List<string> _selectedFiels = new List<string>();
        private string _zipName = string.Empty;
        private string _basePath = string.Empty;

        public SelectFilesForm()
        {
            InitializeComponent();
        }

        public void Ini(string basePath, List<string> selectedFiles, string zipName)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                MessageBox.Show("请选择文件所在目录");
                return;
            }

            _selectedFiels = selectedFiles ?? new List<string>();
            _zipName = zipName;
            _basePath = basePath;

            RefreshTreeView();
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

    }
}
