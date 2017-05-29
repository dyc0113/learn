namespace SyncChatClient
{
    partial class FManaDirectory
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
            this.lvDir = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.txtWin = new System.Windows.Forms.TextBox();
            this.txtLinux = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvDir
            // 
            this.lvDir.AllowColumnReorder = true;
            this.lvDir.FullRowSelect = true;
            this.lvDir.HideSelection = false;
            this.lvDir.Location = new System.Drawing.Point(12, 155);
            this.lvDir.Name = "lvDir";
            this.lvDir.ShowItemToolTips = true;
            this.lvDir.Size = new System.Drawing.Size(714, 268);
            this.lvDir.TabIndex = 14;
            this.lvDir.UseCompatibleStateImageBehavior = false;
            this.lvDir.View = System.Windows.Forms.View.Details;
            this.lvDir.SelectedIndexChanged += new System.EventHandler(this.lvDir_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(26, 112);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "增加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(127, 112);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 15;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // txtWin
            // 
            this.txtWin.Location = new System.Drawing.Point(140, 33);
            this.txtWin.Name = "txtWin";
            this.txtWin.Size = new System.Drawing.Size(586, 21);
            this.txtWin.TabIndex = 16;
            // 
            // txtLinux
            // 
            this.txtLinux.Location = new System.Drawing.Point(140, 73);
            this.txtLinux.Name = "txtLinux";
            this.txtLinux.Size = new System.Drawing.Size(586, 21);
            this.txtLinux.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "window下目录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "linux下目录";
            // 
            // FManaDirectory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 435);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLinux);
            this.Controls.Add(this.txtWin);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvDir);
            this.Name = "FManaDirectory";
            this.Text = "FManaDirectory";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FManaDirectory_FormClosed);
            this.Load += new System.EventHandler(this.FManaDirectory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvDir;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.TextBox txtWin;
        private System.Windows.Forms.TextBox txtLinux;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}