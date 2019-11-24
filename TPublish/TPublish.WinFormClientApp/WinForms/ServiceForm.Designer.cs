namespace TPublish.WinFormClientApp.WinForms
{
    partial class ServiceForm
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.lbSerPath = new MetroFramework.Controls.MetroLabel();
            this.cbProjType = new MetroFramework.Controls.MetroComboBox();
            this.cbServiceName = new MetroFramework.Controls.MetroComboBox();
            this.txtDeployRemark = new MetroFramework.Controls.MetroTextBox();
            this.btnDeploy = new MetroFramework.Controls.MetroButton();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.toolTip1 = new MetroFramework.Components.MetroToolTip();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.cbServiceList = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(37, 126);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(107, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "应用程序类型：";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(23, 168);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(121, 19);
            this.metroLabel2.TabIndex = 1;
            this.metroLabel2.Text = "服务器程序名称：";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(23, 210);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(121, 19);
            this.metroLabel3.TabIndex = 2;
            this.metroLabel3.Text = "服务器程序路径：";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(37, 254);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(107, 19);
            this.metroLabel4.TabIndex = 3;
            this.metroLabel4.Text = "本次发布内容：";
            this.metroLabel4.Visible = false;
            // 
            // lbSerPath
            // 
            this.lbSerPath.AutoSize = true;
            this.lbSerPath.Location = new System.Drawing.Point(150, 210);
            this.lbSerPath.Name = "lbSerPath";
            this.lbSerPath.Size = new System.Drawing.Size(15, 19);
            this.lbSerPath.TabIndex = 4;
            this.lbSerPath.Text = "-";
            // 
            // cbProjType
            // 
            this.cbProjType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbProjType.FormattingEnabled = true;
            this.cbProjType.ItemHeight = 23;
            this.cbProjType.Items.AddRange(new object[] {
            "IIS",
            "EXE"});
            this.cbProjType.Location = new System.Drawing.Point(150, 121);
            this.cbProjType.Name = "cbProjType";
            this.cbProjType.Size = new System.Drawing.Size(236, 29);
            this.cbProjType.TabIndex = 5;
            this.cbProjType.UseSelectable = true;
            this.cbProjType.SelectedIndexChanged += new System.EventHandler(this.cbProjType_SelectedIndexChanged);
            // 
            // cbServiceName
            // 
            this.cbServiceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbServiceName.FormattingEnabled = true;
            this.cbServiceName.ItemHeight = 23;
            this.cbServiceName.Location = new System.Drawing.Point(150, 163);
            this.cbServiceName.Name = "cbServiceName";
            this.cbServiceName.Size = new System.Drawing.Size(236, 29);
            this.cbServiceName.TabIndex = 6;
            this.cbServiceName.UseSelectable = true;
            this.cbServiceName.SelectedIndexChanged += new System.EventHandler(this.cbServiceName_SelectedIndexChanged);
            // 
            // txtDeployRemark
            // 
            this.txtDeployRemark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtDeployRemark.CustomButton.Image = null;
            this.txtDeployRemark.CustomButton.Location = new System.Drawing.Point(154, 1);
            this.txtDeployRemark.CustomButton.Name = "";
            this.txtDeployRemark.CustomButton.Size = new System.Drawing.Size(81, 81);
            this.txtDeployRemark.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtDeployRemark.CustomButton.TabIndex = 1;
            this.txtDeployRemark.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtDeployRemark.CustomButton.UseSelectable = true;
            this.txtDeployRemark.CustomButton.Visible = false;
            this.txtDeployRemark.Lines = new string[0];
            this.txtDeployRemark.Location = new System.Drawing.Point(150, 254);
            this.txtDeployRemark.MaxLength = 32767;
            this.txtDeployRemark.Multiline = true;
            this.txtDeployRemark.Name = "txtDeployRemark";
            this.txtDeployRemark.PasswordChar = '\0';
            this.txtDeployRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDeployRemark.SelectedText = "";
            this.txtDeployRemark.SelectionLength = 0;
            this.txtDeployRemark.SelectionStart = 0;
            this.txtDeployRemark.ShortcutsEnabled = true;
            this.txtDeployRemark.Size = new System.Drawing.Size(236, 83);
            this.txtDeployRemark.TabIndex = 7;
            this.txtDeployRemark.UseSelectable = true;
            this.txtDeployRemark.Visible = false;
            this.txtDeployRemark.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtDeployRemark.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnDeploy
            // 
            this.btnDeploy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeploy.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnDeploy.Location = new System.Drawing.Point(150, 253);
            this.btnDeploy.Name = "btnDeploy";
            this.btnDeploy.Size = new System.Drawing.Size(75, 46);
            this.btnDeploy.TabIndex = 8;
            this.btnDeploy.Text = "发布";
            this.btnDeploy.UseSelectable = true;
            this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnCancel.Location = new System.Drawing.Point(242, 253);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 46);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.toolTip1.StyleManager = null;
            this.toolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            // 
            // cbServiceList
            // 
            this.cbServiceList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbServiceList.FormattingEnabled = true;
            this.cbServiceList.ItemHeight = 23;
            this.cbServiceList.Location = new System.Drawing.Point(150, 79);
            this.cbServiceList.Name = "cbServiceList";
            this.cbServiceList.Size = new System.Drawing.Size(236, 29);
            this.cbServiceList.TabIndex = 11;
            this.cbServiceList.UseSelectable = true;
            this.cbServiceList.SelectedIndexChanged += new System.EventHandler(this.cbServiceList_SelectedIndexChanged);
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(51, 84);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(93, 19);
            this.metroLabel5.TabIndex = 10;
            this.metroLabel5.Text = "服务器地址：";
            // 
            // ServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 317);
            this.Controls.Add(this.cbServiceList);
            this.Controls.Add(this.metroLabel5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDeploy);
            this.Controls.Add(this.txtDeployRemark);
            this.Controls.Add(this.cbServiceName);
            this.Controls.Add(this.cbProjType);
            this.Controls.Add(this.lbSerPath);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Name = "ServiceForm";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Text = "发布到服务器";
            this.Shown += new System.EventHandler(this.ServiceForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel lbSerPath;
        private MetroFramework.Controls.MetroComboBox cbProjType;
        private MetroFramework.Controls.MetroComboBox cbServiceName;
        private MetroFramework.Controls.MetroTextBox txtDeployRemark;
        private MetroFramework.Controls.MetroButton btnDeploy;
        private MetroFramework.Controls.MetroButton btnCancel;
        private MetroFramework.Components.MetroToolTip toolTip1;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Controls.MetroComboBox cbServiceList;
        private MetroFramework.Controls.MetroLabel metroLabel5;
    }
}