using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using SyncChatClient;
namespace SyncChatClient
{
    public partial class Main_Form : Form
    {
        private bool isExit = false;


        public bool _isSyn = false;

        private Dictionary<string, DateTime> lastModifyTime = new Dictionary<string, DateTime>();
        // 树状目录展开控制
        private Dictionary<string, bool> _mTreeExpand = new Dictionary<string, bool>();
        private string _selectNodeTag = "";  // 展开的时候记录位置

        public CDbFile _dbFile  = new CDbFile(0);  // 数据保存对象
        public CSynFiles _synFiles ;
        CFileWatcher _fileWaher;

        public List<CFileWatcher> _lsFileWatches = new List<CFileWatcher>(); // 目录同步的集合
        public Main_Form()
        {
            InitializeComponent();       
        }
        private void LoadComBox()
        {
            // 加载目录combox
            cmbDirs.Items.Clear();
            CDirs dirs = _dbFile._dirs;
            for (int i = 0; i < dirs.synDirs.Count; ++i)
            {
                cmbDirs.Items.Add(dirs.synDirs[i].win_dir);
            }
            if (cmbDirs.Items.Count > 0)
            {
                cmbDirs.SelectedIndex = dirs.currentIndex;
            }
        }
        /// <summary>
        /// 判断文件是否发生改变
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        bool IsChange(string fileFullPath)
        {

            FileInfo fi = new FileInfo(fileFullPath);
            if (lastModifyTime.ContainsKey(fileFullPath))
            {
                if (lastModifyTime[fileFullPath] < fi.LastWriteTime)
                {
                    lastModifyTime[fileFullPath] = fi.LastWriteTime;
                    return true;
                }
            }
            else
            {
                lastModifyTime[fileFullPath] = fi.LastWriteTime;
                return true;
            }
            return false;
        }
        private void Main_Form_Load(object sender, EventArgs e)
        {
            _synFiles = new CSynFiles(this, _dbFile); // 同步文件的类

            Random ran = new Random((int)DateTime.Now.Ticks);
            this.lbx_linux_dirs.HorizontalScrollbar = true;

            // 连接服务器
            //Connnet();
            // 初试话listView
             ColumnHeader ch = new ColumnHeader();
             ch.Text = "文件名称";   //设置列标题 
             ch.Width = 400;    //设置列宽度 
             ch.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式 
             this.lvTask.Columns.Add(ch);    //将列头添加到ListView控件。 
             this.lvTask.Columns.Add("linux目录", 500, HorizontalAlignment.Left);

             LoadComBox();
            // 加载任务
           //  _dbFile.LoadTask(this.lvTask);
             _dbFile.LoadDir(this.lbx_linux_dirs);

             // 初始化树形目录
             this.cmbDirs.Text = _dbFile.GetWindowRootPath();
             TreeNode root = new TreeNode();
             //根目录名称
             root.Text = "目录";
             //根目录路径
             SynCommon.GetFiles(this.cmbDirs.Text, root, lvTask);
             tvFiles.Nodes.Clear();
             foreach (TreeNode n in root.Nodes)
             {
                 tvFiles.Nodes.Add(n);
             }
             _dbFile.SetWindowRootPath(this.cmbDirs.Text);

            // 加载当前同步目录
             _dbFile.LoadSynDirectory();
             CSynDirectory synDir = _dbFile._synDirectory; // 目录同步
             foreach (CSynDirectoryItem item in synDir.lsItems)
             {
                 CFileWatcher fileWatch = new CFileWatcher(this, item.win_dir, item.linux_dir);
                 fileWatch._allFileWatch = _lsFileWatches;
                 _lsFileWatches.Add(fileWatch);
             }
        }

        void SynListViewFile()
        {
            foreach (ListViewItem item in this.lvTask.Items)
            {
                string fileFullPath = item.Tag as string;
                string linuxDir = item.SubItems[1].Text;

                // 判断文件有没有被修改
                if (!IsChange(fileFullPath))
                {
                    continue;
                }

                _synFiles.SynFile(fileFullPath, linuxDir);             // 同步此文件
            }
        }
        private void SendData()
        {
                try
                {
                    //从网络流流中读取字符串
                    //此方法会自动判断字符串长度前缀，并根据长度前缀读取字符串
//                   receivString = br.ReadString();

                    SynListViewFile();
                }
                catch(Exception e)
                {
                    if (isExit == false)
                    {
                        MessageBox.Show("与服务器失去联系" + e.Message);
                    }
                }
        }
        private void PrintMessage(string str)
        {
            if (this.rtb_Dialog.InvokeRequired)
            {
                Action<string> d=PrintMessage;
                this.rtb_Dialog.Invoke(d, new object[] { str });
            }
            else
            {                
                this.rtb_Dialog.AppendText("["+DateTime.Now.ToString("HH:mm:ss.fff")+"]" +str + Environment.NewLine);
                this.rtb_Dialog.ScrollToCaret();
            }
        }

