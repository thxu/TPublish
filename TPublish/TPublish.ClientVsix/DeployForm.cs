using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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
        private List<DirSimpleName> _dirSimpleNames;
        private bool isIni = true;

        public DeployForm()
        {
            InitializeComponent();
        }
        public Result Ini(ProjModel projModel)
        {
            Result res = new Result();
            try
            {
                _projModel = projModel;
                _dirSimpleNames = ProjModel.ToDirSimpleNames(projModel.PublishDir);

                if (_projModel.ProjType == "Library")
                {
                    lbAppType.Text = "IIS";
                    _appViews = TPublishService.GetAllIISAppNames();
                }
                else
                {
                    lbAppType.Text = "Exe";
                    cbAppName.Enabled = false;
                    cbAppPublishDir.Enabled = false;
                    AppView view = TPublishService.GetExeAppView(_projModel.LibName);
                    if (string.IsNullOrWhiteSpace(view?.AppName))
                    {
                        throw new Exception("未找到该进程");
                    }
                    _appViews.Add(view);
                }
                cbAppName.DataSource = _appViews;
                cbAppName.DisplayMember = "AppName";
                cbAppName.ValueMember = "AppPhysicalPath";
                string lastChooseAppName = _projModel.LastChooseInfo.LastChooseAppName ?? _appViews[0].AppName;
                _projModel.LastChooseInfo.LastChooseAppName = lastChooseAppName;
                cbAppName.SelectedIndex = cbAppName.FindString(lastChooseAppName);
                showAppPath((cbAppName.SelectedItem as AppView)?.AppPhysicalPath ?? string.Empty);

                cbAppPublishDir.DataSource = _dirSimpleNames;
                cbAppPublishDir.DisplayMember = "Name";
                cbAppPublishDir.ValueMember = "FullName";
                string lastChoosePublishDir = _projModel.LastChooseInfo.LastChoosePublishDir ?? _dirSimpleNames[0].FullName;
                _projModel.LastChooseInfo.LastChoosePublishDir = lastChoosePublishDir;
                cbAppPublishDir.SelectedIndex = cbAppPublishDir.FindString(_dirSimpleNames.Count > 0 ? new DirectoryInfo(_projModel.LastChooseInfo.LastChoosePublishDir ?? _dirSimpleNames[0].FullName)
                        .Name
                    : string.Empty);

                lbChoosedFiles.Text = $"(已选择{_projModel?.LastChooseInfo?.LastChoosePublishFiles?.Where(n => !n.EndsWith("pdb"))?.Count() ?? 0}个文件)";

                isIni = false;
                res.IsSucceed = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
            }

            return res;
        }

        private void showAppPath(string path)
        {
            if (path.Length >= 50)
            {
                path = new string(path.Take(15).ToArray()) + "....." + new string(path.Skip(path.Length - 20).ToArray());
            }
            lbAppPath.Text = path;
            //int rowNum = 500;
            //float fontWidth = (float)lbAppPath.Width / lbAppPath.Text.Length;
            //int RowHeight = 15;
            //int colNum = (path.Length - (path.Length / rowNum) * rowNum) == 0 ? (path.Length / rowNum) : (path.Length / rowNum) + 1;
            //lbAppPath.AutoSize = false;
            //lbAppPath.Width = (int)(fontWidth * 10.0);
            //lbAppPath.Height = RowHeight * colNum;
        }

        private void cbAppName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (isIni)
            {
                return;
            }
            showAppPath((cbAppName.SelectedItem as AppView)?.AppPhysicalPath ?? string.Empty);
            _projModel.LastChooseInfo.LastChooseAppName = (cbAppName.SelectedItem as AppView)?.AppName;
        }

        private void btnDeploy_Click(object sender, System.EventArgs e)
        {
            try
            {
                string appName = (cbAppName.SelectedItem as AppView)?.AppName;
                if (string.IsNullOrWhiteSpace(appName))
                {
                    MessageBox.Show("请选择要发布的项目");
                    return;
                }

                string lastChooseSetting = Path.Combine(_projModel.LibDebugPath, "TPublish.setting");
                using (StreamWriter writer = File.CreateText(lastChooseSetting))
                {
                    writer.WriteLine(_projModel.LastChooseInfo.SerializeObject());
                    writer.Flush();
                }

                List<string> paths = new List<string>();
                //DirectoryInfo dir = new DirectoryInfo(_projModel.LibDebugPath);
                //if (radioFullPush.Checked)
                //{
                //    if (_projModel.ProjType == "Library")
                //    {
                //        dir = dir.Parent;
                //        // iis的全量，仅仅部署bin,content,view三个文件夹
                //        foreach (DirectoryInfo subDir in dir.GetDirectories())
                //        {
                //            if (subDir.Name.ToLower().Contains("bin")
                //                || subDir.Name.ToLower().Contains("content")
                //                || subDir.Name.ToLower().Contains("scripts")
                //                || subDir.Name.ToLower().Contains("view"))
                //            {
                //                paths.Add(subDir.FullName);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        // exe的全量，仅仅部署后缀为.dll .pdb .exe格式
                //        foreach (FileInfo fileInfo in dir.GetFiles("*.*").Where(n => !n.Name.ToLower().EndsWith("xml") && !n.Name.ToLower().EndsWith("pdb")))
                //        {
                //            paths.Add(fileInfo.FullName);
                //        }
                //        //foreach (FileInfo fileInfo in dir.GetFiles("*.pdb"))
                //        //{
                //        //    paths.Add(fileInfo.FullName);
                //        //}
                //        //foreach (FileInfo fileInfo in dir.GetFiles("*.exe"))
                //        //{
                //        //    paths.Add(fileInfo.FullName);
                //        //}
                //    }
                //}
                //else
                //{

                //}

                if (_projModel.LastChooseInfo.LastChoosePublishFiles == null || !_projModel.LastChooseInfo.LastChoosePublishFiles.Any())
                {
                    MessageBox.Show("请选择要部署的文件");
                }

                //var uploadRes = ZipAndUpload(_projModel.LastChooseInfo.LastChoosePublishFiles);
                //MessageBox.Show(uploadRes.IsSucceed ? "推送成功" : $"推送失败:{uploadRes.Message}");
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
                var zipRes = ZipHelper.ZipManyFilesOrDictorysAsync(paths, zipFullPath, pathTmp);
                return zipRes;
            }));

            NameValueCollection dic = new NameValueCollection();
            dic.Add("Type", _projModel.ProjType == "Library" ? "iis" : "exe");
            dic.Add("AppName", (cbAppName.SelectedItem as AppView)?.AppName);

            string url = $"{TPublishService.GetSettingPage().GetApiUrl()}/UploadZip";
            lbStatus.Text = "文件上传中。。。";
            string uploadResStr = HttpHelper.HttpPostData(url, 30000, _projModel.LibName + ".zip", zipFullPath, dic);
            var uploadRes = uploadResStr.DeserializeObject<Result>();
            lbStatus.Text = $"版本切换{(uploadRes.IsSucceed ? "成功" : "失败")} {uploadRes.Message}";
            return uploadRes;
        }

        private void radioPartPush_Click(object sender, EventArgs e)
        {
            PushFilesForm form = new PushFilesForm();
            form.Ini(_projModel.LastChooseInfo.LastChoosePublishDir, _projModel.LastChooseInfo.LastChoosePublishFiles);
            form.Show();

            PushFilesForm.FileSaveEvent = list =>
            {
                _projModel.LastChooseInfo.LastChoosePublishFiles = list;
                lbChoosedFiles.Text = $"(已选择{_projModel.LastChooseInfo.LastChoosePublishFiles.Count}个文件)";
            };
        }

        private void cbAppPublishDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isIni)
            {
                return;
            }
            _projModel.LastChooseInfo.LastChoosePublishDir = (cbAppPublishDir.SelectedItem as DirSimpleName)?.FullName;
        }

        private void linklbChooseFiles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PushFilesForm form = new PushFilesForm();
            form.Ini(_projModel.LastChooseInfo.LastChoosePublishDir, _projModel.LastChooseInfo.LastChoosePublishFiles);
            form.Show();

            PushFilesForm.FileSaveEvent = list =>
            {
                _projModel.LastChooseInfo.LastChoosePublishFiles = list;
                lbChoosedFiles.Text = $"(已选择{_projModel?.LastChooseInfo?.LastChoosePublishFiles?.Where(n => !n.EndsWith("pdb"))?.Count() ?? 0}个文件)";
            };
        }
    }
}
