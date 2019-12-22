namespace TPublish.VsixClient2017.WinForms
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbUpload = new System.Windows.Forms.ProgressBar();
            this.lbChoosedFiles = new System.Windows.Forms.Label();
            this.linklbChooseFiles = new System.Windows.Forms.LinkLabel();
            this.cbAppPublishDir = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDeploy = new System.Windows.Forms.Button();
            this.lbAppPath = new System.Windows.Forms.Label();
            this.cbAppName = new System.Windows.Forms.ComboBox();
            this.lbAppType = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bwUploadZip = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(237, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 50;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pbUpload
            // 
            this.pbUpload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbUpload.Location = new System.Drawing.Point(119, 252);
            this.pbUpload.Name = "pbUpload";
            this.pbUpload.Size = new System.Drawing.Size(311, 23);
            this.pbUpload.TabIndex = 51;
            // 
            // lbChoosedFiles
            // 
            this.lbChoosedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbChoosedFiles.AutoSize = true;
            this.lbChoosedFiles.Location = new System.Drawing.Point(170, 209);
            this.lbChoosedFiles.Name = "lbChoosedFiles";
            this.lbChoosedFiles.Size = new System.Drawing.Size(11, 12);
            this.lbChoosedFiles.TabIndex = 49;
            this.lbChoosedFiles.Text = "-";
            // 
            // linklbChooseFiles
            // 
            this.linklbChooseFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linklbChooseFiles.AutoSize = true;
            this.linklbChooseFiles.Location = new System.Drawing.Point(117, 209);
            this.linklbChooseFiles.Name = "linklbChooseFiles";
            this.linklbChooseFiles.Size = new System.Drawing.Size(53, 12);
            this.linklbChooseFiles.TabIndex = 48;
            this.linklbChooseFiles.TabStop = true;
            this.linklbChooseFiles.Text = "选择文件";
            this.linklbChooseFiles.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklbChooseFiles_LinkClicked);
            // 
            // cbAppPublishDir
            // 
            this.cbAppPublishDir.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAppPublishDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAppPublishDir.FormattingEnabled = true;
            this.cbAppPublishDir.Location = new System.Drawing.Point(119, 156);
            this.cbAppPublishDir.Name = "cbAppPublishDir";
            this.cbAppPublishDir.Size = new System.Drawing.Size(311, 20);
            this.cbAppPublishDir.TabIndex = 47;
            this.cbAppPublishDir.SelectedIndexChanged += new System.EventHandler(this.cbAppPublishDir_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 159);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 46;
            this.label7.Text = "本机程序位置：";
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(117, 300);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(11, 12);
            this.lbStatus.TabIndex = 45;
            this.lbStatus.Text = "-";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(66, 257);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 44;
            this.label6.Text = "状态：";
            // 
            // btnDeploy
            // 
            this.btnDeploy.Location = new System.Drawing.Point(119, 342);
            this.btnDeploy.Name = "btnDeploy";
            this.btnDeploy.Size = new System.Drawing.Size(75, 23);
            this.btnDeploy.TabIndex = 43;
            this.btnDeploy.Text = "发布";
            this.btnDeploy.UseVisualStyleBackColor = true;
            this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
            // 
            // lbAppPath
            // 
            this.lbAppPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAppPath.AutoSize = true;
            this.lbAppPath.Location = new System.Drawing.Point(121, 109);
            this.lbAppPath.Name = "lbAppPath";
            this.lbAppPath.Size = new System.Drawing.Size(11, 12);
            this.lbAppPath.TabIndex = 42;
            this.lbAppPath.Text = "-";
            // 
            // cbAppName
            // 
            this.cbAppName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAppName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAppName.FormattingEnabled = true;
            this.cbAppName.Location = new System.Drawing.Point(119, 56);
            this.cbAppName.Name = "cbAppName";
            this.cbAppName.Size = new System.Drawing.Size(311, 20);
            this.cbAppName.TabIndex = 41;
            this.cbAppName.SelectedIndexChanged += new System.EventHandler(this.cbAppName_SelectedIndexChanged);
            // 
            // lbAppType
            // 
            this.lbAppType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAppType.AutoSize = true;
            this.lbAppType.Location = new System.Drawing.Point(121, 9);
            this.lbAppType.Name = "lbAppType";
            this.lbAppType.Size = new System.Drawing.Size(11, 12);
            this.lbAppType.TabIndex = 40;
            this.lbAppType.Text = "-";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 39;
            this.label4.Text = "部署文件：";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 38;
            this.label3.Text = "服务器程序路径：";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 37;
            this.label2.Text = "服务器程序名称：";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "应用程序类型：";
            // 
            // bwUploadZip
            // 
            this.bwUploadZip.WorkerReportsProgress = true;
            this.bwUploadZip.WorkerSupportsCancellation = true;
            this.bwUploadZip.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwUploadZip_DoWork);
            this.bwUploadZip.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwUploadZip_ProgressChanged);
            this.bwUploadZip.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwUploadZip_RunWorkerCompleted);
            // 
            // DeployForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 388);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pbUpload);
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
            this.Text = "DeployForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar pbUpload;
        private System.Windows.Forms.Label lbChoosedFiles;
        private System.Windows.Forms.LinkLabel linklbChooseFiles;
        private System.Windows.Forms.ComboBox cbAppPublishDir;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDeploy;
        private System.Windows.Forms.Label lbAppPath;
        private System.Windows.Forms.ComboBox cbAppName;
        private System.Windows.Forms.Label lbAppType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.BackgroundWorker bwUploadZip;
    }
}