        /// <summary>
        /// 添加其他在线客户端信息
        /// </summary>
        /// <param name="str"></param>
        private void AddOnline(string str)
        {
            if (this.lbx_linux_dirs.InvokeRequired)
            {
                Action<string> d = AddOnline;
                this.lbx_linux_dirs.Invoke(d, new object[] { str });
            }
            else
            {
                this.lbx_linux_dirs.Items.Add(str);
                this.lbx_linux_dirs.SelectedIndex = this.lbx_linux_dirs.Items.Count-1;
                this.lbx_linux_dirs.ClearSelected();
            }
        }

        /// <summary>
        /// 移除掉线的用户
        /// </summary>
        /// <param name="userName"></param>
        private void RemoveUserName(string userName)
        {
            //在UI线程之外(主线程)访问，需要使用委托来间接调用
            if (this.lbx_linux_dirs.InvokeRequired)
            {
                Action<string> d = RemoveUserName;
                this.lbx_linux_dirs.Invoke(d, userName);
            }
            else
            {
                this.lbx_linux_dirs.Items.Remove(userName);
                this.lbx_linux_dirs.SelectedIndex = this.lbx_linux_dirs.Items.Count - 1;
                this.lbx_linux_dirs.ClearSelected();
            }
        }
        public void Log(string str)
        {
            PrintMessage(str);
        }
        /// <summary>
        /// 启动同步 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Send_Click(object sender, EventArgs e)
        {
            //Thread receiveThread = new Thread(new ThreadStart(SendData));
            //receiveThread.IsBackground = true;
            //receiveThread.Start();
            int iRet =  _synFiles.Connnet();
            if (iRet != 0)
            {
                return;
            }
            if (_isSyn == false)
            {
                this.timer1.Start();
                _isSyn = true;
                btn_Syn.Text = "取消同步";
                // 启动目录同步
                foreach (CFileWatcher item in _lsFileWatches)
                {
                    item.Start();
                }
            }
            else
            {
                this.timer1.Stop();
                _isSyn = false;
                btn_Syn.Text = "启动同步";
                // 取消目录同步
                foreach (CFileWatcher item in _lsFileWatches)
                {
                    item.Stop();
                } 
            }
        }

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {

            _synFiles.CloseConnect();

            foreach (CFileWatcher item in _lsFileWatches)
            {
                item.Stop();
            }

            isExit = true;
            _dbFile.SaveLinuxDirs(lbx_linux_dirs);
            _dbFile.SaveTask(lvTask);
            Environment.Exit(0);
        }

