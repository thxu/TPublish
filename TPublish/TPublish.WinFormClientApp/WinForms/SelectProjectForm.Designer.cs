namespace TPublish.WinFormClientApp.WinForms
{
    partial class SelectProjectForm
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
            this.btnSelectProj = new MetroFramework.Controls.MetroButton();
            this.btnSelectFolder = new MetroFramework.Controls.MetroButton();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.ListSelectedRecords = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroStyleExtender1 = new MetroFramework.Components.MetroStyleExtender(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectProj
            // 
            this.btnSelectProj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectProj.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSelectProj.Location = new System.Drawing.Point(23, 343);
            this.btnSelectProj.Name = "btnSelectProj";
            this.btnSelectProj.Size = new System.Drawing.Size(159, 51);
            this.btnSelectProj.TabIndex = 3;
            this.btnSelectProj.Text = "选择项目";
            this.btnSelectProj.UseSelectable = true;
            this.btnSelectProj.Click += new System.EventHandler(this.btnSelectProj_Click);
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectFolder.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSelectFolder.Location = new System.Drawing.Point(188, 343);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(159, 51);
            this.btnSelectFolder.TabIndex = 4;
            this.btnSelectFolder.Text = "选择文件夹\r\n(可跳过编译直接部署)";
            this.btnSelectFolder.UseSelectable = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 67);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(233, 19);
            this.metroLabel1.TabIndex = 1;
            this.metroLabel1.Text = "最近20项记录，双击某一项直接选择";
            // 
            // ListSelectedRecords
            // 
            this.ListSelectedRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroStyleExtender1.SetApplyMetroTheme(this.ListSelectedRecords, true);
            this.ListSelectedRecords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.ListSelectedRecords.FullRowSelect = true;
            this.ListSelectedRecords.HideSelection = false;
            this.ListSelectedRecords.Location = new System.Drawing.Point(23, 89);
            this.ListSelectedRecords.Name = "ListSelectedRecords";
            this.ListSelectedRecords.Size = new System.Drawing.Size(510, 248);
            this.ListSelectedRecords.TabIndex = 2;
            this.ListSelectedRecords.UseCompatibleStateImageBehavior = false;
            this.ListSelectedRecords.DoubleClick += new System.EventHandler(this.ListSelectedRecords_DoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "名称";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "路径";
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            // 
            // SelectProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 413);
            this.Controls.Add(this.ListSelectedRecords);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.btnSelectProj);
            this.Name = "SelectProjectForm";
            this.Text = "项目选择";
            this.Shown += new System.EventHandler(this.SelectProjectForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnSelectProj;
        private MetroFramework.Controls.MetroButton btnSelectFolder;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private System.Windows.Forms.ListView ListSelectedRecords;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Components.MetroStyleExtender metroStyleExtender1;
    }
}