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
            this.deployStep = new TPublish.WinFormClientApp.Controls.StepControl();
            this.metroStyleExtender1 = new MetroFramework.Components.MetroStyleExtender(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
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
            this.deployStep.StepWidth = 35;
            this.deployStep.TabIndex = 3;
            this.deployStep.IndexChecked += new System.EventHandler(this.DeployStepOnIndexChecked);
            // 
            // DeployForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 467);
            this.Controls.Add(this.linkSetting);
            this.Controls.Add(this.richTxtLog);
            this.Controls.Add(this.deployStep);
            this.Controls.Add(this.buildProgressBar);
            this.Name = "DeployForm";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Text = "文件部署";
            this.Shown += new System.EventHandler(this.DeployForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroProgressBar buildProgressBar;
        private Controls.StepControl deployStep;
        private System.Windows.Forms.RichTextBox richTxtLog;
        private MetroFramework.Controls.MetroLink linkSetting;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Components.MetroStyleExtender metroStyleExtender1;
    }
}

