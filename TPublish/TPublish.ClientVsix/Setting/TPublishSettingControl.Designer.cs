namespace TPublish.ClientVsix.Setting
{
    partial class TPublishSettingControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtAuthour = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIpAdress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtAuthour
            // 
            this.txtAuthour.Location = new System.Drawing.Point(132, 25);
            this.txtAuthour.Name = "txtAuthour";
            this.txtAuthour.Size = new System.Drawing.Size(205, 21);
            this.txtAuthour.TabIndex = 17;
            this.txtAuthour.TextChanged += new System.EventHandler(this.txtAuthour_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "Authour:";
            // 
            // txtIpAdress
            // 
            this.txtIpAdress.Location = new System.Drawing.Point(132, 72);
            this.txtIpAdress.Name = "txtIpAdress";
            this.txtIpAdress.Size = new System.Drawing.Size(210, 21);
            this.txtIpAdress.TabIndex = 19;
            this.txtIpAdress.TextChanged += new System.EventHandler(this.txtIpAdress_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "ServerIp:";
            // 
            // TPublishSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtIpAdress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAuthour);
            this.Controls.Add(this.label3);
            this.Name = "TPublishSettingControl";
            this.Size = new System.Drawing.Size(385, 134);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtAuthour;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIpAdress;
        private System.Windows.Forms.Label label1;
    }
}
