namespace TPublish.ClientVsix
{
    partial class DeployForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbAppType = new System.Windows.Forms.Label();
            this.cbAppName = new System.Windows.Forms.ComboBox();
            this.lbAppPath = new System.Windows.Forms.Label();
            this.btnDeploy = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbAppPublishDir = new System.Windows.Forms.ComboBox();
            this.linklbChooseFiles = new System.Windows.Forms.LinkLabel();
            this.lbChoosedFiles = new System.Windows.Forms.Label();
            this.bwUploadZip = new System.ComponentModel.BackgroundWorker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbUpload = new System.Windows.Forms.ProgressBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "应用程序类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "服务器程序名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "服务器程序路径：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "部署文件：";
            // 
            // lbAppType
            // 
            this.lbAppType.AutoSize = true;
            this.lbAppType.Location = new System.Drawing.Point(131, 24);
            this.lbAppType.Name = "lbAppType";
            this.lbAppType.Size = new System.Drawing.Size(11, 12);
            this.lbAppType.TabIndex = 5;
            this.lbAppType.Text = "-";
            // 
            // cbAppName
            // 
            this.cbAppName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAppName.FormattingEnabled = true;
            this.cbAppName.Location = new System.Drawing.Point(129, 71);
            this.cbAppName.Name = "cbAppName";
            this.cbAppName.Size = new System.Drawing.Size(311, 20);
            this.cbAppName.TabIndex = 6;
            this.cbAppName.SelectedIndexChanged += new System.EventHandler(this.cbAppName_SelectedIndexChanged);
            // 
            // lbAppPath
            // 
            this.lbAppPath.AutoSize = true;
            this.lbAppPath.Location = new System.Drawing.Point(131, 124);
            this.lbAppPath.Name = "lbAppPath";
            this.lbAppPath.Size = new System.Drawing.Size(11, 12);
            this.lbAppPath.TabIndex = 7;
            this.lbAppPath.Text = "-";
            // 
            // btnDeploy
            // 
            this.btnDeploy.Location = new System.Drawing.Point(129, 357);
            this.btnDeploy.Name = "btnDeploy";
            this.btnDeploy.Size = new System.Drawing.Size(75, 23);
            this.btnDeploy.TabIndex = 11;
            this.btnDeploy.Text = "发布";
            this.btnDeploy.UseVisualStyleBackColor = true;
            this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(76, 272);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "状态：";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(127, 315);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(11, 12);
            this.lbStatus.TabIndex = 13;
            this.lbStatus.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "本机程序位置：";
            // 
            // cbAppPublishDir
            // 
            this.cbAppPublishDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAppPublishDir.FormattingEnabled = true;
            this.cbAppPublishDir.Location = new System.Drawing.Point(129, 171);
            this.cbAppPublishDir.Name = "cbAppPublishDir";
            this.cbAppPublishDir.Size = new System.Drawing.Size(311, 20);
            this.cbAppPublishDir.TabIndex = 15;
            this.cbAppPublishDir.SelectedIndexChanged += new System.EventHandler(this.cbAppPublishDir_SelectedIndexChanged);
            // 
            // linklbChooseFiles
            // 
            this.linklbChooseFiles.AutoSize = true;
            this.linklbChooseFiles.Location = new System.Drawing.Point(127, 224);
            this.linklbChooseFiles.Name = "linklbChooseFiles";
            this.linklbChooseFiles.Size = new System.Drawing.Size(53, 12);
            this.linklbChooseFiles.TabIndex = 16;
            this.linklbChooseFiles.TabStop = true;
            this.linklbChooseFiles.Text = "选择文件";
            this.linklbChooseFiles.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklbChooseFiles_LinkClicked);
            // 
            // lbChoosedFiles
            // 
            this.lbChoosedFiles.AutoSize = true;
            this.lbChoosedFiles.Location = new System.Drawing.Point(180, 224);
            this.lbChoosedFiles.Name = "lbChoosedFiles";
            this.lbChoosedFiles.Size = new System.Drawing.Size(11, 12);
            this.lbChoosedFiles.TabIndex = 17;
            this.lbChoosedFiles.Text = "-";
            // 
            // bwUploadZip
            // 
            this.bwUploadZip.WorkerReportsProgress = true;
            this.bwUploadZip.WorkerSupportsCancellation = true;
            this.bwUploadZip.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwUploadZip_DoWork);
            this.bwUploadZip.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwUploadZip_ProgressChanged);
            this.bwUploadZip.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwUploadZip_RunWorkerCompleted);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(247, 357);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pbUpload
            // 
            this.pbUpload.Location = new System.Drawing.Point(129, 267);
            this.pbUpload.Name = "pbUpload";
            this.pbUpload.Size = new System.Drawing.Size(311, 23);
            this.pbUpload.TabIndex = 19;
            // 
            // DeployForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 399);
            this.Controls.Add(this.pbUpload);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbChoosedFiles);
            this.Controls.Add(this.linklbChooseFiles);
            this.Controls.Add(this.cbAppPublishDir);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnDeploy);
            this.Controls.Add(this.lbAppPath);
            this.Controls.Add(this.cbAppName);
            this.Controls.Add(this.lbAppType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "DeployForm";
            this.Text = "应用程序信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbAppType;
        private System.Windows.Forms.ComboBox cbAppName;
        private System.Windows.Forms.Label lbAppPath;
        private System.Windows.Forms.Button btnDeploy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbAppPublishDir;
        private System.Windows.Forms.LinkLabel linklbChooseFiles;
        private System.Windows.Forms.Label lbChoosedFiles;
        private System.ComponentModel.BackgroundWorker bwUploadZip;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar pbUpload;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}