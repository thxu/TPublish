using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TPublish.ClientVsix.Service;

namespace TPublish.ClientVsix
{
    public partial class PushFilesForm : Form
    {
        public static Action<List<string>> FileSaveEvent;
        private List<string> SelectedFiels = new List<string>();

        public PushFilesForm()
        {
            InitializeComponent();
        }

        public void Ini(string basePath, List<string> selectedFiles)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                MessageBox.Show("请选择文件所在目录");
                return;
            }

            SelectedFiels = selectedFiles ?? new List<string>();
            DirectoryInfo root = new DirectoryInfo(basePath);

            TreeNode rootNode = new TreeNode { Text = "全选", Tag = null };
            tvPushFiles.Nodes.Add(rootNode);
            bool isDirExist = AddAllDirs(root.GetDirectories(), rootNode.Nodes);
            bool isFileExist = AddAllFiles(root, rootNode.Nodes);
            rootNode.Checked = isDirExist || isFileExist;
            rootNode.Expand();
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

        private bool AddAllFiles(DirectoryInfo root, TreeNodeCollection nodes)
        {
            bool res = false;
            foreach (FileInfo file in root.GetFiles("*.*").Where(n => !n.Name.ToLower().EndsWith("xml") && !n.Name.ToLower().EndsWith("pdb") && !n.Name.Equals("TPublish.setting")))
            {
                TreeNode nodeTmp = new TreeNode
                {
                    Text = file.Name,
                    Tag = file.FullName,
                    Checked = SelectedFiels.Exists(n => n == file.FullName)
                };
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
                if (directory.Name.ToLower().Contains("log"))
                {
                    continue;
                }
                TreeNode nodeTmp = new TreeNode
                {
                    Text = directory.Name,
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

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            List<string> selectedFiles = new List<string>();
            GetAllTreeNode(tvPushFiles.TopNode.Nodes, selectedFiles);
            if (!selectedFiles.Any())
            {
                MessageBox.Show("请选择要发布的文件");
                return;
            }
            FileSaveEvent?.Invoke(selectedFiles);
            this.Close();
        }

        private void tvPushFiles_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeViewCheck.CheckControl(e);
        }
    }
}
