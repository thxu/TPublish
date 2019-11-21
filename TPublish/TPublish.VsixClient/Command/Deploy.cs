using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPublish.VsixClient.Model;
using Process = System.Diagnostics.Process;
using Task = System.Threading.Tasks.Task;

namespace TPublish.VsixClient.Command
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Deploy
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("a5ca32b4-5a8f-4d9d-a88f-74a21fbc7d70");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deploy"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private Deploy(AsyncPackage package, OleMenuCommandService commandService)
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
        public static Deploy Instance
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
            // Switch to the main thread - the call to AddCommand in Deploy's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new Deploy(package, commandService);
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
                    MessageBox.Show("您还未选中项目");
                    return;
                }

                if (projInfo.Kind != "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}")
                {
                    throw new Exception("当前插件仅支持C#程序");
                }

                var assembly = Assembly.GetExecutingAssembly();
                var codeBase = assembly.Location;
                var codeBaseDirectory = Path.GetDirectoryName(codeBase);
                var exeName = Path.Combine(codeBaseDirectory, "TPublish.WinFormClientApp.exe");
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = exeName;
                    process.StartInfo.Arguments = projInfo.FullName;
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.Verb = "runas";
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                var dte = await ServiceProvider.GetServiceAsync(typeof(DTE)) as DTE2;
                if (dte == null)
                {
                    return null;
                }
                var projInfo = (Array)dte.ToolWindows.SolutionExplorer.SelectedItems;
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
    }
}
