namespace SyncChatClient
{
    partial class Main_Form
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txt_dir = new System.Windows.Forms.TextBox();
            this.rtb_Dialog = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvFiles = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbx_linux_dirs = new System.Windows.Forms.ListBox();
            this.btn_Syn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentTask = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectDir = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddDir = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvTask = new System.Windows.Forms.ListView();
            this.btnDelTask = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnfresh = new System.Windows.Forms.Button();
            this.btnDelDir = new System.Windows.Forms.Button();
            this.btnChangeSynDir = new System.Windows.Forms.Button();
            this.cmbDirs = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.win目录管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.目录同步管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.工具包ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_dir
            // 
            this.txt_dir.Location = new System.Drawing.Point(531, 84);
            this.txt_dir.Name = "txt_dir";
            this.txt_dir.Size = new System.Drawing.Size(391, 21);
            this.txt_dir.TabIndex = 1;
            this.txt_dir.TextChanged += new System.EventHandler(this.txt_UserName_TextChanged);
            this.txt_dir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_UserName_KeyPress);
            // 
            // rtb_Dialog
            // 
            this.rtb_Dialog.Location = new System.Drawing.Point(12, 511);
            this.rtb_Dialog.Name = "rtb_Dialog";
            this.rtb_Dialog.Size = new System.Drawing.Size(1506, 274);
            this.rtb_Dialog.TabIndex = 3;
            this.rtb_Dialog.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tvFiles);
            this.groupBox1.Location = new System.Drawing.Point(12, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 380);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "目录树";
            // 
            // tvFiles
            // 
            this.tvFiles.CheckBoxes = true;
            this.tvFiles.Location = new System.Drawing.Point(8, 20);
            this.tvFiles.Name = "tvFiles";
            this.tvFiles.Size = new System.Drawing.Size(446, 368);
            this.tvFiles.TabIndex = 1;
            this.tvFiles.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvFiles_AfterCheck);
            this.tvFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFiles_AfterSelect);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbx_linux_dirs);
            this.groupBox2.Location = new System.Drawing.Point(479, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(443, 336);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "linux目录";
            // 
            // lbx_linux_dirs
            // 
            this.lbx_linux_dirs.FormattingEnabled = true;
            this.lbx_linux_dirs.ItemHeight = 12;
            this.lbx_linux_dirs.Location = new System.Drawing.Point(6, 20);
            this.lbx_linux_dirs.Name = "lbx_linux_dirs";
            this.lbx_linux_dirs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbx_linux_dirs.Size = new System.Drawing.Size(416, 304);
            this.lbx_linux_dirs.TabIndex = 0;
            // 
            // btn_Syn
            // 
            this.btn_Syn.Location = new System.Drawing.Point(771, 482);
            this.btn_Syn.Name = "btn_Syn";
            this.btn_Syn.Size = new System.Drawing.Size(75, 23);
            this.btn_Syn.TabIndex = 6;
            this.btn_Syn.Text = "启动同步";
            this.btn_Syn.UseVisualStyleBackColor = true;
            this.btn_Syn.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(472, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "增加目录";
            // 
            // lblCurrentTask
            // 
            this.lblCurrentTask.AutoSize = true;
            this.lblCurrentTask.Location = new System.Drawing.Point(715, 56);
            this.lblCurrentTask.Name = "lblCurrentTask";
            this.lblCurrentTask.Size = new System.Drawing.Size(53, 12);
            this.lblCurrentTask.TabIndex = 9;
            this.lblCurrentTask.Text = "当前任务";
            // 
            // btnSelectDir
            // 
            this.btnSelectDir.Location = new System.Drawing.Point(612, 31);
            this.btnSelectDir.Name = "btnSelectDir";
            this.btnSelectDir.Size = new System.Drawing.Size(75, 23);
            this.btnSelectDir.TabIndex = 12;
            this.btnSelectDir.Text = "选择文件夹";
            this.btnSelectDir.UseVisualStyleBackColor = true;
            this.btnSelectDir.Click += new System.EventHandler(this.btnSelectDir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "输入目录:";
            // 
            // btnAddDir
            // 
            this.btnAddDir.Location = new System.Drawing.Point(936, 82);
            this.btnAddDir.Name = "btnAddDir";
            this.btnAddDir.Size = new System.Drawing.Size(90, 23);
            this.btnAddDir.TabIndex = 6;
            this.btnAddDir.Text = "添加目录";
            this.btnAddDir.UseVisualStyleBackColor = true;
            this.btnAddDir.Click += new System.EventHandler(this.btnAddDir_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(1168, 87);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "添加到任务当中";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvTask
            // 
            this.lvTask.AllowColumnReorder = true;
            this.lvTask.FullRowSelect = true;
            this.lvTask.HideSelection = false;
            this.lvTask.Location = new System.Drawing.Point(936, 131);
            this.lvTask.Name = "lvTask";
            this.lvTask.ShowItemToolTips = true;
            this.lvTask.Size = new System.Drawing.Size(597, 336);
            this.lvTask.TabIndex = 13;
            this.lvTask.UseCompatibleStateImageBehavior = false;
            this.lvTask.View = System.Windows.Forms.View.Details;
            this.lvTask.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTask_ColumnClick);
            this.lvTask.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(this.lvTask_ColumnReordered);
            this.lvTask.SelectedIndexChanged += new System.EventHandler(this.lvTask_SelectedIndexChanged);
            // 
            // btnDelTask
            // 
            this.btnDelTask.Location = new System.Drawing.Point(1262, 87);
            this.btnDelTask.Name = "btnDelTask";
            this.btnDelTask.Size = new System.Drawing.Size(75, 23);
            this.btnDelTask.TabIndex = 7;
            this.btnDelTask.Text = "删除任务";
            this.btnDelTask.UseVisualStyleBackColor = true;
            this.btnDelTask.Click += new System.EventHandler(this.btnDelTask_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnfresh
            // 
            this.btnfresh.Location = new System.Drawing.Point(699, 31);
            this.btnfresh.Name = "btnfresh";
            this.btnfresh.Size = new System.Drawing.Size(75, 23);
            this.btnfresh.TabIndex = 14;
            this.btnfresh.Text = "刷新目录树";
            this.btnfresh.UseVisualStyleBackColor = true;
            this.btnfresh.Click += new System.EventHandler(this.btnfresh_Click);
            // 
            // btnDelDir
            // 
            this.btnDelDir.Location = new System.Drawing.Point(1052, 82);
            this.btnDelDir.Name = "btnDelDir";
            this.btnDelDir.Size = new System.Drawing.Size(75, 23);
            this.btnDelDir.TabIndex = 6;
            this.btnDelDir.Text = "删除目录";
            this.btnDelDir.UseVisualStyleBackColor = true;
            this.btnDelDir.Click += new System.EventHandler(this.btnDelDir_Click);
            // 
            // btnChangeSynDir
            // 
            this.btnChangeSynDir.Location = new System.Drawing.Point(780, 30);
            this.btnChangeSynDir.Name = "btnChangeSynDir";
            this.btnChangeSynDir.Size = new System.Drawing.Size(75, 23);
            this.btnChangeSynDir.TabIndex = 15;
            this.btnChangeSynDir.Text = "切换目录树";
            this.btnChangeSynDir.UseVisualStyleBackColor = true;
            this.btnChangeSynDir.Click += new System.EventHandler(this.btnChangeSynDir_Click);
            // 
            // cmbDirs
            // 
            this.cmbDirs.FormattingEnabled = true;
            this.cmbDirs.Location = new System.Drawing.Point(81, 33);
            this.cmbDirs.Name = "cmbDirs";
            this.cmbDirs.Size = new System.Drawing.Size(525, 20);
            this.cmbDirs.TabIndex = 16;
            this.cmbDirs.SelectedIndexChanged += new System.EventHandler(this.cmbDirs_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.win目录管理ToolStripMenuItem,
            this.目录同步管理ToolStripMenuItem,
            this.工具包ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1545, 25);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // win目录管理ToolStripMenuItem
            // 
            this.win目录管理ToolStripMenuItem.Name = "win目录管理ToolStripMenuItem";
            this.win目录管理ToolStripMenuItem.Size = new System.Drawing.Size(87, 21);
            this.win目录管理ToolStripMenuItem.Text = "win目录管理";
            this.win目录管理ToolStripMenuItem.Click += new System.EventHandler(this.win目录管理ToolStripMenuItem_Click);
            // 
            // 目录同步管理ToolStripMenuItem
            // 
            this.目录同步管理ToolStripMenuItem.Name = "目录同步管理ToolStripMenuItem";
            this.目录同步管理ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.目录同步管理ToolStripMenuItem.Text = "目录同步管理";
            this.目录同步管理ToolStripMenuItem.Click += new System.EventHandler(this.目录同步管理ToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(936, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // 工具包ToolStripMenuItem
            // 
            this.工具包ToolStripMenuItem.Name = "工具包ToolStripMenuItem";
            this.工具包ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.工具包ToolStripMenuItem.Text = "工具包";
            this.工具包ToolStripMenuItem.Click += new System.EventHandler(this.工具包ToolStripMenuItem_Click);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1545, 831);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lvTask);
            this.Controls.Add(this.cmbDirs);
            this.Controls.Add(this.btnChangeSynDir);
            this.Controls.Add(this.btnfresh);
            this.Controls.Add(this.btnSelectDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCurrentTask);
            this.Controls.Add(this.rtb_Dialog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDelTask);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnAddDir);
            this.Controls.Add(this.btnDelDir);
            this.Controls.Add(this.btn_Syn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txt_dir);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件同步程序升级版";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Form_FormClosing);
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_dir;
        private System.Windows.Forms.RichTextBox rtb_Dialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lbx_linux_dirs;
        private System.Windows.Forms.Button btn_Syn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentTask;
        private System.Windows.Forms.TreeView tvFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnSelectDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddDir;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView lvTask;
        private System.Windows.Forms.Button btnDelTask;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnfresh;
        private System.Windows.Forms.Button btnDelDir;
        private System.Windows.Forms.Button btnChangeSynDir;
        private System.Windows.Forms.ComboBox cmbDirs;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem win目录管理ToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem 目录同步管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具包ToolStripMenuItem;
    }
}

