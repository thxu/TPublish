namespace TPublish.WinFormClient.WinForms
{
    partial class SelectFilesForm
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
            this.tvFiles = new System.Windows.Forms.TreeView();
            this.ucCheckBox_ChkAll = new HZH_Controls.Controls.UCCheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pannel_ChkList = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.tvFiles);
            this.panel3.Size = new System.Drawing.Size(537, 463);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 524);
            this.panel2.Size = new System.Drawing.Size(537, 64);
            // 
            // tvFiles
            // 
            this.tvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvFiles.CheckBoxes = true;
            this.tvFiles.HotTracking = true;
            this.tvFiles.ItemHeight = 25;
            this.tvFiles.Location = new System.Drawing.Point(12, 6);
            this.tvFiles.Name = "tvFiles";
            this.tvFiles.Size = new System.Drawing.Size(513, 299);
            this.tvFiles.TabIndex = 0;
            this.tvFiles.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCheck);
            // 
            // ucCheckBox_ChkAll
            // 
            this.ucCheckBox_ChkAll.BackColor = System.Drawing.Color.Transparent;
            this.ucCheckBox_ChkAll.Checked = false;
            this.ucCheckBox_ChkAll.Location = new System.Drawing.Point(6, 22);
            this.ucCheckBox_ChkAll.Name = "ucCheckBox_ChkAll";
            this.ucCheckBox_ChkAll.Padding = new System.Windows.Forms.Padding(1);
            this.ucCheckBox_ChkAll.Size = new System.Drawing.Size(102, 30);
            this.ucCheckBox_ChkAll.TabIndex = 1;
            this.ucCheckBox_ChkAll.TextValue = "全选";
            this.ucCheckBox_ChkAll.CheckedChangeEvent += new System.EventHandler(this.ucCheckBox_ChkAll_CheckedChangeEvent);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pannel_ChkList);
            this.groupBox1.Controls.Add(this.ucCheckBox_ChkAll);
            this.groupBox1.Location = new System.Drawing.Point(12, 311);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(513, 147);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "快速勾选";
            // 
            // pannel_ChkList
            // 
            this.pannel_ChkList.AutoScroll = true;
            this.pannel_ChkList.Location = new System.Drawing.Point(6, 58);
            this.pannel_ChkList.Name = "pannel_ChkList";
            this.pannel_ChkList.Size = new System.Drawing.Size(501, 83);
            this.pannel_ChkList.TabIndex = 2;
            // 
            // SelectFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyleColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(537, 588);
            this.DoubleBuffered = true;
            this.Name = "SelectFilesForm";
            this.Text = "SelectFilesForm";
            this.Title = "选择要发布的文件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectFilesForm_FormClosing);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvFiles;
        private HZH_Controls.Controls.UCCheckBox ucCheckBox_ChkAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel pannel_ChkList;
    }
}