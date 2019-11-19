using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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

        //private string _publishProjPath = string.Empty;
        //private string _publishProjName = string.Empty;

        public DeployForm(ProjectModel projectModel = null)
        {
            InitializeComponent();
            _projectModel = projectModel;
        }

        #region 线程中操作UI控件

        delegate void SetProcessCallback(int val);
        private void SetProcessVal(int val)
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
                this.buildProgressBar.Invoke(callback, new object[] { val });
            }
            else
            {
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

        #endregion

        #region Build

        /// <summary>
        /// NetFramework编译
        /// </summary>
        private bool Msbuild()
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
            buildArg += " /verbosity:minimal /p:Configuration=Debug /p:DeployOnBuild=true /p:Platform=AnyCPU /t:WebPublish /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=False /p:publishUrl=\"" + _publishFilesDir + "\"";
            SetProcessVal(2);

            var isSuccess = RunCmd(_settingInfo.MsBuildExePath, buildArg);
            return isSuccess;
        }

        /// <summary>
        /// NetCore编译
        /// </summary>
        private bool DotNetBuild()
        {
            string filePath = ProjectHelper.GetBuildToPath(_projectModel.ProjName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                LogAppend($"编译项目失败：未获取到文件生成路径");
                return false;
            }
            var _publishFilesDir = filePath.Replace("\\\\", "\\");
            if (_publishFilesDir.EndsWith("\\"))
            {
                _publishFilesDir = _publishFilesDir.Substring(0, _publishFilesDir.Length - 1);
            }
            ClearPublishFolder(_publishFilesDir);

            var buildArg = $"publish \"{_projectModel.ProjPath}\" -c Debug ";
            buildArg += " -o \"" + _publishFilesDir + "\"";
            SetProcessVal(2);

            var isSuccess = RunCmd("dotnet", buildArg);
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
        private bool RunCmd(string fileName, string arguments)
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
                    if (!string.IsNullOrWhiteSpace(args.Data))
                    {
                        if (args.Data.StartsWith(" "))
                        {
                            LogAppend(args.Data);
                        }
                        else if (args.Data.Contains(": warning"))
                        {
                            LogAppend($"!!!【warning】:{args.Data}", Color.BurlyWood);
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
                    process.Kill();
                }
                catch (Exception)
                {
                    //ignore
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
                                if (item.Type == 2)
                                {
                                    // c# 项目
                                    _projectModel = ProjectHelper.ParseProject(item.Path);
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
                                    _publishFilesDir = item.Path;
                                    SetStepIndex(2);
                                }
                            };
                            form.ShowDialog();
                        }

                        break;
                    case 2:
                        {
                            _projectSetting = ProjectHelper.LoadProjectSettingInfo(this._projectModel.ProjName);
                            SelectFilesForm filesForm = new SelectFilesForm(_projectSetting, _publishFilesDir);
                            filesForm.Activate();
                            SelectFilesForm.FileSaveEvent = list =>
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
                            };
                            filesForm.ShowDialog();
                        }
                        break;
                    case 3:
                        {
                            ServiceForm form = new ServiceForm(_projectModel, _projectSetting, _settingInfo);
                            form.Activate();
                            ServiceForm.ServiceSelectedEvent = (type, appId) =>
                            {
                                ControlHelper.ThreadRunExt(this, () =>
                                {
                                    LogAppend("开始上传文件");
                                    ApiHelper.UploadZipFile(_settingInfo, type, appId, _zipFilePath);

                                    ControlHelper.ThreadInvokerControl(this, () =>
                                    {
                                        SetProcessVal(100);
                                        LogAppend("文件部署完成");
                                    });
                                }, null, this);
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
                var isSuccess = false;
                SetProcessVal(1);
                // 判断项目类型
                if (_projectModel.IsNetCore)
                {
                    LogAppend($"开始编译DotNet项目:{this._projectModel.ProjName}", Color.FromArgb(0, 174, 219));
                    isSuccess = DotNetBuild();
                }
                else
                {
                    LogAppend($"开始编译NetFramework项目:{this._projectModel.ProjName}");
                    isSuccess = Msbuild();
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
            SettingForm settingForm = new SettingForm(_settingInfo);
            settingForm.Activate();
            settingForm.ShowDialog();
        }

        private void DeployForm_Shown(object sender, EventArgs e)
        {
            _settingInfo = SettingHelper.LoadSettingInfo();
            if (_settingInfo == null || _settingInfo.ApiIpAdress.IsNullOrEmpty() || _settingInfo.ApiKey.IsNullOrEmpty())
            {
                SettingForm settingForm = new SettingForm(_settingInfo);
                settingForm.Activate();
                settingForm.ShowDialog();
            }

            // 尝试连接服务器
            var isConnect = ApiHelper.Connect(_settingInfo);
            if (!isConnect)
            {
                MessageBox.Show("服务器连接失败，请检查服务器地址");
                return;
            }

            SetStepIndex(1);
        }
    }
}
