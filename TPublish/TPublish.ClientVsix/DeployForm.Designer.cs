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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbAppType = new System.Windows.Forms.Label();
            this.cbAppName = new System.Windows.Forms.ComboBox();
            this.lbAppPath = new System.Windows.Forms.Label();
            this.radioFullPush = new System.Windows.Forms.RadioButton();
            this.radioPartPush = new System.Windows.Forms.RadioButton();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.btnDeploy = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
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
            this.label2.Location = new System.Drawing.Point(58, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "程序名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "程序路径：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "部署方式：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(82, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "备注：";
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
            this.cbAppName.Location = new System.Drawing.Point(131, 65);
            this.cbAppName.Name = "cbAppName";
            this.cbAppName.Size = new System.Drawing.Size(311, 20);
            this.cbAppName.TabIndex = 6;
            this.cbAppName.SelectedIndexChanged += new System.EventHandler(this.cbAppName_SelectedIndexChanged);
            // 
            // lbAppPath
            // 
            this.lbAppPath.AutoSize = true;
            this.lbAppPath.Location = new System.Drawing.Point(131, 110);
            this.lbAppPath.Name = "lbAppPath";
            this.lbAppPath.Size = new System.Drawing.Size(11, 12);
            this.lbAppPath.TabIndex = 7;
            this.lbAppPath.Text = "-";
            // 
            // radioFullPush
            // 
            this.radioFullPush.AutoSize = true;
            this.radioFullPush.Checked = true;
            this.radioFullPush.Location = new System.Drawing.Point(131, 149);
            this.radioFullPush.Name = "radioFullPush";
            this.radioFullPush.Size = new System.Drawing.Size(47, 16);
            this.radioFullPush.TabIndex = 8;
            this.radioFullPush.TabStop = true;
            this.radioFullPush.Text = "全量";
            this.radioFullPush.UseVisualStyleBackColor = true;
            // 
            // radioPartPush
            // 
            this.radioPartPush.AutoSize = true;
            this.radioPartPush.Location = new System.Drawing.Point(182, 149);
            this.radioPartPush.Name = "radioPartPush";
            this.radioPartPush.Size = new System.Drawing.Size(47, 16);
            this.radioPartPush.TabIndex = 9;
            this.radioPartPush.Text = "增量";
            this.radioPartPush.UseVisualStyleBackColor = true;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(131, 188);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(311, 80);
            this.txtRemark.TabIndex = 10;
            // 
            // btnDeploy
            // 
            this.btnDeploy.Location = new System.Drawing.Point(131, 324);
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
            this.label6.Location = new System.Drawing.Point(82, 283);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "状态：";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(137, 283);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(11, 12);
            this.lbStatus.TabIndex = 13;
            this.lbStatus.Text = "-";
            // 
            // DeployForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 376);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnDeploy);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.radioPartPush);
            this.Controls.Add(this.radioFullPush);
            this.Controls.Add(this.lbAppPath);
            this.Controls.Add(this.cbAppName);
            this.Controls.Add(this.lbAppType);
            this.Controls.Add(this.label5);
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbAppType;
        private System.Windows.Forms.ComboBox cbAppName;
        private System.Windows.Forms.Label lbAppPath;
        private System.Windows.Forms.RadioButton radioFullPush;
        private System.Windows.Forms.RadioButton radioPartPush;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Button btnDeploy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbStatus;
    }
}