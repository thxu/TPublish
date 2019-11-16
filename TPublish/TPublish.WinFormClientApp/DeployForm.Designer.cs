namespace TPublish.WinFormClientApp
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
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.buildProgressBar = new MetroFramework.Controls.MetroProgressBar();
            this.textLog = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(450, 180);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(78, 29);
            this.metroButton1.TabIndex = 0;
            this.metroButton1.Text = "metroButton1";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
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
            // 
            // textLog
            // 
            this.textLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.textLog.CustomButton.Image = null;
            this.textLog.CustomButton.Location = new System.Drawing.Point(313, 2);
            this.textLog.CustomButton.Name = "";
            this.textLog.CustomButton.Size = new System.Drawing.Size(189, 189);
            this.textLog.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textLog.CustomButton.TabIndex = 1;
            this.textLog.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textLog.CustomButton.UseSelectable = true;
            this.textLog.CustomButton.Visible = false;
            this.textLog.Lines = new string[0];
            this.textLog.Location = new System.Drawing.Point(23, 254);
            this.textLog.MaxLength = 32767;
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.PasswordChar = '\0';
            this.textLog.ReadOnly = true;
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textLog.SelectedText = "";
            this.textLog.SelectionLength = 0;
            this.textLog.SelectionStart = 0;
            this.textLog.ShortcutsEnabled = true;
            this.textLog.Size = new System.Drawing.Size(505, 194);
            this.textLog.TabIndex = 2;
            this.textLog.UseSelectable = true;
            this.textLog.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textLog.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // DeployForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 467);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.buildProgressBar);
            this.Controls.Add(this.metroButton1);
            this.Name = "DeployForm";
            this.Text = "文件部署";
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroProgressBar buildProgressBar;
        private MetroFramework.Controls.MetroTextBox textLog;
    }
}

