namespace SyncChatClient
{
    partial class FManaWinRoots
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
            this.lvWinRoot = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvWinRoot
            // 
            this.lvWinRoot.AllowColumnReorder = true;
            this.lvWinRoot.FullRowSelect = true;
            this.lvWinRoot.HideSelection = false;
            this.lvWinRoot.Location = new System.Drawing.Point(12, 23);
            this.lvWinRoot.Name = "lvWinRoot";
            this.lvWinRoot.ShowItemToolTips = true;
            this.lvWinRoot.Size = new System.Drawing.Size(548, 332);
            this.lvWinRoot.TabIndex = 14;
            this.lvWinRoot.UseCompatibleStateImageBehavior = false;
            this.lvWinRoot.View = System.Windows.Forms.View.Details;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(576, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "删除";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FManaWinRoots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 370);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lvWinRoot);
            this.Name = "FManaWinRoots";
            this.Text = "FChangeDirs";
            this.Load += new System.EventHandler(this.FManaWinRoots_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvWinRoot;
        private System.Windows.Forms.Button button1;
    }
}