        private void txt_UserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                //触发btn_Send的Client事件
                this.btn_Syn.PerformClick();
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_UserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            this.cmbDirs.Text = _dbFile.GetWindowRootPath();
            this.folderBrowserDialog1.SelectedPath = this.cmbDirs.Text; 
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.cmbDirs.Text = this.folderBrowserDialog1.SelectedPath;
                TreeNode root = new TreeNode();
                //根目录名称
                root.Text = "目录";
                //根目录路径
                SynCommon.GetFiles(this.cmbDirs.Text, root, lvTask);
                tvFiles.Nodes.Clear();
                foreach (TreeNode n in root.Nodes)
                {
                    tvFiles.Nodes.Add(n);
                }
                _dbFile.SetWindowRootPath(this.cmbDirs.Text);
                _dbFile.LoadDir(lbx_linux_dirs);
                LoadComBox();
            }
        }

        private void btnAddDir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_dir.Text.Trim()))
            {
                string dir = txt_dir.Text.Trim();
                foreach (string s in this.lbx_linux_dirs.Items)
                {
                    if (s == dir)
                    {
                        MessageBox.Show("已经存在此目录");
                        return;
                    }
                }
                this.lbx_linux_dirs.Items.Add(dir);
                this.lbx_linux_dirs.SelectedIndex = this.lbx_linux_dirs.Items.Count - 1;
                this.lbx_linux_dirs.ClearSelected();
                _dbFile.SaveLinuxDirs(lbx_linux_dirs);
            }
            else
            {
                MessageBox.Show("目录为空值");
            }
   
        }
        private void ForeachTree(TreeNode node, string dir, int flag =1)
        {
            string filePath = node.Tag as string ;
            if (flag == 1)
            {
                // 添加到任务中
                if (node.Checked && SynCommon.IsFile(filePath))
                {

                    bool isExist = false;
                    foreach (ListViewItem it in lvTask.Items)
                    {
                        string fullPath = it.Tag as string;
                        if (fullPath == filePath)
                        {
                            isExist = true;
                        }
                    }
                    if (!isExist)// 如果文件不在里面， 则添加
                    {
                        // 添加到
                        this.lvTask.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
                        ListViewItem lvi = new ListViewItem();
                        lvi.Tag = filePath;
                        lvi.Text = SynCommon.ShortFilePath(filePath);

                        lvi.SubItems.Add(dir);
                        this.lvTask.Items.Add(lvi);

                        this.lvTask.EndUpdate();  //结束数据处理，UI界面一次性绘制。 
                    }

                }
            }
            else if (flag == 2)// 刷新目录树
            {
                bool isExist = false;
                foreach (ListViewItem it in lvTask.Items)
                {
                    string fullPath = it.Tag as string;
                    if (fullPath == filePath)
                    {
                        isExist = true;
                    }
                }
                if (isExist)
                {
                    node.Checked = true;
                }
                else
                    node.Checked = false;
            }
            else if (flag == 3) // 选中listView
            {
                if (lvTask.SelectedItems.Count > 0)
                {
                    string fullName = lvTask.SelectedItems[0].Tag as string;
                    if (filePath == fullName)
                    {
                        node.EnsureVisible();
                    }
                }
            }
            else if (flag == 4)// 保存每个节点的展开结构
            {
                 _mTreeExpand[node.Tag as string] = node.IsExpanded;
                 if (node.IsSelected)  // 记录选中节点的Tag路径
                     _selectNodeTag = node.Tag as string;                   
            }
            else if (flag == 5)// 回复每个节点的展开结构
            {
                bool value = false;
                if (_mTreeExpand.TryGetValue(node.Tag as string, out value))
                {
                    if (value)
                        node.Expand();
                }
                if (node.Tag as string == _selectNodeTag)
                {
                    node.EnsureVisible();
                }
            }
            foreach (TreeNode n in node.Nodes)
            {
                ForeachTree(n, dir, flag);
            }
        }
        // 添加任务
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 获取选中的目录
            string linuxDir = lbx_linux_dirs.SelectedItem as string;
            if (string.IsNullOrEmpty(linuxDir))
            {
                MessageBox.Show("请选择非空目录");
                return;
            }
            Log("选中" + linuxDir);
            foreach(TreeNode n in tvFiles.Nodes)
            {
                ForeachTree(n, linuxDir);
            }
         
        }
         
        private void btnDelTask_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvTask.SelectedItems)
            {
                lvTask.Items.RemoveAt(lvi.Index); // 按索引移除 
            }
            foreach (TreeNode n in tvFiles.Nodes)  // 刷新目录树
            {
                ForeachTree(n, "", 2);
            } 

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendData();
        }

        private void btnfresh_Click(object sender, EventArgs e)
        {
            _mTreeExpand.Clear();//清空扩展信息
            // 记录树结构
            Point pos =  tvFiles.AutoScrollOffset;
            foreach (TreeNode n in tvFiles.Nodes)
            {
                ForeachTree(n, "", 4); 
            }
            this.cmbDirs.Text = _dbFile.GetWindowRootPath();
            TreeNode root = new TreeNode();
            //根目录名称
            root.Text = "目录";
            //根目录路径
            SynCommon.GetFiles(this.cmbDirs.Text, root, lvTask);
            tvFiles.Nodes.Clear();
            foreach (TreeNode n in root.Nodes)
            {
                tvFiles.Nodes.Add(n);
            }
            _dbFile.SetWindowRootPath(this.cmbDirs.Text);
            // 恢复树结构
            foreach (TreeNode n in tvFiles.Nodes)
            {
                ForeachTree(n, "", 5);
            }
            tvFiles.AutoScrollOffset = pos;
        }

        private void btnDelDir_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection indices = lbx_linux_dirs.SelectedIndices;
            int count = indices.Count;
            lbx_linux_dirs.BeginUpdate();
            for (int i = 0; count != 0; i++)
            {
                lbx_linux_dirs.Items.RemoveAt(indices[0]);
                count--;
            }
            lbx_linux_dirs.EndUpdate();
            _dbFile.SaveLinuxDirs(lbx_linux_dirs);
        }

        private void tvFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
           
            if (this.tvFiles.SelectedNode != null)
            {
                // 任务窗口中显示，并保证可见
                string fullFile = tvFiles.SelectedNode.Tag as string;
                for (int i = 0; i < lvTask.Items.Count; ++i)
                {
                    if ((lvTask.Items[i].Tag as string) == fullFile)
                    {
                        lvTask.Items[i].Selected = true;
                        lvTask.Items[i].EnsureVisible(); // 保证可见
                    }
                    else
                        lvTask.Items[i].Selected = false;
                }

                int index1, index2;
                index1 = index2 = -1;
                int count = 0;
                for (int i = fullFile.Length - 1; i >= 0; --i)
                {
                    if (fullFile[i] == '\\')
                    {
                        count++;
                        if (count == 1)
                        {
                            index2 = i;
                        }
                        else if (count == 2)
                        {
                            index1 = i;
                        }
                    }
                }
                if (index1 != -1 && index2 != -1)
                {
                    lbx_linux_dirs.ClearSelected();

                    string lastFolder = fullFile.Substring(index1 + 1, index2 - index1 - 1);
                    Log("lastFolder:" + lastFolder);


                    for (int i = 0; i < lbx_linux_dirs.Items.Count; i++)
                    {
                        string linuxPath = lbx_linux_dirs.Items[i] as string;
                        if (linuxPath.Contains(lastFolder))
                        {
                            lbx_linux_dirs.SelectedIndex = i;
                        }
                    }
                }
            }
        }

        private void lvTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (TreeNode n in tvFiles.Nodes)
            {
                ForeachTree(n, "", 3);
            }
        }

        private void lvTask_ColumnReordered(object sender, ColumnReorderedEventArgs e)
        {

        }

        private void lvTask_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 检查点击的列是不是现在的排序列.
        }

        private void tvFiles_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                bool check = node.Checked;
               
                // 父亲节点选中，子节点也选中
                foreach (TreeNode n in node.Nodes)
                {
                     n.Checked = check; 
                }
            }
        }
        // 切换同步的目录树
        private void btnChangeSynDir_Click(object sender, EventArgs e)
        {

        }

        private void cmbDirs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDirs.Text != "")
            {
                TreeNode root = new TreeNode();
                //根目录名称
                root.Text = "目录";
                //根目录路径
                SynCommon.GetFiles(this.cmbDirs.Text, root, lvTask);
                tvFiles.Nodes.Clear();
                foreach (TreeNode n in root.Nodes)
                {
                    tvFiles.Nodes.Add(n);
                }
                _dbFile.SetWindowRootPath(this.cmbDirs.Text);
                lbx_linux_dirs.Items.Clear();
                _dbFile.LoadDir(lbx_linux_dirs);
            }
        }
        // 刷新界面所有的数据
        private void RefushAll()
        {

            LoadComBox();
            // 加载任务
            _dbFile.LoadTask(this.lvTask);
            _dbFile.LoadDir(this.lbx_linux_dirs);

            // 初始化树形目录
            this.cmbDirs.Text = _dbFile.GetWindowRootPath();
            TreeNode root = new TreeNode();
            //刷新树形目录
            root.Text = "目录";
            SynCommon.GetFiles(this.cmbDirs.Text, root, lvTask);
            tvFiles.Nodes.Clear();
            foreach (TreeNode n in root.Nodes)
            {
                tvFiles.Nodes.Add(n);
            }
            // 设置当前同步的目录
            _dbFile.SetWindowRootPath(this.cmbDirs.Text);
        }
        private void win目录管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FManaWinRoots manaWinRoot = new FManaWinRoots();
            manaWinRoot.Init(this._dbFile);
            manaWinRoot.ShowDialog();
            RefushAll(); // 刷新主界面数据
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            StreamReader sr = new StreamReader("E:\\json.txt", Encoding.Default);

            string str = sr.ReadToEnd();

            sr.Close();
           
        }

        private void 目录同步管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FManaDirectory f = new FManaDirectory();
            f.Init(this, _dbFile);
            f.ShowDialog();
        }

        private void 工具包ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FCommTools fCommTool = new FCommTools();
            fCommTool.ShowDialog();
        } 
    }
}
