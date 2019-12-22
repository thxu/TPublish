namespace TPublish.VsixClient2017.WinForms
{
    partial class PublishForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.step_Pubsh = new HZH_Controls.Controls.UCStep();
            this.ucProcessLine1 = new HZH_Controls.Controls.UCProcessLine();
            this.txtLog = new HZH_Controls.Controls.TextBoxEx();
            this.SuspendLayout();
            // 
            // step_Pubsh
            // 
            this.step_Pubsh.BackColor = System.Drawing.Color.Transparent;
            this.step_Pubsh.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.step_Pubsh.ImgCompleted = null;
            this.step_Pubsh.LineWidth = 2;
            this.step_Pubsh.Location = new System.Drawing.Point(12, 12);
            this.step_Pubsh.Name = "step_Pubsh";
            this.step_Pubsh.Size = new System.Drawing.Size(527, 95);
            this.step_Pubsh.StepBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(189)))));
            this.step_Pubsh.StepFontColor = System.Drawing.Color.White;
            this.step_Pubsh.StepForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.step_Pubsh.StepIndex = 0;
            this.step_Pubsh.Steps = new string[] {
        "准备项目",
        "选择文件",
        "推送到服务器"};
            this.step_Pubsh.StepWidth = 35;
            this.step_Pubsh.TabIndex = 0;
            this.step_Pubsh.IndexChecked += new System.EventHandler(this.step_Pubsh_IndexChecked);
            // 
            // ucProcessLine1
            // 
            this.ucProcessLine1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.ucProcessLine1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ucProcessLine1.ForeColor = System.Drawing.Color.White;
            this.ucProcessLine1.Location = new System.Drawing.Point(12, 129);
            this.ucProcessLine1.MaxValue = 100;
            this.ucProcessLine1.Name = "ucProcessLine1";
            this.ucProcessLine1.Size = new System.Drawing.Size(527, 29);
            this.ucProcessLine1.TabIndex = 1;
            this.ucProcessLine1.Text = "ucProcessLine1";
            this.ucProcessLine1.Value = 0;
            this.ucProcessLine1.ValueBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.ucProcessLine1.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucProcessLine1.ValueTextType = HZH_Controls.Controls.ValueTextType.Percent;
            // 
            // txtLog
            // 
            this.txtLog.DecLength = 2;
            this.txtLog.InputType = HZH_Controls.TextInputType.NotControl;
            this.txtLog.Location = new System.Drawing.Point(12, 173);
            this.txtLog.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.txtLog.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.txtLog.Multiline = true;
            this.txtLog.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.txtLog.Name = "txtLog";
            this.txtLog.OldText = null;
            this.txtLog.PromptColor = System.Drawing.Color.Gray;
            this.txtLog.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLog.PromptText = "";
            this.txtLog.RegexPattern = "";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(441, 194);
            this.txtLog.TabIndex = 2;
            // 
            // PublishForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 379);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.ucProcessLine1);
            this.Controls.Add(this.step_Pubsh);
            this.Name = "PublishForm";
            this.Text = "PublishForm";
            this.Shown += new System.EventHandler(this.PublishForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HZH_Controls.Controls.UCStep step_Pubsh;
        private HZH_Controls.Controls.UCProcessLine ucProcessLine1;
        private HZH_Controls.Controls.TextBoxEx txtLog;
    }
}