namespace TPublish.WinFormClientApp.WinForms
{
    partial class AddServiceForm
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
            this.txtApiAlias = new MetroFramework.Controls.MetroTextBox();
            this.txtApiAdress = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.btnTestConnect = new MetroFramework.Controls.MetroButton();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.lbApiAdressErr = new MetroFramework.Controls.MetroLabel();
            this.lbApiAliasErr = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtApiAlias
            // 
            // 
            // 
            // 
            this.txtApiAlias.CustomButton.Image = null;
            this.txtApiAlias.CustomButton.Location = new System.Drawing.Point(232, 1);
            this.txtApiAlias.CustomButton.Name = "";
            this.txtApiAlias.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtApiAlias.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtApiAlias.CustomButton.TabIndex = 1;
            this.txtApiAlias.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtApiAlias.CustomButton.UseSelectable = true;
            this.txtApiAlias.CustomButton.Visible = false;
            this.txtApiAlias.Lines = new string[0];
            this.txtApiAlias.Location = new System.Drawing.Point(123, 124);
            this.txtApiAlias.MaxLength = 32767;
            this.txtApiAlias.Name = "txtApiAlias";
            this.txtApiAlias.PasswordChar = '\0';
            this.txtApiAlias.PromptText = "自己定义服务器地址的简称，如：19";
            this.txtApiAlias.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtApiAlias.SelectedText = "";
            this.txtApiAlias.SelectionLength = 0;
            this.txtApiAlias.SelectionStart = 0;
            this.txtApiAlias.ShortcutsEnabled = true;
            this.txtApiAlias.Size = new System.Drawing.Size(254, 23);
            this.txtApiAlias.TabIndex = 14;
            this.txtApiAlias.UseSelectable = true;
            this.txtApiAlias.WaterMark = "自己定义服务器地址的简称，如：19";
            this.txtApiAlias.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtApiAlias.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
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
            this.txtApiAdress.Location = new System.Drawing.Point(123, 75);
            this.txtApiAdress.MaxLength = 32767;
            this.txtApiAdress.Name = "txtApiAdress";
            this.txtApiAdress.PasswordChar = '\0';
            this.txtApiAdress.PromptText = "示例：http://192.168.10.19:9527";
            this.txtApiAdress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtApiAdress.SelectedText = "";
            this.txtApiAdress.SelectionLength = 0;
            this.txtApiAdress.SelectionStart = 0;
            this.txtApiAdress.ShortcutsEnabled = true;
            this.txtApiAdress.Size = new System.Drawing.Size(254, 23);
            this.txtApiAdress.TabIndex = 13;
            this.txtApiAdress.UseSelectable = true;
            this.txtApiAdress.WaterMark = "示例：http://192.168.10.19:9527";
            this.txtApiAdress.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtApiAdress.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(58, 126);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(51, 19);
            this.metroLabel3.TabIndex = 12;
            this.metroLabel3.Text = "别名：";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(28, 75);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(93, 19);
            this.metroLabel1.TabIndex = 15;
            this.metroLabel1.Text = "服务器地址：";
            // 
            // btnSave
            // 
            this.btnSave.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSave.Location = new System.Drawing.Point(302, 174);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "保存";
            this.btnSave.UseSelectable = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTestConnect
            // 
            this.btnTestConnect.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnTestConnect.Location = new System.Drawing.Point(123, 174);
            this.btnTestConnect.Name = "btnTestConnect";
            this.btnTestConnect.Size = new System.Drawing.Size(64, 23);
            this.btnTestConnect.TabIndex = 17;
            this.btnTestConnect.Text = "测试连接";
            this.btnTestConnect.UseSelectable = true;
            this.btnTestConnect.Click += new System.EventHandler(this.btnTestConnect_Click);
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            // 
            // lbApiAdressErr
            // 
            this.lbApiAdressErr.AutoSize = true;
            this.lbApiAdressErr.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbApiAdressErr.ForeColor = System.Drawing.Color.Red;
            this.lbApiAdressErr.Location = new System.Drawing.Point(123, 101);
            this.lbApiAdressErr.Name = "lbApiAdressErr";
            this.lbApiAdressErr.Size = new System.Drawing.Size(12, 15);
            this.lbApiAdressErr.TabIndex = 18;
            this.lbApiAdressErr.Text = "-";
            this.lbApiAdressErr.UseCustomForeColor = true;
            this.lbApiAdressErr.Visible = false;
            // 
            // lbApiAliasErr
            // 
            this.lbApiAliasErr.AutoSize = true;
            this.lbApiAliasErr.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbApiAliasErr.ForeColor = System.Drawing.Color.Red;
            this.lbApiAliasErr.Location = new System.Drawing.Point(123, 150);
            this.lbApiAliasErr.Name = "lbApiAliasErr";
            this.lbApiAliasErr.Size = new System.Drawing.Size(12, 15);
            this.lbApiAliasErr.TabIndex = 19;
            this.lbApiAliasErr.Text = "-";
            this.lbApiAliasErr.UseCustomForeColor = true;
            this.lbApiAliasErr.Visible = false;
            // 
            // AddServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 215);
            this.Controls.Add(this.lbApiAliasErr);
            this.Controls.Add(this.lbApiAdressErr);
            this.Controls.Add(this.btnTestConnect);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.txtApiAlias);
            this.Controls.Add(this.txtApiAdress);
            this.Controls.Add(this.metroLabel3);
            this.Name = "AddServiceForm";
            this.Text = "添加新服务器";
            this.Shown += new System.EventHandler(this.AddServiceForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox txtApiAlias;
        private MetroFramework.Controls.MetroTextBox txtApiAdress;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton btnSave;
        private MetroFramework.Controls.MetroButton btnTestConnect;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Controls.MetroLabel lbApiAdressErr;
        private MetroFramework.Controls.MetroLabel lbApiAliasErr;
    }
}