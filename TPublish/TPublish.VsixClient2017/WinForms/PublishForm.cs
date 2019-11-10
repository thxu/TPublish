using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TPublish.VsixClient2017.Model;

namespace TPublish.VsixClient2017.WinForms
{
    public partial class PublishForm : Form
    {
        /// <summary>
        /// UI线程的同步上下文
        /// </summary>
        private SynchronizationContext _syncContext = null;
        protected ProjModel _projModel;

        private void LogAppend1(object obj)
        {
            txtLog.AppendText((string)obj + Environment.NewLine);
        }
        private void SetProcessVal(object val)
        {
            this.ucProcessLine1.Value = (int)val;
        }

        private void ProcessUp(object maxVal)
        {
            if (this.ucProcessLine1.Value >= (int)maxVal)
            {
                return;
            }
            this.ucProcessLine1.Value += 1;
        }

        delegate void SetProcessCallback(int val);
        private void SetProcess(int val)
        {
            if (this.ucProcessLine1.InvokeRequired)
            {
                while (!this.ucProcessLine1.IsHandleCreated)
                {
                    if (this.ucProcessLine1.Disposing || this.ucProcessLine1.IsDisposed)
                    {
                        return;
                    }
                }
                SetProcessCallback callback = new SetProcessCallback(SetProcess);
                this.ucProcessLine1.Invoke(callback, new object[] { val });
            }
            else
            {
                this.ucProcessLine1.Value = val;
            }
        }

        delegate void ProcessAutoIncrementCallback(int maxLimit);
        private void ProcessAutoIncrement(int maxLimit)
        {
            if (this.ucProcessLine1.InvokeRequired)
            {
                while (!this.ucProcessLine1.IsHandleCreated)
                {
                    if (this.ucProcessLine1.Disposing || this.ucProcessLine1.IsDisposed)
                    {
                        return;
                    }
                }
                ProcessAutoIncrementCallback callback = new ProcessAutoIncrementCallback(SetProcess);
                this.ucProcessLine1.Invoke(callback, new object[] { maxLimit });
            }
            else
            {
                if (this.ucProcessLine1.Value >= (int)maxLimit)
                {
                    return;
                }
                this.ucProcessLine1.Value += 1;
            }
        }

        delegate void LogAppendCallback(string txt);
        private void LogAppend(string txt)
        {
            if (this.txtLog.InvokeRequired)
            {
                while (!this.txtLog.IsHandleCreated)
                {
                    if (this.txtLog.Disposing || this.txtLog.IsDisposed)
                    {
                        return;
                    }
                }

                Action<string> actionDelegate = (x) => { this.txtLog.AppendText($"{txt}{Environment.NewLine}"); };
                this.txtLog.Invoke(actionDelegate, txt);

                //LogAppendCallback callback = new LogAppendCallback(LogAppend);
                //this.txtLog.Invoke(callback, new object[] { txt });
            }
            else
            {
                this.txtLog.AppendText($"{txt}{Environment.NewLine}");
            }
        }


        public PublishForm()
        {
            InitializeComponent();
            _syncContext = SynchronizationContext.Current;
        }

        public Result Ini(ProjModel projModel)
        {
            Result res = new Result();
            try
            {
                _projModel = projModel;
                step_Pubsh.StepIndex = 1;
            }
            catch (Exception e)
            {
                txtLog.AppendText($"{e.Message}{Environment.NewLine}");
            }
            return res;
        }

        public bool BuildProj()
        {
            try
            {
                // 判断项目类型
                if (_projModel.IsNetCore())
                {
                    DotNetBuild();
                }
                else
                {
                    Msbuild();
                }
                return true;
            }
            catch (Exception e)
            {
                txtLog.AppendText($"编译项目失败：{e.Message}{Environment.NewLine}");
                return false;
            }
        }

        public bool SelectFiles()
        {
            try
            {
                return true;
            }
            catch (Exception e)
            {
                txtLog.AppendText($"打包文件失败：{e.Message}{Environment.NewLine}");
                return false;
            }
        }

        #region Build

        private void Msbuild()
        {
            SetProcess(1);
            if (string.IsNullOrWhiteSpace(_projModel.MsBuildPath))
            {
                txtLog.AppendText($"编译项目失败：未获取到MsBuild路径 {Environment.NewLine}");
                return;
            }
            string filePath = ProjectHelper.GetBuildToPath(_projModel.LibName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                txtLog.AppendText($"编译项目失败：未获取到文件生成路径 {Environment.NewLine}");
                return;
            }
            var toPath = filePath.Replace("\\\\", "\\");
            if (toPath.EndsWith("\\"))
            {
                toPath = toPath.Substring(0, toPath.Length - 1);
            }
            ClearPublishFolder(toPath);
            var buildArg = "\"" + _projModel.ProjPath.Replace("\\\\", "\\") + "\"";
            buildArg += " /verbosity:minimal /p:Configuration=Debug /p:DeployOnBuild=true /p:Platform=AnyCPU /t:WebPublish /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=False /p:publishUrl=\"" + toPath + "\"";
            SetProcess(2);

            var isSuccess = RunCmd(_projModel.MsBuildPath, buildArg);
            if (!isSuccess)
            {
                SetProcess(0);
                txtLog.AppendText($"编译项目失败 {Environment.NewLine}");
                return;
            }
            SetProcess(100);
        }

        private void DotNetBuild()
        {
            SetProcess(1);
            string filePath = ProjectHelper.GetBuildToPath(_projModel.LibName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                txtLog.AppendText($"编译项目失败：未获取到文件生成路径 {Environment.NewLine}");
                return;
            }
            var toPath = filePath.Replace("\\\\", "\\");
            if (toPath.EndsWith("\\"))
            {
                toPath = toPath.Substring(0, toPath.Length - 1);
            }
            ClearPublishFolder(toPath);

            var buildArg = $"publish \"{_projModel.ProjPath}\" -c Debug ";
            buildArg += " -o \"" + toPath + "\"";
            SetProcess(2);

            var isSuccess = RunCmd("dotnet", buildArg);
            if (!isSuccess)
            {
                SetProcess(0);
                txtLog.AppendText($"编译项目失败 {Environment.NewLine}");
                return;
            }
            SetProcess(100);
        }

        /// <summary>
        /// 发布前清空
        /// </summary>
        /// <param name="srcDir"></param>
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
                        if (args.Data.StartsWith(" "))//有这个代表肯定build有出问题
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
                SetProcess(99);
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

        private void step_Pubsh_IndexChecked(object sender, EventArgs e)
        {
            switch (this.step_Pubsh.StepIndex)
            {
                case 1:
                    BuildProj();
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }

        private void PublishForm_Shown(object sender, EventArgs e)
        {
            //for (int i = 0; i < 100; i++)
            //{
            //    Thread.Sleep(100);
            //    //_syncContext.Post(SetProcessVal, i);
            //    this.ucProcessLine1.Value = i;
            //}
        }
    }
}
