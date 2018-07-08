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
                    //cbAppPublishDir.Enabled = false;
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
                showLbText(lbAppPath, (cbAppName.SelectedItem as AppView)?.AppPhysicalPath ?? string.Empty);

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

        private void showLbText(Label lb, string text)
        {
            if (text.Length >= 50)
            {
                text = new string(text.Take(15).ToArray()) + "....." + new string(text.Skip(text.Length - 20).ToArray());
            }
            lb.Text = text;
        }

        private void cbAppName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (isIni)
            {
                return;
            }
            showLbText(lbAppPath, (cbAppName.SelectedItem as AppView)?.AppPhysicalPath ?? string.Empty);
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

                if (_projModel.LastChooseInfo.LastChoosePublishFiles == null || !_projModel.LastChooseInfo.LastChoosePublishFiles.Any())
                {
                    MessageBox.Show("请选择要部署的文件");
                }

                bwUploadZip.RunWorkerAsync();

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
            form.ShowDialog();

            PushFilesForm.FileSaveEvent = list =>
            {
                _projModel.LastChooseInfo.LastChoosePublishFiles = list;
                lbChoosedFiles.Text = $"(已选择{_projModel?.LastChooseInfo?.LastChoosePublishFiles?.Where(n => !n.EndsWith("pdb"))?.Count() ?? 0}个文件)";
            };
        }

        private void bwUploadZip_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string pathTmp = _projModel.LibDebugPath;
            if (_projModel.ProjType == "Library")
            {
                pathTmp = new DirectoryInfo(pathTmp).Parent?.FullName ?? pathTmp;
            }
            string zipFullPath = Path.Combine(pathTmp, _projModel.LibName + ".zip");

            if (bwUploadZip.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            bwUploadZip.ReportProgress(50, "压缩文件中...");

            ZipHelper.ZipManyFilesOrDictorys(_projModel.LastChooseInfo.LastChoosePublishFiles, zipFullPath, _projModel.LastChooseInfo.LastChoosePublishDir);

            NameValueCollection dic = new NameValueCollection();
            dic.Add("Type", _projModel.ProjType == "Library" ? "iis" : "exe");
            dic.Add("AppName", _projModel.LastChooseInfo.LastChooseAppName);

            if (bwUploadZip.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            bwUploadZip.ReportProgress(90, "文件上传中...");

            string url = $"{TPublishService.GetSettingPage().GetApiUrl()}/UploadZip";
            string uploadResStr = HttpHelper.HttpPostData(url, 30000, _projModel.LibName + ".zip", zipFullPath, dic);
            var uploadRes = uploadResStr.DeserializeObject<Result>();
            string msg = uploadRes.IsSucceed ? "部署成功" : "部署失败：" + uploadRes.Message;
            bwUploadZip.ReportProgress(100, msg);
        }

        private void bwUploadZip_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            pbUpload.Value = e.ProgressPercentage;
            showLbText(lbStatus, e.UserState.ToString());
        }

        private void bwUploadZip_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                showLbText(lbStatus, "任务取消");
            else if (e.Error != null)
                showLbText(lbStatus, "异常:" + e.Error);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bwUploadZip.CancelAsync();
        }
    }
}
