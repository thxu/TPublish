namespace TPublish.WinFormClient.WinForms
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
            this.ucStep = new HZH_Controls.Controls.UCStep();
            this.ucProcessLine = new HZH_Controls.Controls.UCProcessLine();
            this.textLog = new HZH_Controls.Controls.TextBoxEx();
            this.SuspendLayout();
            // 
            // ucStep
            // 
            this.ucStep.BackColor = System.Drawing.Color.Transparent;
            this.ucStep.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucStep.ImgCompleted = null;
            this.ucStep.LineWidth = 2;
            this.ucStep.Location = new System.Drawing.Point(12, 12);
            this.ucStep.Name = "ucStep";
            this.ucStep.Size = new System.Drawing.Size(546, 120);
            this.ucStep.StepBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(189)))));
            this.ucStep.StepFontColor = System.Drawing.Color.White;
            this.ucStep.StepForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucStep.StepIndex = 0;
            this.ucStep.Steps = new string[] {
        "项目准备",
        "选择文件",
        "推送到服务器"};
            this.ucStep.StepWidth = 35;
            this.ucStep.TabIndex = 0;
            this.ucStep.IndexChecked += new System.EventHandler(this.ucStep_IndexChecked);
            // 
            // ucProcessLine
            // 
            this.ucProcessLine.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.ucProcessLine.Font = new System.Drawing.Font("Arial Unicode MS", 10F);
            this.ucProcessLine.ForeColor = System.Drawing.Color.White;
            this.ucProcessLine.Location = new System.Drawing.Point(12, 138);
            this.ucProcessLine.MaxValue = 100;
            this.ucProcessLine.Name = "ucProcessLine";
            this.ucProcessLine.Size = new System.Drawing.Size(546, 37);
            this.ucProcessLine.TabIndex = 1;
            this.ucProcessLine.Text = "ucProcessLine1";
            this.ucProcessLine.Value = 0;
            this.ucProcessLine.ValueBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.ucProcessLine.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucProcessLine.ValueTextType = HZH_Controls.Controls.ValueTextType.Percent;
            // 
            // textLog
            // 
            this.textLog.BackColor = System.Drawing.SystemColors.Control;
            this.textLog.DecLength = 2;
            this.textLog.InputType = HZH_Controls.TextInputType.NotControl;
            this.textLog.Location = new System.Drawing.Point(12, 191);
            this.textLog.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.textLog.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.textLog.Multiline = true;
            this.textLog.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textLog.Name = "textLog";
            this.textLog.OldText = null;
            this.textLog.PromptColor = System.Drawing.Color.Gray;
            this.textLog.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textLog.PromptText = "";
            this.textLog.ReadOnly = true;
            this.textLog.RegexPattern = "";
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textLog.Size = new System.Drawing.Size(546, 181);
            this.textLog.TabIndex = 2;
            // 
            // DeployForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 384);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.ucProcessLine);
            this.Controls.Add(this.ucStep);
            this.Name = "DeployForm";
            this.Text = "部署文件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HZH_Controls.Controls.UCStep ucStep;
        private HZH_Controls.Controls.UCProcessLine ucProcessLine;
        private HZH_Controls.Controls.TextBoxEx textLog;
    }
}

