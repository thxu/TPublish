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
            this.ucCheckBox1 = new HZH_Controls.Controls.UCCheckBox();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ucCheckBox1);
            this.panel3.Controls.Add(this.tvFiles);
            this.panel3.Size = new System.Drawing.Size(537, 563);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 624);
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
            this.tvFiles.Size = new System.Drawing.Size(513, 438);
            this.tvFiles.TabIndex = 0;
            this.tvFiles.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCheck);
            // 
            // ucCheckBox1
            // 
            this.ucCheckBox1.BackColor = System.Drawing.Color.Transparent;
            this.ucCheckBox1.Checked = false;
            this.ucCheckBox1.Location = new System.Drawing.Point(12, 450);
            this.ucCheckBox1.Name = "ucCheckBox1";
            this.ucCheckBox1.Padding = new System.Windows.Forms.Padding(1);
            this.ucCheckBox1.Size = new System.Drawing.Size(102, 30);
            this.ucCheckBox1.TabIndex = 1;
            this.ucCheckBox1.TextValue = "排除文件";
            // 
            // SelectFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyleColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(537, 688);
            this.DoubleBuffered = true;
            this.Name = "SelectFilesForm";
            this.Text = "SelectFilesForm";
            this.Title = "选择要发布的文件";
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvFiles;
        private HZH_Controls.Controls.UCCheckBox ucCheckBox1;
    }
}