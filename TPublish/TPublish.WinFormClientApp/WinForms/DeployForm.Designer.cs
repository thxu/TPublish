namespace TPublish.WinFormClientApp.WinForms
{
    partial class DeployForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buildProgressBar = new MetroFramework.Controls.MetroProgressBar();
            this.richTxtLog = new System.Windows.Forms.RichTextBox();
            this.linkSetting = new MetroFramework.Controls.MetroLink();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroStyleExtender1 = new MetroFramework.Components.MetroStyleExtender(this.components);
            this.metroCbDeployType = new MetroFramework.Controls.MetroComboBox();
            this.deployStep = new TPublish.WinFormClientApp.Controls.StepControl();
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buildProgressBar
            // 
            this.buildProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buildProgressBar.HideProgressText = false;
            this.buildProgressBar.Location = new System.Drawing.Point(23, 215);
            this.buildProgressBar.Name = "buildProgressBar";
            this.buildProgressBar.Size = new System.Drawing.Size(505, 23);
            this.buildProgressBar.TabIndex = 1;
            this.buildProgressBar.UseCustomBackColor = true;
            // 
            // richTxtLog
            // 
            this.richTxtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroStyleExtender1.SetApplyMetroTheme(this.richTxtLog, true);
            this.richTxtLog.BackColor = System.Drawing.SystemColors.Window;
            this.richTxtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTxtLog.CausesValidation = false;
            this.richTxtLog.Location = new System.Drawing.Point(23, 244);
            this.richTxtLog.Name = "richTxtLog";
            this.richTxtLog.ReadOnly = true;
            this.richTxtLog.Size = new System.Drawing.Size(505, 200);
            this.richTxtLog.TabIndex = 4;
            this.richTxtLog.Text = "";
            // 
            // linkSetting
            // 
            this.linkSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkSetting.BackColor = System.Drawing.SystemColors.Control;
            this.linkSetting.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.linkSetting.Location = new System.Drawing.Point(453, 34);
            this.linkSetting.Name = "linkSetting";
            this.linkSetting.Size = new System.Drawing.Size(75, 23);
            this.linkSetting.TabIndex = 6;
            this.linkSetting.Text = "设置";
            this.linkSetting.UseSelectable = true;
            this.linkSetting.UseStyleColors = true;
            this.linkSetting.Click += new System.EventHandler(this.linkSetting_Click);
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = null;
            // 
            // metroCbDeployType
            // 
            this.metroCbDeployType.BackColor = System.Drawing.Color.Transparent;
            this.metroCbDeployType.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.metroCbDeployType.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.metroCbDeployType.FormattingEnabled = true;
            this.metroCbDeployType.ItemHeight = 19;
            this.metroCbDeployType.Items.AddRange(new object[] {
            "FDD(runtime)",
            "SCD(win-x86)",
            "SCD(win-x64)",
            "SCD(oxs-x64)",
            "SCD(linux-x64)",
            "SCD(win-arm)",
            "SCD(linux-arm)"});
            this.metroCbDeployType.Location = new System.Drawing.Point(23, 172);
            this.metroCbDeployType.Name = "metroCbDeployType";
            this.metroCbDeployType.Size = new System.Drawing.Size(111, 25);
            this.metroCbDeployType.TabIndex = 7;
            this.metroToolTip1.SetToolTip(this.metroCbDeployType, "FDD：依赖式部署\r\n需要部署环境已安装NetCore\r\nSCD：独立式部署\r\n编译时已包含对应平台框架\r\n");
            this.metroCbDeployType.UseSelectable = true;
            this.metroCbDeployType.Visible = false;
            this.metroCbDeployType.SelectedIndexChanged += new System.EventHandler(this.metroCbDeployType_SelectedIndexChanged);
            // 
            // deployStep
            // 
            this.deployStep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroStyleExtender1.SetApplyMetroTheme(this.deployStep, true);
            this.deployStep.BackColor = System.Drawing.Color.Transparent;
            this.deployStep.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.deployStep.ImgCompleted = null;
            this.deployStep.LineWidth = 2;
            this.deployStep.Location = new System.Drawing.Point(23, 63);
            this.deployStep.Name = "deployStep";
            this.deployStep.Size = new System.Drawing.Size(505, 146);
            this.deployStep.StepBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(189)))));
            this.deployStep.StepFontColor = System.Drawing.Color.White;
            this.deployStep.StepForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.deployStep.StepIndex = 0;
            this.deployStep.Steps = new string[] {
        "项目准备",
        "选择文件",
        "推送到服务器"};
            this.deployStep.StepStyle = MetroFramework.MetroColorStyle.Default;
            this.deployStep.StepWidth = 35;
            this.deployStep.TabIndex = 3;
            this.deployStep.IndexChecked += new System.EventHandler(this.DeployStepOnIndexChecked);
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.AutomaticDelay = 200;
            this.metroToolTip1.AutoPopDelay = 5000;
            this.metroToolTip1.InitialDelay = 200;
            this.metroToolTip1.ReshowDelay = 40;
            this.metroToolTip1.StripAmpersands = true;
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::TPublish.WinFormClientApp.Properties.Resources.iconsQuestion;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(133, 162);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 15);
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            this.metroToolTip1.SetToolTip(this.pictureBox1, "FDD：依赖式部署\r\n需要部署环境已安装NetCore\r\nSCD：独立式部署\r\n编译时已包含对应平台框架");
            this.pictureBox1.Visible = false;
            // 
            // DeployForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 467);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.metroCbDeployType);
            this.Controls.Add(this.linkSetting);
            this.Controls.Add(this.richTxtLog);
            this.Controls.Add(this.deployStep);
            this.Controls.Add(this.buildProgressBar);
            this.Name = "DeployForm";
            this.Text = "文件部署";
            this.Shown += new System.EventHandler(this.DeployForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroProgressBar buildProgressBar;
        private Controls.StepControl deployStep;
        private System.Windows.Forms.RichTextBox richTxtLog;
        private MetroFramework.Controls.MetroLink linkSetting;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Components.MetroStyleExtender metroStyleExtender1;
        private MetroFramework.Controls.MetroComboBox metroCbDeployType;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

