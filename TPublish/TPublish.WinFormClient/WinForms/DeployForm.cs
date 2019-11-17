using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HZH_Controls;
using HZH_Controls.Forms;
using TPublish.Common;
using TPublish.Common.Model;
using TPublish.WinFormClient.Utils;

namespace TPublish.WinFormClient.WinForms
{
    public partial class DeployForm : Form
    {
        private ProjectModel _projectModel = null;
        private string _publishFilesDir = null;
        private List<string> _selectedFileList = new List<string>();

        public DeployForm(ProjectModel projectModel = null)
        {
            InitializeComponent();
            _projectModel = projectModel;
        }

        private void ucStep_IndexChecked(object sender, EventArgs e)
        {
            switch (this.ucStep.StepIndex)
            {
                case 1:
                    {
                        SetProcessVal(0);

                        ControlHelper.ThreadRunExt(this, () =>
                        {
                            BuildProj();
                            ControlHelper.ThreadInvokerControl(this, () =>
                            {
                                SetProcessVal(100);
                                SetStep(2);
                            });
                        }, null, this, false);
                    }
                    break;
                case 2:
                    {
                        // 弹出选择文件窗口
                        var fileForm = new SelectFilesForm();
                        fileForm.Ini(_publishFilesDir, _selectedFileList, null);
                        SelectFilesForm.FileSaveEvent = list =>
                        {
                            _selectedFileList = list;
                            this.ucStep.Steps[1] = $"(已选择{list?.Where(n => !n.EndsWith("pdb"))?.Count() ?? 0}个文件)";
                            this.ucStep.Refresh();
                            SetProcessVal(0);

                            // 打包文件
                            ControlHelper.ThreadRunExt(this, () =>
                            {
                                LogAppend("开始压缩选中的文件");
                                var zipPath = ProjectHelper.GetZipPath(_projectModel.ProjName);
                                ZipHelper.BatchZip(list, zipPath, _publishFilesDir, (progressValue) =>
                                {
                                    SetProcessVal(progressValue);
                                    return false;
                                });

                                ControlHelper.ThreadInvokerControl(this, () =>
                                {
                                    SetProcessVal(100);
                                    LogAppend("文件压缩完成");
                                    SetStep(3);
                                });
                            }, null, this, false);
                        };

                        var res = fileForm.ShowDialog();
                    }
                    break;
                case 3:
                    {
                        // 弹出选择服务器窗口
                        var serviceForm = new ServiceForm();
                        serviceForm.ShowDialog();
                    }
                    break;
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
                // 判断项目类型
                if (_projectModel.IsNetCore())
                {
                    LogAppend($"开始编译DotNet项目");
                    DotNetBuild();
                }
                else
                {
                    LogAppend($"开始编译NetFramework项目");
                    Msbuild();
                }
                return true;
            }
            catch (Exception e)
            {
                LogAppend($"编译项目失败：{e.Message}");
                return false;
            }
        }

        #region Build

        /// <summary>
        /// NetFramework编译
        /// </summary>
        private void Msbuild()
        {
            SetProcessVal(1);
            //if (string.IsNullOrWhiteSpace(_projectModel.MsBuildPath))
            //{
            //    LogAppend($"编译项目失败：未获取到MsBuild路径");
            //    return;
            //}
            string filePath = ProjectHelper.GetBuildToPath(_projectModel.ProjName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                LogAppend($"编译项目失败：未获取到文件生成路径");
                return;
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

            //var isSuccess = RunCmd(_projectModel.MsBuildPath, buildArg);
            //if (!isSuccess)
            //{
            //    SetProcessVal(0);
            //    LogAppend($"编译项目失败");
            //    return;
            //}
            SetProcessVal(100);
        }

        /// <summary>
        /// NetCore编译
        /// </summary>
        private void DotNetBuild()
        {
            SetProcessVal(1);
            string filePath = ProjectHelper.GetBuildToPath(_projectModel.ProjName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                LogAppend($"编译项目失败：未获取到文件生成路径");
                return;
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
            if (!isSuccess)
            {
                SetProcessVal(0);
                LogAppend($"编译项目失败");
                return;
            }
            SetProcessVal(100);
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
            catch (Exception)
            {

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
                            LogAppend($"[warning]:{args.Data}");
                        }
                        else if (args.Data.Contains(": error"))
                        {
                            LogAppend($"[error]:{args.Data}");
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
                LogAppend("编译完成");
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

        #region 线程中操作UI控件

        delegate void SetProcessCallback(int val);
        private void SetProcessVal(int val)
        {
            if (this.ucProcessLine.InvokeRequired)
            {
                while (!this.ucProcessLine.IsHandleCreated)
                {
                    if (this.ucProcessLine.Disposing || this.ucProcessLine.IsDisposed)
                    {
                        return;
                    }
                }
                SetProcessCallback callback = new SetProcessCallback(SetProcessVal);
                this.ucProcessLine.Invoke(callback, new object[] { val });
            }
            else
            {
                this.ucProcessLine.Value = val;
            }
        }

        delegate void ProcessAutoIncrementCallback(int maxLimit);
        private void ProcessAutoIncrement(int maxLimit)
        {
            if (this.ucProcessLine.InvokeRequired)
            {
                while (!this.ucProcessLine.IsHandleCreated)
                {
                    if (this.ucProcessLine.Disposing || this.ucProcessLine.IsDisposed)
                    {
                        return;
                    }
                }
                ProcessAutoIncrementCallback callback = new ProcessAutoIncrementCallback(ProcessAutoIncrement);
                this.ucProcessLine.Invoke(callback, new object[] { maxLimit });
            }
            else
            {
                if (this.ucProcessLine.Value >= (int)maxLimit)
                {
                    return;
                }
                this.ucProcessLine.Value += 1;
            }
        }

        delegate void LogAppendCallback(string txt);
        private void LogAppend(string txt)
        {
            if (this.textLog.InvokeRequired)
            {
                while (!this.textLog.IsHandleCreated)
                {
                    if (this.textLog.Disposing || this.textLog.IsDisposed)
                    {
                        return;
                    }
                }

                Action<string> actionDelegate = (x) => { this.textLog.AppendText($"{txt}{Environment.NewLine}"); };
                this.textLog.Invoke(actionDelegate, txt);

                //LogAppendCallback callback = new LogAppendCallback(LogAppend);
                //this.textLog.Invoke(callback, new object[] { txt });
            }
            else
            {
                this.textLog.AppendText($"{txt}{Environment.NewLine}");
            }
        }

        private void SetStep(int stepIndex)
        {
            if (this.ucStep.InvokeRequired)
            {
                while (!this.ucStep.IsHandleCreated)
                {
                    if (this.ucStep.Disposing || this.ucStep.IsDisposed)
                    {
                        return;
                    }
                }

                Action<string> actionDelegate = (x) => { this.ucStep.StepIndex = stepIndex; };
                this.ucStep.Invoke(actionDelegate, stepIndex);

                //LogAppendCallback callback = new LogAppendCallback(LogAppend);
                //this.textLog.Invoke(callback, new object[] { txt });
            }
            else
            {
                this.ucStep.StepIndex = stepIndex;
            }
        }

        #endregion
    }
}
