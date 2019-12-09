using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using TPublish.Common;
using TPublish.Common.Model;
using TPublish.WinFormClientApp.Controls;
using TPublish.WinFormClientApp.Model;
using TPublish.WinFormClientApp.Utils;

namespace TPublish.WinFormClientApp.WinForms
{
    public partial class DeployForm : MetroForm
    {
        private ProjectModel _projectModel = null;
        private string _publishFilesDir = null;
        private MProjectSettingInfo _projectSetting;
        private string _zipFilePath = null;
        private MSettingInfo _settingInfo = new MSettingInfo();

        private volatile bool stop_iis_cancel_token;

        private readonly string[] _deployTypesStr = { "runtime", "win-x86", "win-x64", "oxs-x64", "linux-x64", "win-arm", "linux-arm" };
        private bool _isUpdatingDeployType = false;

        public DeployForm(ProjectModel projectModel = null)
        {
            InitializeComponent();
            _projectModel = projectModel;
        }

        #region 线程中操作UI控件

        delegate void SetProcessCallback(int val, bool isUseMarqueeStyle);
        private void SetProcessVal(int val, bool isUseMarqueeStyle = false)
        {
            if (this.buildProgressBar.InvokeRequired)
            {
                while (!this.buildProgressBar.IsHandleCreated)
                {
                    if (this.buildProgressBar.Disposing || this.buildProgressBar.IsDisposed)
                    {
                        return;
                    }
                }
                SetProcessCallback callback = new SetProcessCallback(SetProcessVal);
                this.buildProgressBar.Invoke(callback, new object[] { val, isUseMarqueeStyle });
            }
            else
            {
                this.buildProgressBar.ProgressBarStyle = isUseMarqueeStyle ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
                this.buildProgressBar.Value = val;
            }
        }

        delegate void ProcessAutoIncrementCallback(int maxLimit);
        private void ProcessAutoIncrement(int maxLimit)
        {
            if (this.buildProgressBar.InvokeRequired)
            {
                while (!this.buildProgressBar.IsHandleCreated)
                {
                    if (this.buildProgressBar.Disposing || this.buildProgressBar.IsDisposed)
                    {
                        return;
                    }
                }
                ProcessAutoIncrementCallback callback = new ProcessAutoIncrementCallback(ProcessAutoIncrement);
                this.buildProgressBar.Invoke(callback, new object[] { maxLimit });
            }
            else
            {
                if (this.buildProgressBar.Value >= (int)maxLimit)
                {
                    return;
                }
                this.buildProgressBar.Value += 1;
            }
        }

        delegate void LogAppendCallback(string txt, Color? color);
        private void LogAppend(string txt, Color? color = null)
        {
            if (this.richTxtLog.InvokeRequired)
            {
                while (!this.richTxtLog.IsHandleCreated)
                {
                    if (this.richTxtLog.Disposing || this.richTxtLog.IsDisposed)
                    {
                        return;
                    }
                }

                LogAppendCallback callback = LogAppend;
                this.richTxtLog.Invoke(callback, txt, color);
            }
            else
            {
                richTxtLog.SelectionColor = color ?? Color.Black;
                this.richTxtLog.AppendText($"{txt}{Environment.NewLine}");
                this.richTxtLog.ScrollToCaret();
            }
        }

        delegate void SetStepIndexCallback(int stepIndex);
        private void SetStepIndex(int stepIndex)
        {
            if (this.deployStep.InvokeRequired)
            {
                while (!this.deployStep.IsHandleCreated)
                {
                    if (this.deployStep.Disposing || this.deployStep.IsDisposed)
                    {
                        return;
                    }
                }

                SetStepIndexCallback callback = SetStepIndex;
                this.deployStep.Invoke(callback, stepIndex);
            }
            else
            {
                this.deployStep.StepIndex = stepIndex;
            }
        }

        private void SetStepAsync(int stepIndex)
        {
            Task.Run((() =>
            {
                SetStepIndex(stepIndex);
            }));
        }

        delegate void SetStepsCallback(int stepIndex, string val);
        private void SetSteps(int index, string val)
        {
            if (this.deployStep.InvokeRequired)
            {
                while (!this.deployStep.IsHandleCreated)
                {
                    if (this.deployStep.Disposing || this.deployStep.IsDisposed)
                    {
                        return;
                    }
                }

                SetStepsCallback callback = SetSteps;
                this.deployStep.Invoke(callback, index, val);
            }
            else
            {
                this.deployStep.Steps[index] = val;
                this.deployStep.Refresh();
            }
        }

