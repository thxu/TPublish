using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TPublish.ClientVsix.Service;
using TPublish.ClientVsix.Setting;

namespace TPublish.ClientVsix
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Push
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("4a7d00d0-118c-43b4-90bf-ccf7b4e7e4b1");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="Push"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private Push(Package package)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));

            OleMenuCommandService commandService = ServiceProvider.GetService((typeof(IMenuCommandService))) as OleMenuCommandService;
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Push Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
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
        public static void Initialize(Package package)
        {
            // Verify the current thread is the UI thread - the call to AddCommand in Push's constructor requires
            // the UI thread.
            //ThreadHelper.ThrowIfNotOnUIThread();
            Instance = new Push(package);
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
            ThreadHelper.ThrowIfNotOnUIThread();
            try
            {
                string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()----", this.GetType().FullName);
                string title = "Push";

                // Show a message box to prove we were here
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    message,
                    title,
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);


                var projInfo = GetSelectedProjInfo();
                if (projInfo == null)
                {
                    throw new Exception("您还未选中项目");
                }

                var projModel = projInfo.AnalysisProject();
                if (projModel == null)
                {
                    throw new Exception("项目信息解析失败");
                }

                OptionPageGrid settingInfo = TPublishService.GetSettingPage();
                if (string.IsNullOrWhiteSpace(settingInfo?.IpAdress))
                {
                    throw new Exception("请先完善设置信息");
                }

                var form = new DeployForm();
                form.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private Project GetSelectedProjInfo()
        {
            if (!((ServiceProvider.GetService(typeof(DTE))) is DTE2 dte))
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
    }
}
