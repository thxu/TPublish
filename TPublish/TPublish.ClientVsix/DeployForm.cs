using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using TPublish.ClientVsix.Model;
using TPublish.ClientVsix.Service;

namespace TPublish.ClientVsix
{
    public partial class DeployForm : Form
    {
        protected ProjModel _projModel;
        private List<AppView> _appViews = new List<AppView>();

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
                _appViews.Add(view);
                cbAppName.Items.Add(view?.AppName ?? string.Empty);
                //lbAppPath.Text = view?.AppPhysicalPath ?? string.Empty;
                showAppPath(view?.AppPhysicalPath ?? string.Empty);
            }
            cbAppName.SelectedIndex = -1;
        }

        private void showAppPath(string path)
        {
            lbAppPath.Text = path;
            int rowNum = 500;
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

                List<string> paths = new List<string>();
                DirectoryInfo dir = new DirectoryInfo(_projModel.LibDebugPath);
                if (radioFullPush.Checked)
                {
                    if (_projModel.ProjType == "Library")
                    {
                        dir = dir.Parent;
                        // iis的全量，仅仅部署bin,content,view三个文件夹
                        foreach (DirectoryInfo subDir in dir.GetDirectories())
                        {
                            if (subDir.Name.ToLower().Contains("bin")
                                || subDir.Name.ToLower().Contains("content")
                                || subDir.Name.ToLower().Contains("scripts")
                                || subDir.Name.ToLower().Contains("view"))
                            {
                                paths.Add(subDir.FullName);
                            }
                        }
                    }
                    else
                    {
                        // exe的全量，仅仅部署后缀为.dll .pdb .exe格式
                        foreach (FileInfo fileInfo in dir.GetFiles("*.dll"))
                        {
                            paths.Add(fileInfo.FullName);
                        }
                        foreach (FileInfo fileInfo in dir.GetFiles("*.pdb"))
                        {
                            paths.Add(fileInfo.FullName);
                        }
                        foreach (FileInfo fileInfo in dir.GetFiles("*.exe"))
                        {
                            paths.Add(fileInfo.FullName);
                        }
                    }
                }
                else
                {

                }

                var uploadRes = ZipAndUpload(paths);
                MessageBox.Show(uploadRes.IsSucceed ? "推送成功" : $"推送失败:{uploadRes.Message}");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private Result ZipAndUpload(List<string> paths)
        {
            string pathTmp = _projModel.LibDebugPath;
            if (_projModel.ProjType == "Library")
            {
                pathTmp = new DirectoryInfo(pathTmp).Parent?.FullName ?? pathTmp;
            }
            string zipFullPath = Path.Combine(pathTmp, _projModel.LibName + ".zip");
            lbStatus.Text = "压缩文件中...";
            var tmp = ThreadHelper.JoinableTaskFactory.Run((() =>
            {
                var zipRes = ZipHelper.ZipManyFilesOrDictorysAsync(paths, zipFullPath, _projModel.LibDebugPath);
                return zipRes;
            }));

            NameValueCollection dic = new NameValueCollection();
            dic.Add("Type", _projModel.ProjType == "Library" ? "iis" : "exe");
            dic.Add("AppName", cbAppName.SelectedItem.ToString());

            string url = $"{TPublishService.GetSettingPage().GetApiUrl()}/UploadZip";
            lbStatus.Text = "文件上传中。。。";
            string uploadResStr = HttpHelper.HttpPostData(url, 30000, _projModel.LibName + ".zip", zipFullPath, dic);
            var uploadRes = uploadResStr.DeserializeObject<Result>();
            lbStatus.Text = $"版本切换{(uploadRes.IsSucceed ? "成功" : "失败")} {uploadRes.Message}";
            return uploadRes;
        }
    }
}
