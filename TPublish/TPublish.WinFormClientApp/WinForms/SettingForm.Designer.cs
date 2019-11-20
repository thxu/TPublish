namespace TPublish.WinFormClientApp.WinForms
{
    partial class SettingForm
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.txtAuthor = new MetroFramework.Controls.MetroTextBox();
            this.txtApiAdress = new MetroFramework.Controls.MetroTextBox();
            this.txtApiKey = new MetroFramework.Controls.MetroTextBox();
            this.txtMsBuildPath = new MetroFramework.Controls.MetroTextBox();
            this.btnSelectMsBuild = new MetroFramework.Controls.MetroButton();
            this.btnSaveSetting = new MetroFramework.Controls.MetroButton();
            this.btnReset = new MetroFramework.Controls.MetroButton();
            this.btnTestConnect = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(89, 75);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(51, 19);
            this.metroLabel1.TabIndex = 4;
            this.metroLabel1.Text = "姓名：";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(47, 112);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(93, 19);
            this.metroLabel2.TabIndex = 6;
            this.metroLabel2.Text = "服务器地址：";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(77, 149);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(63, 19);
            this.metroLabel3.TabIndex = 7;
            this.metroLabel3.Text = "ApiKey：";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(20, 211);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(120, 19);
            this.metroLabel4.TabIndex = 8;
            this.metroLabel4.Text = "MsBuild.Exe路径：";
            // 
            // txtAuthor
            // 
            this.txtAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtAuthor.CustomButton.Image = null;
            this.txtAuthor.CustomButton.Location = new System.Drawing.Point(232, 1);
            this.txtAuthor.CustomButton.Name = "";
            this.txtAuthor.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtAuthor.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtAuthor.CustomButton.TabIndex = 1;
            this.txtAuthor.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtAuthor.CustomButton.UseSelectable = true;
            this.txtAuthor.CustomButton.Visible = false;
            this.txtAuthor.Lines = new string[0];
            this.txtAuthor.Location = new System.Drawing.Point(146, 73);
            this.txtAuthor.MaxLength = 32767;
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.PasswordChar = '\0';
            this.txtAuthor.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtAuthor.SelectedText = "";
            this.txtAuthor.SelectionLength = 0;
            this.txtAuthor.SelectionStart = 0;
            this.txtAuthor.ShortcutsEnabled = true;
            this.txtAuthor.Size = new System.Drawing.Size(254, 23);
            this.txtAuthor.TabIndex = 9;
            this.txtAuthor.UseSelectable = true;
            this.txtAuthor.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtAuthor.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtApiAdress
            // 
            // 
            // 
            // 
            this.txtApiAdress.CustomButton.Image = null;
            this.txtApiAdress.CustomButton.Location = new System.Drawing.Point(232, 1);
            this.txtApiAdress.CustomButton.Name = "";
            this.txtApiAdress.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtApiAdress.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtApiAdress.CustomButton.TabIndex = 1;
            this.txtApiAdress.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtApiAdress.CustomButton.UseSelectable = true;
            this.txtApiAdress.CustomButton.Visible = false;
            this.txtApiAdress.Lines = new string[0];
            this.txtApiAdress.Location = new System.Drawing.Point(146, 110);
            this.txtApiAdress.MaxLength = 32767;
            this.txtApiAdress.Name = "txtApiAdress";
            this.txtApiAdress.PasswordChar = '\0';
            this.txtApiAdress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtApiAdress.SelectedText = "";
            this.txtApiAdress.SelectionLength = 0;
            this.txtApiAdress.SelectionStart = 0;
            this.txtApiAdress.ShortcutsEnabled = true;
            this.txtApiAdress.Size = new System.Drawing.Size(254, 23);
            this.txtApiAdress.TabIndex = 10;
            this.txtApiAdress.UseSelectable = true;
            this.txtApiAdress.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtApiAdress.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtApiKey
            // 
            // 
            // 
            // 
            this.txtApiKey.CustomButton.Image = null;
            this.txtApiKey.CustomButton.Location = new System.Drawing.Point(232, 1);
            this.txtApiKey.CustomButton.Name = "";
            this.txtApiKey.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtApiKey.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtApiKey.CustomButton.TabIndex = 1;
            this.txtApiKey.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtApiKey.CustomButton.UseSelectable = true;
            this.txtApiKey.CustomButton.Visible = false;
            this.txtApiKey.Lines = new string[0];
            this.txtApiKey.Location = new System.Drawing.Point(146, 147);
            this.txtApiKey.MaxLength = 32767;
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.PasswordChar = '\0';
            this.txtApiKey.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtApiKey.SelectedText = "";
            this.txtApiKey.SelectionLength = 0;
            this.txtApiKey.SelectionStart = 0;
            this.txtApiKey.ShortcutsEnabled = true;
            this.txtApiKey.Size = new System.Drawing.Size(254, 23);
            this.txtApiKey.TabIndex = 11;
            this.txtApiKey.UseSelectable = true;
            this.txtApiKey.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtApiKey.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtMsBuildPath
            // 
            // 
            // 
            // 
            this.txtMsBuildPath.CustomButton.Image = null;
            this.txtMsBuildPath.CustomButton.Location = new System.Drawing.Point(186, 1);
            this.txtMsBuildPath.CustomButton.Name = "";
            this.txtMsBuildPath.CustomButton.Size = new System.Drawing.Size(67, 67);
            this.txtMsBuildPath.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtMsBuildPath.CustomButton.TabIndex = 1;
            this.txtMsBuildPath.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtMsBuildPath.CustomButton.UseSelectable = true;
            this.txtMsBuildPath.CustomButton.Visible = false;
            this.txtMsBuildPath.Lines = new string[0];
            this.txtMsBuildPath.Location = new System.Drawing.Point(146, 186);
            this.txtMsBuildPath.MaxLength = 32767;
            this.txtMsBuildPath.Multiline = true;
            this.txtMsBuildPath.Name = "txtMsBuildPath";
            this.txtMsBuildPath.PasswordChar = '\0';
            this.txtMsBuildPath.ReadOnly = true;
            this.txtMsBuildPath.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsBuildPath.SelectedText = "";
            this.txtMsBuildPath.SelectionLength = 0;
            this.txtMsBuildPath.SelectionStart = 0;
            this.txtMsBuildPath.ShortcutsEnabled = true;
            this.txtMsBuildPath.Size = new System.Drawing.Size(254, 69);
            this.txtMsBuildPath.TabIndex = 12;
            this.txtMsBuildPath.UseSelectable = true;
            this.txtMsBuildPath.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtMsBuildPath.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnSelectMsBuild
            // 
            this.btnSelectMsBuild.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSelectMsBuild.Location = new System.Drawing.Point(403, 203);
            this.btnSelectMsBuild.Name = "btnSelectMsBuild";
            this.btnSelectMsBuild.Size = new System.Drawing.Size(64, 23);
            this.btnSelectMsBuild.TabIndex = 13;
            this.btnSelectMsBuild.Text = "选择";
            this.btnSelectMsBuild.UseSelectable = true;
            this.btnSelectMsBuild.Click += new System.EventHandler(this.btnSelectMsBuild_Click);
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSaveSetting.Location = new System.Drawing.Point(146, 264);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(98, 36);
            this.btnSaveSetting.TabIndex = 14;
            this.btnSaveSetting.Text = "保存";
            this.btnSaveSetting.UseSelectable = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // btnReset
            // 
            this.btnReset.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnReset.Location = new System.Drawing.Point(403, 232);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(64, 23);
            this.btnReset.TabIndex = 15;
            this.btnReset.Text = "系统默认";
            this.btnReset.UseSelectable = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnTestConnect
            // 
            this.btnTestConnect.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnTestConnect.Location = new System.Drawing.Point(403, 110);
            this.btnTestConnect.Name = "btnTestConnect";
            this.btnTestConnect.Size = new System.Drawing.Size(64, 23);
            this.btnTestConnect.TabIndex = 16;
            this.btnTestConnect.Text = "测试连接";
            this.btnTestConnect.UseSelectable = true;
            this.btnTestConnect.Click += new System.EventHandler(this.btnTestConnect_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 310);
            this.Controls.Add(this.btnTestConnect);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSaveSetting);
            this.Controls.Add(this.btnSelectMsBuild);
            this.Controls.Add(this.txtMsBuildPath);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.txtApiAdress);
            this.Controls.Add(this.txtAuthor);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Name = "SettingForm";
            this.Text = "设置页面";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroTextBox txtAuthor;
        private MetroFramework.Controls.MetroTextBox txtApiAdress;
        private MetroFramework.Controls.MetroTextBox txtApiKey;
        private MetroFramework.Controls.MetroTextBox txtMsBuildPath;
        private MetroFramework.Controls.MetroButton btnSelectMsBuild;
        private MetroFramework.Controls.MetroButton btnSaveSetting;
        private MetroFramework.Controls.MetroButton btnReset;
        private MetroFramework.Controls.MetroButton btnTestConnect;
    }
}