using System;
using System.ComponentModel.Design;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using TPublish.VsixClient2019.Forms;
using TPublish.VsixClient2019.Service;
using TPublish.VsixClient2019.Settings;
using VSLangProj;
using Task = System.Threading.Tasks.Task;

namespace TPublish.VsixClient2019
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Publish
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("461cdd42-36b5-42bc-b300-33050f142163");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private DTE2 _dte2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Publish"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private Publish(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Publish Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in Publish's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new Publish(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var projInfo = ThreadHelper.JoinableTaskFactory.Run(GetSelectedProjInfoAsync);
                if (projInfo == null)
                {
                    throw new Exception("您还未选中项目");
                }

                if (projInfo.Kind != PrjKind.prjKindCSharpProject)
                {
                    throw new Exception("当前插件仅支持C#程序");
                }

                SaveSolution();

                var projModel = projInfo.AnalysisProject();
                if (projModel == null)
                {
                    throw new Exception("项目信息解析失败");
                }

                OptionPageGrid settingInfo = PublishService.GetSettingPage();
                if (string.IsNullOrWhiteSpace(settingInfo?.IpAdress))
                {
                    throw new Exception("请先完善设置信息");
                }

                // 尝试连接服务器
                var isConnected = PublishService.CheckConnection();
                if (!isConnected)
                {
                    throw new Exception("尝试连接服务器失败");
                }

                var form = new DeployForm();
                var iniRes = form.Ini(projModel);
                if (iniRes.IsSucceed)
                {
                    form.Show();
                }
                else
                {
                    MessageBox.Show(iniRes.Message);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// 获取当前选中的项目
        /// </summary>
        /// <returns>项目信息</returns>
        private async Task<Project> GetSelectedProjInfoAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (ServiceProvider != null)
            {
                _dte2 = await ServiceProvider.GetServiceAsync(typeof(DTE)) as DTE2;
                if (_dte2 == null)
                {
                    return null;
                }
                var projInfo = (Array)_dte2.ToolWindows.SolutionExplorer.SelectedItems;
                foreach (UIHierarchyItem selItem in projInfo)
                {
                    if (selItem.Object is Project item)
                    {
                        return item;
                    }
                }
                return null;
            }
            return null;
        }

        private void SaveSolution()
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                _dte2.Documents.SaveAll();
                var solution = _dte2.Solution;
                if (!solution.Saved)
                {
                    solution.SaveAs(solution.FullName);
                }

                for (int i = 1; i <= solution.Projects.Count; i++)
                {
                    var project = solution.Projects.Item(i);
                    if (project.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                    {
                        continue;
                    }

                    if (!project.Saved)
                    {
                        project.Save();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 获取当前选中项目的输出目录
        /// </summary>
        /// <param name="project">项目信息</param>
        /// <returns>输出路径</returns>
        public static string GetFullOutputPath(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var outputPath = (string)project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value;
            var projFullPath = project.Properties.Item("FullPath").Value?.ToString();
            if (string.IsNullOrWhiteSpace(projFullPath))
            {
                return string.Empty;
            }
            DirectoryInfo projDirectoryInfo = new DirectoryInfo(projFullPath);

            while (outputPath.StartsWith(@"..\"))
            {
                projDirectoryInfo = projDirectoryInfo?.Parent;
                char[] tmp = { '.', '.', '\\' };
                outputPath = outputPath.Substring(3);
            }

            outputPath = outputPath.TrimStart('.', '\\');

            var path = Path.Combine(projDirectoryInfo?.FullName ?? string.Empty, outputPath);
            return path;
        }
    }
}
