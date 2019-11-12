using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TPublish.Common.Model;
using TPublish.WinFormClient.Utils;

namespace TPublish.WinFormClient.WinForms
{
    public partial class DeployForm : Form
    {
        private ProjectModel _projectModel = null;

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
                        for (int i = 1; i <= 100; i++)
                        {
                            SetProcessVal(i);
                            LogAppend("hello" + i);
                            Thread.Sleep(100);
                        }
                    }
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }

        #region Build

        private void Msbuild()
        {
            SetProcessVal(1);
            if (string.IsNullOrWhiteSpace(_projectModel.MsBuildPath))
            {
                textLog.AppendText($"编译项目失败：未获取到MsBuild路径 {Environment.NewLine}");
                return;
            }
            string filePath = ProjectHelper.GetBuildToPath(_projectModel.ProjName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                textLog.AppendText($"编译项目失败：未获取到文件生成路径 {Environment.NewLine}");
                return;
            }
            var toPath = filePath.Replace("\\\\", "\\");
            if (toPath.EndsWith("\\"))
            {
                toPath = toPath.Substring(0, toPath.Length - 1);
            }
            ClearPublishFolder(toPath);
            var buildArg = "\"" + _projectModel.ProjPath.Replace("\\\\", "\\") + "\"";
            buildArg += " /verbosity:minimal /p:Configuration=Debug /p:DeployOnBuild=true /p:Platform=AnyCPU /t:WebPublish /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=False /p:publishUrl=\"" + toPath + "\"";
            SetProcessVal(2);

            var isSuccess = RunCmd(_projectModel.MsBuildPath, buildArg);
            if (!isSuccess)
            {
                SetProcessVal(0);
                textLog.AppendText($"编译项目失败 {Environment.NewLine}");
                return;
            }
            SetProcessVal(100);
        }

        private void DotNetBuild()
        {
            SetProcessVal(1);
            string filePath = ProjectHelper.GetBuildToPath(_projectModel.ProjName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                textLog.AppendText($"编译项目失败：未获取到文件生成路径 {Environment.NewLine}");
                return;
            }
            var toPath = filePath.Replace("\\\\", "\\");
            if (toPath.EndsWith("\\"))
            {
                toPath = toPath.Substring(0, toPath.Length - 1);
            }
            ClearPublishFolder(toPath);

            var buildArg = $"publish \"{_projectModel.ProjPath}\" -c Debug ";
            buildArg += " -o \"" + toPath + "\"";
            SetProcessVal(2);

            var isSuccess = RunCmd("dotnet", buildArg);
            if (!isSuccess)
            {
                SetProcessVal(0);
                textLog.AppendText($"编译项目失败 {Environment.NewLine}");
                return;
            }
            SetProcessVal(100);
        }

        /// <summary>
        /// 发布前清空
        /// </summary>
        /// <param name="srcDir">需要清空的目录</param>
        private static void ClearPublishFolder(string srcDir)
        {
            try
            {
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

        #endregion
    }
}