        delegate void SetDeployTypeVisibleCallback(bool visible);
        private void SetDeployTypeVisible(bool visible)
        {
            if (this.metroCbDeployType.InvokeRequired)
            {
                while (!this.metroCbDeployType.IsHandleCreated)
                {
                    if (this.metroCbDeployType.Disposing || this.metroCbDeployType.IsDisposed)
                    {
                        return;
                    }
                }

                SetDeployTypeVisibleCallback callback = SetDeployTypeVisible;
                this.metroCbDeployType.Invoke(callback, visible);
            }
            else
            {
                this.metroCbDeployType.Visible = visible;
                this.pictureBox1.Visible = visible;
            }
        }

        delegate void SetDeployTypeIndexCallback(int index);
        private void SetDeployTypeIndex(int index)
        {
            if (this.metroCbDeployType.InvokeRequired)
            {
                while (!this.metroCbDeployType.IsHandleCreated)
                {
                    if (this.metroCbDeployType.Disposing || this.metroCbDeployType.IsDisposed)
                    {
                        return;
                    }
                }

                SetDeployTypeIndexCallback callback = SetDeployTypeIndex;
                this.metroCbDeployType.Invoke(callback, index);
            }
            else
            {
                _isUpdatingDeployType = true;
                this.metroCbDeployType.SelectedIndex = index;
                _isUpdatingDeployType = false;
            }
        }

        #endregion

        #region Build

        /// <summary>
        /// NetFramework编译
        /// </summary>
        private bool Msbuild(Func<bool> checkCancel = null)
        {
            if (string.IsNullOrWhiteSpace(_settingInfo.MsBuildExePath))
            {
                LogAppend($"编译项目失败：未获取到MsBuild路径");
                return false;
            }
            string filePath = ProjectHelper.GetBuildToPath(_projectModel.ProjName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                LogAppend($"编译项目失败：未获取到文件生成路径");
                return false;
            }
            _publishFilesDir = filePath.Replace("\\\\", "\\");
            if (_publishFilesDir.EndsWith("\\"))
            {
                _publishFilesDir = _publishFilesDir.Substring(0, _publishFilesDir.Length - 1);
            }
            ClearPublishFolder(_publishFilesDir);
            var buildArg = "\"" + _projectModel.ProjPath.Replace("\\\\", "\\") + "\"";
            if (_projectModel.IsExe())
            {
                buildArg += " /verbosity:minimal /p:Configuration=Debug /p:DeployOnBuild=true /p:Platform=AnyCPU /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=False /p:OutputPath=\"" + _publishFilesDir + "\"";
            }
            else
            {
                buildArg += " /verbosity:minimal /p:Configuration=Debug /p:DeployOnBuild=true /p:Platform=AnyCPU /t:WebPublish /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=False /p:publishUrl=\"" + _publishFilesDir + "\"";
            }

            SetProcessVal(2);

            var isSuccess = RunCmd(_settingInfo.MsBuildExePath, buildArg, checkCancel);
            return isSuccess;
        }

        /// <summary>
        /// NetCore编译
        /// </summary>
        private bool DotNetBuild(Func<bool> checkCancel = null)
        {
            string filePath = ProjectHelper.GetBuildToPath(_projectModel.ProjName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                LogAppend($"编译项目失败：未获取到文件生成路径");
                return false;
            }
            _publishFilesDir = filePath.Replace("\\\\", "\\");
            if (_publishFilesDir.EndsWith("\\"))
            {
                _publishFilesDir = _publishFilesDir.Substring(0, _publishFilesDir.Length - 1);
            }
            ClearPublishFolder(_publishFilesDir);

            var buildArg = $"publish \"{_projectModel.ProjPath}\" -c Debug {((_projectSetting.DeployType.IsNullOrEmpty() || _projectSetting.DeployType == "runtime") ? " --no-build " : " --runtime " + _projectSetting.DeployType)}  ";
            buildArg += " -o \"" + _publishFilesDir + "\"";
            SetProcessVal(2);

            var isSuccess = RunCmd("dotnet", buildArg, checkCancel);
            return isSuccess;
        }

