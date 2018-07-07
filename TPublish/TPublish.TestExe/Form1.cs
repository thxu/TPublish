using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPublish.TestExe
{
    public partial class Form1 : Form
    {
        private int cnt = 90;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
            comboBox1.Items.Add("22222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222");
            comboBox1.Items.Add("33333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333");
            comboBox1.Items.Add("44444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444");
            comboBox1.Items.Add("55555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555");
            comboBox1.Items.Add("66666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666");


            string path = @"E:\IIS\GroundingResistance\1.0.0.6";
            DirectoryInfo root = new DirectoryInfo(path);

            AddAllDirs(root.GetDirectories(), treeView1.Nodes);
            AddAllFiles(root, treeView1.Nodes);
        }

        private void AddAllFiles(DirectoryInfo root, TreeNodeCollection nodes)
        {
            foreach (FileInfo file in root.GetFiles("*.*").Where(n => !n.Name.ToLower().EndsWith("xml") && !n.Name.ToLower().EndsWith("pdb")))
            {
                TreeNode nodeTmp = new TreeNode
                {
                    Text = file.Name,
                    Tag = file.FullName
                };
                nodes.Add(nodeTmp);
            }
        }

        private void AddAllDirs(IEnumerable<DirectoryInfo> dirs, TreeNodeCollection nodes)
        {
            foreach (DirectoryInfo directory in dirs)
            {
                TreeNode nodeTmp = new TreeNode
                {
                    Text = directory.Name,
                    Tag = null,
                };
                nodes.Add(nodeTmp);
                AddAllFiles(directory, nodeTmp.Nodes);
                AddAllDirs(directory.GetDirectories(), nodeTmp.Nodes);
            }
        }

        private void showAppPath(string path)
        {
            if (path.Length >= 30)
            {
                path = new string(path.Take(5).ToArray()) + "....." + new string(path.Skip(path.Length - 10).ToArray());
            }
            label1.Text = path;
            //int rowNum = 5000;
            //float fontWidth = (float)label1.Width / label1.Text.Length;
            //int RowHeight = 15;
            //int colNum = (path.Length - (path.Length / rowNum) * rowNum) == 0 ? (path.Length / rowNum) : (path.Length / rowNum) + 1;
            //label1.AutoSize = false;
            //label1.Width = (int)(fontWidth * 10.0);
            //label1.Height = RowHeight * colNum;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            showAppPath(comboBox1.Text);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var count = cnt;
            for (int i = 1; i <= count; i++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                backgroundWorker1.ReportProgress(i);
                Thread.Sleep(200);      // 模拟耗时的任务
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            textBox1.Text += DateTime.Now + Environment.NewLine;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                textBox1.Text += "任务取消。" + Environment.NewLine;
            else if (e.Error != null)
                textBox1.Text += "出现异常: " + e.Error + Environment.NewLine;
            else
                textBox1.Text += "任务完成。 " + Environment.NewLine;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = 100;
            backgroundWorker1.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //backgroundWorker1.CancelAsync();
            List<string> tmp = new List<string>();
            getAllTreeNode(treeView1.Nodes, tmp);
            var a = tmp.Count;
        }

        private void getAllTreeNode(TreeNodeCollection nodes, List<string> paths)
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

                    getAllTreeNode(treeNode.Nodes, paths);
                }
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeViewCheck.CheckControl(e);
        }
    }
}
