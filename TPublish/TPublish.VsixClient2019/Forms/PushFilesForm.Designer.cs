namespace TPublish.VsixClient2019.Forms
{
    partial class PushFilesForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.tvPushFiles = new System.Windows.Forms.TreeView();
            this.chk_ShowConfig = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(138, 468);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tvPushFiles
            // 
            this.tvPushFiles.CheckBoxes = true;
            this.tvPushFiles.Location = new System.Drawing.Point(12, 12);
            this.tvPushFiles.Name = "tvPushFiles";
            this.tvPushFiles.Size = new System.Drawing.Size(327, 426);
            this.tvPushFiles.TabIndex = 9;
            this.tvPushFiles.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvPushFiles_AfterCheck);
            // 
            // chk_ShowConfig
            // 
            this.chk_ShowConfig.AutoSize = true;
            this.chk_ShowConfig.Location = new System.Drawing.Point(12, 444);
            this.chk_ShowConfig.Name = "chk_ShowConfig";
            this.chk_ShowConfig.Size = new System.Drawing.Size(132, 16);
            this.chk_ShowConfig.TabIndex = 11;
            this.chk_ShowConfig.Text = "是否显示配置文件项";
            this.chk_ShowConfig.UseVisualStyleBackColor = true;
            this.chk_ShowConfig.CheckedChanged += new System.EventHandler(this.Chk_ShowConfig_CheckedChanged);
            // 
            // PushFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 504);
            this.Controls.Add(this.chk_ShowConfig);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tvPushFiles);
            this.Name = "PushFilesForm";
            this.Text = "PushFilesForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TreeView tvPushFiles;
        private System.Windows.Forms.CheckBox chk_ShowConfig;
    }
}