        /// <summary>
        /// 发布前清空
        /// </summary>
        /// <param name="srcDir">需要清空的目录</param>
        private void ClearPublishFolder(string srcDir)
        {
            try
            {
                LogAppend($"开始清空发布目录...");
                DirectoryInfo dir = new DirectoryInfo(srcDir);

                if (!dir.Exists) return;

                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }

                var folderList = dir.GetDirectories();

                foreach (var folder in folderList)
                {
                    if (folder.Name.EndsWith(".git")) continue;
                    try
                    {
                        folder.Delete(true);
                    }
                    catch (Exception)
                    {

                    }
                }
                LogAppend($"清空发布目录完成");
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "清空发布目录异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 调用cmd进行编译
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private bool RunCmd(string fileName, string arguments, Func<bool> checkCancel = null)
        {
            Process process = null;
            try
            {
                process = new Process();

                process.StartInfo.WorkingDirectory = string.Empty;
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.Verb = "runas";
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.Start();


                process.OutputDataReceived += (sender, args) =>
                {
                    if (checkCancel != null)
                    {
                        var r = checkCancel();
                        if (r)
                        {
                            LogAppend($"编译取消", Color.Red);
                            try
                            {
                                process?.Dispose();
                            }
                            catch (Exception)
                            {
                                //ignore
                            }

                            try
                            {
                                process?.Kill();
                            }
                            catch (Exception)
                            {
                                //ignore
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(args.Data))
                    {
                        if (args.Data.StartsWith(" "))
                        {
                            LogAppend(args.Data);
                        }
                        else if (args.Data.Contains(": warning"))
                        {
                            LogAppend($"【warning】:{args.Data}", Color.BurlyWood);
                        }
                        else if (args.Data.Contains(": error"))
                        {
                            LogAppend($"【error】:{args.Data}", Color.Red);
                        }
                        else
                        {
                            LogAppend($"{args.Data}");
                        }
                        ProcessAutoIncrement(98);
                    }
                };
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (sender, data) =>
                {
                    if (checkCancel != null)
                    {
                        var r = checkCancel();
                        if (r)
                        {
                            LogAppend($"编译取消", Color.Red);
                            try
                            {
                                process?.Dispose();
                            }
                            catch (Exception)
                            {
                                //ignore
                            }

                            try
                            {
                                process?.Kill();
                            }
                            catch (Exception)
                            {
                                //ignore
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(data.Data))
                    {
                        LogAppend($"[error]:{data.Data}");
                    }
                };
                process.BeginErrorReadLine();

                process.WaitForExit();
                SetProcessVal(99);
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
                catch (Exception)
                {

                }
                return process.ExitCode == 0;
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "编译项目异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                try
                {
                    process?.Kill();
                }
                catch (Exception)
                {
                    //ignore
                }
                try
                {
                    process?.Dispose();
                }
                catch (Exception)
                {
                    //ignore
                }

            }
        }

        #endregion

        private void DeployStepOnIndexChecked(object sender, EventArgs e)
        {
            try
            {
                StepControl control = (StepControl)sender;
                switch (control.StepIndex)
                {
                    case 1:
                        SetProcessVal(0);
                        if (_projectModel?.ProjType == 1)
                        {
                            _projectSetting = ProjectHelper.LoadProjectSettingInfo(this._projectModel.ProjName);
                            if (_projectModel.IsNetCore)
                            {
                                this.metroCbDeployType.Visible = true;
                                _isUpdatingDeployType = true;
                                for (int i = 0; i < _deployTypesStr.Length; i++)
                                {
                                    if (_deployTypesStr[i] == _projectSetting.DeployType)
                                    {
                                        this.metroCbDeployType.SelectedIndex = i;
                                    }
                                }

                                if (this.metroCbDeployType.SelectedIndex < 0)
                                {
                                    this.metroCbDeployType.SelectedIndex = 0;
                                }
                                _isUpdatingDeployType = false;
                            }
                            else
                            {
                                this.metroCbDeployType.Visible = false;
                            }
                            ControlHelper.ThreadRunExt(this, () =>
                            {
                                bool isBuildSuccess = BuildProj();
                                if (!isBuildSuccess)
                                {
                                    return;
                                }
                                SetProcessVal(100);
                                SetStepIndex(2);

                            }, null, this);
                        }
                        else
                        {
                            // 打开选择窗口，让人选择项目或者文件夹
                            SelectProjectForm form = new SelectProjectForm(_settingInfo);
                            form.Activate();
                            SelectProjectForm.ProjSelectedEvent = (item) =>
                            {
                                try
                                {
                                    if (item.Type == 2)
                                    {
                                        // c# 项目
                                        _projectModel = ProjectHelper.ParseProject(item.Path);
                                        _projectSetting = ProjectHelper.LoadProjectSettingInfo(this._projectModel.ProjName);
                                        if (_projectModel.IsNetCore)
                                        {
                                            SetDeployTypeVisible(true);
                                            bool find = false;
                                            for (int i = 0; i < _deployTypesStr.Length; i++)
                                            {
                                                if (_deployTypesStr[i] == _projectSetting.DeployType)
                                                {
                                                    find = true;
                                                    SetDeployTypeIndex(i);
                                                }
                                            }

                                            if (!find)
                                            {
                                                SetDeployTypeIndex(0);
                                            }
                                        }
                                        else
                                        {
                                            SetDeployTypeVisible(false);
                                        }

                                        bool isBuildSuccess = BuildProj();
                                        SetProcessVal(100);
                                        if (!isBuildSuccess)
                                        {
                                            return;
                                        }
                                        SetStepIndex(2);
                                    }
                                    else
                                    {
                                        _projectModel = new ProjectModel()
                                        {
                                            Key = Guid.NewGuid().ToString(),
                                            OutPutType = string.Empty,
                                            ProjName = $"{item.Name}{item.Guid}",
                                            ProjPath = item.Path,
                                            ProjType = item.Type,
                                        };
                                        _projectSetting = ProjectHelper.LoadProjectSettingInfo(this._projectModel.ProjName);
                                        SetDeployTypeVisible(false);
                                        _publishFilesDir = item.Path;
                                        SetStepIndex(2);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MetroMessageBox.Show(this, ex.Message, "项目选择处理错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            };
                            form.ShowDialog();
                        }

                        break;
                    case 2:
                        {
                            if (_publishFilesDir.IsNullOrEmpty())
                            {
                                MetroMessageBox.Show(this, "请先选择文件目录或者项目文件", "无法打开文件选择窗口", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                SetStepAsync(1);
                                return;
                            }
                            //_projectSetting = ProjectHelper.LoadProjectSettingInfo(this._projectModel.ProjName);
                            SelectFilesForm filesForm = new SelectFilesForm(_projectSetting, _publishFilesDir, _settingInfo);
                            filesForm.Activate();
                            SelectFilesForm.FileSaveEvent = list =>
                            {
                                try
                                {
                                    // 打开发布服务器窗口
                                    _projectSetting.SelectedFiles = list;
                                    ProjectHelper.SaveProjectSettingInfo(_projectSetting);
                                    SetSteps(1, $"(已选择{list?.Where(n => !n.EndsWith("pdb"))?.Count() ?? 0}个文件)");
                                    SetProcessVal(0);
                                    // 打包文件
                                    LogAppend("开始压缩选中的文件");
                                    _zipFilePath = ProjectHelper.GetZipPath(_projectModel.ProjName);
                                    ZipHelper.BatchZip(list, _zipFilePath, _publishFilesDir, (progressValue) =>
                                    {
                                        SetProcessVal(progressValue);
                                        return false;
                                    });
                                    SetProcessVal(100);
                                    LogAppend("文件压缩完成");
                                    SetStepIndex(3);
                                }
                                catch (Exception ex)
                                {
                                    MetroMessageBox.Show(this, ex.Message, "文件处理错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    _zipFilePath = string.Empty;
                                }
                            };
                            filesForm.ShowDialog();
                        }
                        break;
                    case 3:
                        {
                            if (_projectSetting == null)
                            {
                                MetroMessageBox.Show(this, "请选择文件目录或者项目文件", "无法打开发布到服务器窗口", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                SetStepAsync(1);
                                return;
                            }

                            if (_zipFilePath.IsNullOrEmpty())
                            {
                                MetroMessageBox.Show(this, "重新选择需要发布的文件进行压缩", "未找到压缩文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                SetStepAsync(2);
                                return;
                            }
                            ServiceForm form = new ServiceForm(_projectModel, _projectSetting, _settingInfo);
                            form.Activate();
                            ServiceForm.ServiceSelectedEvent = (type, appId) =>
                            {
                                try
                                {
                                    SetProcessVal(20, true);
                                    LogAppend("开始上传文件");
                                    ApiHelper.UploadZipFile(_settingInfo, type, appId, _zipFilePath);
                                    SetProcessVal(100);
                                    LogAppend("文件部署完成");
                                }
                                catch (Exception ex)
                                {
                                    MetroMessageBox.Show(this, ex.Message, "发布到服务器错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            };
                            form.ShowDialog();
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                //TxtLogService.WriteLog(exception, SettingHelper.GetLogDirPath(), "步骤执行失败");
                LogAppend($"步骤执行失败：{exception.Message}", Color.Red);
                MetroMessageBox.Show(this, exception.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 编译项目
        /// </summary>
        /// <returns></returns>
        public bool BuildProj()
        {
            try
            {
                this.stop_iis_cancel_token = false;
                var isSuccess = false;
                SetProcessVal(1);
                // 判断项目类型
                if (_projectModel.IsNetCore)
                {
                    LogAppend($"开始编译DotNet项目:{this._projectModel.ProjName}", Color.FromArgb(0, 174, 219));
                    isSuccess = DotNetBuild(() => stop_iis_cancel_token);
                }
                else
                {
                    LogAppend($"开始编译NetFramework项目:{this._projectModel.ProjName}");
                    isSuccess = Msbuild(() => stop_iis_cancel_token);
                }

                if (!isSuccess)
                {
                    SetProcessVal(0);
                    LogAppend($"编译项目失败");
                    return false;
                }
                LogAppend($"编译项目成功");
                SetProcessVal(100);
                return true;
            }
            catch (Exception e)
            {
                LogAppend($"编译项目失败：{e.Message}", Color.Red);
                MetroMessageBox.Show(this, e.Message, "编译项目失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 打开设置页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkSetting_Click(object sender, EventArgs e)
        {
            var settingBackUp = _settingInfo.DeepCopy();
            SettingForm settingForm = new SettingForm(_settingInfo);
            settingForm.Activate();
            var dlgRes = settingForm.ShowDialog();
            if (dlgRes != DialogResult.OK)
            {
                _settingInfo = settingBackUp;
            }

            if (_settingInfo == null)
            {
                _settingInfo = new MSettingInfo();
            }
            this.metroStyleManager1.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            if (_settingInfo.MetroColorStyle < 0 || _settingInfo.MetroColorStyle >= 15)
            {
                _settingInfo.MetroColorStyle = MetroColorStyle.Blue.GetHashCode();
            }
            this.metroStyleManager1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.StyleManager = this.metroStyleManager1;
            this.deployStep.StyleManager = this.metroStyleManager1;
            this.linkSetting.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.linkSetting.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            this.buildProgressBar.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.buildProgressBar.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            this.metroToolTip1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.metroToolTip1.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            this.metroCbDeployType.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.metroCbDeployType.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            this.Refresh();
        }

        private void DeployForm_Shown(object sender, EventArgs e)
        {
            _settingInfo = SettingHelper.LoadSettingInfo();
            if (_settingInfo?.GetCurrServiceInfo() == null)
            {
                var settingBackUp = _settingInfo.DeepCopy();
                SettingForm settingForm = new SettingForm(_settingInfo);
                settingForm.Activate();
                var dlgRes = settingForm.ShowDialog();
                if (dlgRes != DialogResult.OK)
                {
                    _settingInfo = settingBackUp;
                }
            }

            if (_settingInfo == null)
            {
                _settingInfo = new MSettingInfo() { SelectedItems = new List<MSelectedItem>(), ServiceInfos = new List<ServiceInfo>() };
            }
            this.metroStyleManager1.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            if (_settingInfo.MetroColorStyle < 0 || _settingInfo.MetroColorStyle >= 15)
            {
                _settingInfo.MetroColorStyle = MetroColorStyle.Blue.GetHashCode();
            }
            this.metroStyleManager1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.StyleManager = this.metroStyleManager1;
            this.deployStep.StyleManager = this.metroStyleManager1;
            this.linkSetting.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.linkSetting.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            this.buildProgressBar.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.buildProgressBar.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            this.metroToolTip1.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.metroToolTip1.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            this.metroCbDeployType.Style = (MetroColorStyle)_settingInfo.MetroColorStyle;
            this.metroCbDeployType.Theme = _settingInfo.MetroThemeStyle <= 1 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            this.Refresh();


            // 尝试连接服务器
            var isConnect = ApiHelper.Connect(_settingInfo);
            if (!isConnect)
            {
                MetroMessageBox.Show(this, "默认服务器连接失败，请检查服务器地址或者选择其他服务器进行部署", "无法连接到服务器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            SetStepIndex(1);
        }

        private void metroCbDeployType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdatingDeployType)
            {
                return;
            }

            this.stop_iis_cancel_token = true;
            _projectSetting.DeployType = _deployTypesStr[this.metroCbDeployType.SelectedIndex];
            ProjectHelper.SaveProjectSettingInfo(_projectSetting);
            ControlHelper.ThreadRunExt(this, () =>
            {
                bool isBuildSuccess = BuildProj();
                if (!isBuildSuccess)
                {
                    return;
                }
                SetProcessVal(100);
                SetStepIndex(2);

            }, null, this);
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTxtLog.Clear();
        }

        private void richTxtLog_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                metroContextMenu1.Show(MousePosition.X, MousePosition.Y);
            }
        }
    }
}
