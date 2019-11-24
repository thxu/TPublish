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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.txtAuthor = new MetroFramework.Controls.MetroTextBox();
            this.txtMsBuildPath = new MetroFramework.Controls.MetroTextBox();
            this.btnSelectMsBuild = new MetroFramework.Controls.MetroButton();
            this.btnSaveSetting = new MetroFramework.Controls.MetroButton();
            this.btnReset = new MetroFramework.Controls.MetroButton();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.metroTile2 = new MetroFramework.Controls.MetroTile();
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.gridServices = new MetroFramework.Controls.MetroGrid();
            this.menuOfServiceGrid = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.AddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.别名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apiAdress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridServices)).BeginInit();
            this.menuOfServiceGrid.SuspendLayout();
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
            this.metroToolTip1.SetToolTip(this.metroLabel2, "列表上点击鼠标右键\r\n可添加、删除服务器");
            // 
            // metroLabel4
            // 
            this.metroLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(20, 269);
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
            this.txtAuthor.CustomButton.Location = new System.Drawing.Point(300, 1);
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
            this.txtAuthor.Size = new System.Drawing.Size(322, 23);
            this.txtAuthor.TabIndex = 9;
            this.txtAuthor.UseSelectable = true;
            this.txtAuthor.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtAuthor.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtMsBuildPath
            // 
            this.txtMsBuildPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtMsBuildPath.CustomButton.Image = null;
            this.txtMsBuildPath.CustomButton.Location = new System.Drawing.Point(272, 1);
            this.txtMsBuildPath.CustomButton.Name = "";
            this.txtMsBuildPath.CustomButton.Size = new System.Drawing.Size(49, 49);
            this.txtMsBuildPath.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtMsBuildPath.CustomButton.TabIndex = 1;
            this.txtMsBuildPath.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtMsBuildPath.CustomButton.UseSelectable = true;
            this.txtMsBuildPath.CustomButton.Visible = false;
            this.txtMsBuildPath.Lines = new string[0];
            this.txtMsBuildPath.Location = new System.Drawing.Point(146, 261);
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
            this.txtMsBuildPath.Size = new System.Drawing.Size(322, 51);
            this.txtMsBuildPath.TabIndex = 12;
            this.txtMsBuildPath.UseSelectable = true;
            this.txtMsBuildPath.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtMsBuildPath.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnSelectMsBuild
            // 
            this.btnSelectMsBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectMsBuild.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSelectMsBuild.Location = new System.Drawing.Point(471, 261);
            this.btnSelectMsBuild.Name = "btnSelectMsBuild";
            this.btnSelectMsBuild.Size = new System.Drawing.Size(64, 23);
            this.btnSelectMsBuild.TabIndex = 13;
            this.btnSelectMsBuild.Text = "选择";
            this.btnSelectMsBuild.UseSelectable = true;
            this.btnSelectMsBuild.Click += new System.EventHandler(this.btnSelectMsBuild_Click);
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveSetting.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSaveSetting.Location = new System.Drawing.Point(146, 319);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(98, 36);
            this.btnSaveSetting.TabIndex = 14;
            this.btnSaveSetting.Text = "保存";
            this.btnSaveSetting.UseSelectable = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnReset.Location = new System.Drawing.Point(471, 290);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(64, 23);
            this.btnReset.TabIndex = 15;
            this.btnReset.Text = "系统默认";
            this.btnReset.UseSelectable = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            // 
            // metroTile1
            // 
            this.metroTile1.ActiveControl = null;
            this.metroTile1.Location = new System.Drawing.Point(130, 38);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(10, 10);
            this.metroTile1.TabIndex = 17;
            this.metroToolTip1.SetToolTip(this.metroTile1, "点我有惊喜");
            this.metroTile1.UseSelectable = true;
            this.metroTile1.Click += new System.EventHandler(this.metroTile1_Click);
            // 
            // metroTile2
            // 
            this.metroTile2.ActiveControl = null;
            this.metroTile2.Location = new System.Drawing.Point(146, 38);
            this.metroTile2.Name = "metroTile2";
            this.metroTile2.Size = new System.Drawing.Size(10, 10);
            this.metroTile2.TabIndex = 18;
            this.metroToolTip1.SetToolTip(this.metroTile2, "点我有惊喜");
            this.metroTile2.UseSelectable = true;
            this.metroTile2.Click += new System.EventHandler(this.metroTile2_Click);
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // gridServices
            // 
            this.gridServices.AllowUserToAddRows = false;
            this.gridServices.AllowUserToDeleteRows = false;
            this.gridServices.AllowUserToResizeRows = false;
            this.gridServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridServices.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridServices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridServices.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridServices.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridServices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.别名,
            this.apiAdress});
            this.gridServices.ContextMenuStrip = this.menuOfServiceGrid;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridServices.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridServices.EnableHeadersVisualStyles = false;
            this.gridServices.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridServices.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridServices.Location = new System.Drawing.Point(146, 114);
            this.gridServices.MultiSelect = false;
            this.gridServices.Name = "gridServices";
            this.gridServices.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridServices.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridServices.RowHeadersVisible = false;
            this.gridServices.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridServices.RowTemplate.Height = 23;
            this.gridServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridServices.Size = new System.Drawing.Size(322, 133);
            this.gridServices.TabIndex = 19;
            this.metroToolTip1.SetToolTip(this.gridServices, "列表上点击鼠标右键\r\n可添加、删除服务器");
            this.gridServices.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridServices_CellMouseDown);
            // 
            // menuOfServiceGrid
            // 
            this.menuOfServiceGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.menuOfServiceGrid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.menuOfServiceGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddToolStripMenuItem,
            this.DelToolStripMenuItem,
            this.SetDefaultToolStripMenuItem});
            this.menuOfServiceGrid.Name = "menuOfServiceGrid";
            this.menuOfServiceGrid.Size = new System.Drawing.Size(125, 70);
            // 
            // AddToolStripMenuItem
            // 
            this.AddToolStripMenuItem.Name = "AddToolStripMenuItem";
            this.AddToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.AddToolStripMenuItem.Text = "添加";
            this.AddToolStripMenuItem.Click += new System.EventHandler(this.AddToolStripMenuItem_Click);
            // 
            // DelToolStripMenuItem
            // 
            this.DelToolStripMenuItem.Name = "DelToolStripMenuItem";
            this.DelToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.DelToolStripMenuItem.Text = "删除";
            this.DelToolStripMenuItem.Click += new System.EventHandler(this.DelToolStripMenuItem_Click);
            // 
            // SetDefaultToolStripMenuItem
            // 
            this.SetDefaultToolStripMenuItem.Name = "SetDefaultToolStripMenuItem";
            this.SetDefaultToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.SetDefaultToolStripMenuItem.Text = "设为默认";
            this.SetDefaultToolStripMenuItem.Click += new System.EventHandler(this.SetDefaultToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::TPublish.WinFormClientApp.Properties.Resources.iconsQuestion;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(127, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(15, 15);
            this.button1.TabIndex = 22;
            this.metroToolTip1.SetToolTip(this.button1, "列表上点击鼠标右键\r\n可添加、删除服务器");
            this.button1.UseVisualStyleBackColor = false;
            // 
            // 别名
            // 
            this.别名.DataPropertyName = "alias";
            this.别名.HeaderText = "别名";
            this.别名.Name = "别名";
            this.别名.ReadOnly = true;
            this.别名.Width = 80;
            // 
            // apiAdress
            // 
            this.apiAdress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.apiAdress.HeaderText = "服务器地址";
            this.apiAdress.Name = "apiAdress";
            this.apiAdress.ReadOnly = true;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 368);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gridServices);
            this.Controls.Add(this.metroTile2);
            this.Controls.Add(this.metroTile1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSaveSetting);
            this.Controls.Add(this.btnSelectMsBuild);
            this.Controls.Add(this.txtMsBuildPath);
            this.Controls.Add(this.txtAuthor);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Name = "SettingForm";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Text = "设置页面";
            this.Shown += new System.EventHandler(this.SettingForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridServices)).EndInit();
            this.menuOfServiceGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroTextBox txtAuthor;
        private MetroFramework.Controls.MetroTextBox txtMsBuildPath;
        private MetroFramework.Controls.MetroButton btnSelectMsBuild;
        private MetroFramework.Controls.MetroButton btnSaveSetting;
        private MetroFramework.Controls.MetroButton btnReset;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Controls.MetroTile metroTile2;
        private MetroFramework.Controls.MetroTile metroTile1;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private MetroFramework.Controls.MetroGrid gridServices;
        private MetroFramework.Controls.MetroContextMenu menuOfServiceGrid;
        private System.Windows.Forms.ToolStripMenuItem DelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SetDefaultToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem AddToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn 别名;
        private System.Windows.Forms.DataGridViewTextBoxColumn apiAdress;
    }
}