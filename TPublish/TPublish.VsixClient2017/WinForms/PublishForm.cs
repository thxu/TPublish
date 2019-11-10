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

        private void LogAppend(object obj)
        {
            txtLog.AppendText((string)obj);
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
            _syncContext.Post(SetProcessVal, 1);
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
            buildArg += " /verbosity:minimal /p:Configuration=Release /p:DeployOnBuild=true /p:Platform=AnyCPU /t:WebPublish /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=False /p:publishUrl=\"" + toPath + "\"";
            _syncContext.Post(SetProcessVal, 2);

            var isSuccess = RunCmd(_projModel.MsBuildPath, buildArg);
            if (!isSuccess)
            {
                _syncContext.Post(SetProcessVal, 0);
                txtLog.AppendText($"编译项目失败 {Environment.NewLine}");
                return;
            }
            _syncContext.Post(SetProcessVal, 100);
        }

        private void DotNetBuild()
        {
            _syncContext.Post(SetProcessVal, 1);
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

            var buildArg = $"publish \"{_projModel.ProjPath}\" -c Release ";
            buildArg += " -o \"" + toPath + "\"";
            _syncContext.Post(SetProcessVal, 2);

            var isSuccess = RunCmd("dotnet", buildArg);
            if (!isSuccess)
            {
                _syncContext.Post(SetProcessVal, 0);
                txtLog.AppendText($"编译项目失败 {Environment.NewLine}");
                return;
            }
            _syncContext.Post(SetProcessVal, 100);
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
                            _syncContext.Post(LogAppend, args.Data);
                        }
                        if (args.Data.Contains(": warning"))
                        {
                            _syncContext.Post(LogAppend, $"[warning]:{args.Data}");
                        }
                        else if (args.Data.Contains(": error"))
                        {
                            _syncContext.Post(LogAppend, $"[error]:{args.Data}");
                        }
                        else
                        {
                            _syncContext.Post(LogAppend, $"{args.Data}");
                            _syncContext.Post(ProcessUp, 98);
                        }
                    }
                };
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (sender, data) =>
                {
                    if (!string.IsNullOrWhiteSpace(data.Data))
                    {
                        _syncContext.Post(LogAppend, $"[error]:{data.Data}");
                    }
                };
                process.BeginErrorReadLine();

                process.WaitForExit();
                _syncContext.Post(SetProcessVal, 99);
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
    }
}
