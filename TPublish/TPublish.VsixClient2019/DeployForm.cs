using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TPublish.VsixClient2019.Model;
using TPublish.VsixClient2019.Service;

namespace TPublish.VsixClient2019
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
                    _appViews = PublishService.GetAllIISAppNames();
                    if (_appViews == null || !_appViews.Any())
                    {
                        throw new Exception("未连接到可用的IIS服务");
                    }
                }
                else
                {
                    lbAppType.Text = "Exe";
                    _appViews = PublishService.GetExeAppView(_projModel.LibName);
                    if (_appViews == null || !_appViews.Any())
                    {
                        throw new Exception("未找到该进程");
                    }
                }
                cbAppName.DataSource = _appViews;
                cbAppName.DisplayMember = "AppAlias";
                cbAppName.ValueMember = "AppPhysicalPath";

                cbAppName.SelectedIndex = _appViews.FindIndex(n => n.Id == _projModel.LastChooseInfo.LastChooseAppName);

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
            toolTip1.InitialDelay = 300;
            toolTip1.SetToolTip(lb, text);
            if (text.Length >= 100)
            {
                text = new string(text.Take(15).ToArray()) + "....." + new string(text.Skip(20).ToArray());
            }
            if (text.Length >= 50)
            {
                //text = new string(text.Take(15).ToArray()) + "....." + new string(text.Skip(text.Length - 20).ToArray());
                text = new string(text.Take(50).ToArray()) + Environment.NewLine + new string(text.Skip(50).ToArray());
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
            _projModel.LastChooseInfo.LastChooseAppName = (cbAppName.SelectedItem as AppView)?.Id;
        }

        private void btnDeploy_Click(object sender, System.EventArgs e)
        {
            try
            {
                var view = cbAppName.SelectedItem as AppView;
                if (string.IsNullOrWhiteSpace(view?.AppAlias))
                {
                    MessageBox.Show("请选择要发布的项目");
                    return;
                }

                string lastChooseSetting = Path.Combine(_projModel.LibDebugPath, "Publish.setting");
                using (StreamWriter writer = File.CreateText(lastChooseSetting))
                {
                    writer.WriteLine(_projModel.LastChooseInfo.SerializeObject());
                    writer.Flush();
                }

                if (_projModel.LastChooseInfo.LastChoosePublishFiles == null || !_projModel.LastChooseInfo.LastChoosePublishFiles.Any())
                {
                    MessageBox.Show("请选择要部署的文件");
                    return;
                }

                bwUploadZip.RunWorkerAsync(view.Id);

                //var uploadRes = ZipAndUpload(_projModel.LastChooseInfo.LastChoosePublishFiles);
                //MessageBox.Show(uploadRes.IsSucceed ? "推送成功" : $"推送失败:{uploadRes.Message}");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
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
            form.Ini(_projModel.LastChooseInfo.LastChoosePublishDir, _projModel.LastChooseInfo.LastChoosePublishFiles, _projModel.LibName + ".zip");

            PushFilesForm.FileSaveEvent = list =>
            {
                _projModel.LastChooseInfo.LastChoosePublishFiles = list;
                lbChoosedFiles.Text = $"(已选择{_projModel?.LastChooseInfo?.LastChoosePublishFiles?.Where(n => !n.EndsWith("pdb"))?.Count() ?? 0}个文件)";
            };
            form.ShowDialog();
        }

        private void bwUploadZip_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var now = DateTime.Now;
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
            dic.Add("AppId", e.Argument.ToString());

            if (bwUploadZip.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            bwUploadZip.ReportProgress(90, "文件上传中...");

            string url = $"{PublishService.GetSettingPage().GetApiUrl()}/UploadZip";
            string uploadResStr = HttpHelper.HttpPostData(url, 30000, _projModel.LibName + ".zip", zipFullPath, dic);
            var uploadRes = uploadResStr.DeserializeObject<Result>();
            string msg = uploadRes.IsSucceed ? "部署成功" : "部署失败：" + uploadRes.Message;
            var timeSpan = (DateTime.Now - now).TotalMilliseconds;
            bwUploadZip.ReportProgress(100, msg + ",耗时：" + timeSpan);
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
