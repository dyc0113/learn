namespace SyncChatClient
{
    partial class FCommTools
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtLeft = new System.Windows.Forms.TextBox();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.txtSelectDir = new System.Windows.Forms.Button();
            this.btnGetDir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtLeft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnGetDir);
            this.splitContainer1.Panel2.Controls.Add(this.txtSelectDir);
            this.splitContainer1.Panel2.Controls.Add(this.txtDir);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1392, 686);
            this.splitContainer1.SplitterDistance = 871;
            this.splitContainer1.TabIndex = 2;
            // 
            // txtLeft
            // 
            this.txtLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLeft.Location = new System.Drawing.Point(0, 0);
            this.txtLeft.Multiline = true;
            this.txtLeft.Name = "txtLeft";
            this.txtLeft.Size = new System.Drawing.Size(871, 686);
            this.txtLeft.TabIndex = 0;
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(16, 24);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(414, 21);
            this.txtDir.TabIndex = 0;
            // 
            // txtSelectDir
            // 
            this.txtSelectDir.Location = new System.Drawing.Point(439, 22);
            this.txtSelectDir.Name = "txtSelectDir";
            this.txtSelectDir.Size = new System.Drawing.Size(75, 23);
            this.txtSelectDir.TabIndex = 1;
            this.txtSelectDir.Text = "选择目录";
            this.txtSelectDir.UseVisualStyleBackColor = true;
            this.txtSelectDir.Click += new System.EventHandler(this.txtSelectDir_Click);
            // 
            // btnGetDir
            // 
            this.btnGetDir.Location = new System.Drawing.Point(196, 65);
            this.btnGetDir.Name = "btnGetDir";
            this.btnGetDir.Size = new System.Drawing.Size(99, 23);
            this.btnGetDir.TabIndex = 2;
            this.btnGetDir.Text = "获取所有目录";
            this.btnGetDir.UseVisualStyleBackColor = true;
            this.btnGetDir.Click += new System.EventHandler(this.btnGetDir_Click);
            // 
            // FCommTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1392, 686);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FCommTools";
            this.Text = "CommTools";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtLeft;
        private System.Windows.Forms.Button btnGetDir;
        private System.Windows.Forms.Button txtSelectDir;
        private System.Windows.Forms.TextBox txtDir;
    }
}