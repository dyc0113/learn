using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SyncChatClient
{
    public partial class FManaDirectory : Form
    {
        public CDbFile _dbFile;
        Main_Form _mainForm;
        public void Init(Main_Form mainForm, CDbFile dbFile)
        {
            _dbFile = dbFile;
            _mainForm = mainForm;
        }

        public FManaDirectory()
        {
            InitializeComponent();
        }

        private void FManaDirectory_Load(object sender, EventArgs e)
        {
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "winRoot目录";   //设置列标题 
            ch.Width = 400;    //设置列宽度 
            ch.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式 
            this.lvDir.Columns.Add(ch);    //将列头添加到ListView控件。 
            this.lvDir.Columns.Add("linux目录", 500, HorizontalAlignment.Left);
            _dbFile.LoadSynDirectory();

            lvDir.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度  
            CSynDirectory synDir = _dbFile._synDirectory; // 目录同步
            foreach (CSynDirectoryItem item in synDir.lsItems)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = item.win_dir;
                lvi.SubItems.Add(item.linux_dir);
                lvDir.Items.Add(lvi);
            }
            lvDir.EndUpdate();  //结束数据处理，UI界面一次性绘制。 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLinux.Text)
                && !string.IsNullOrEmpty(txtWin.Text))
            {
                // 添加到
                this.lvDir.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 

                ListViewItem lvi = new ListViewItem();
                lvi.Tag = "";
                lvi.Text = this.txtWin.Text;

                lvi.SubItems.Add(this.txtLinux.Text);
                this.lvDir.Items.Add(lvi);

                this.lvDir.EndUpdate();  //结束数据处理，UI界面一次性绘制。 
            }
           
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvDir.SelectedItems)
            {
                lvDir.Items.RemoveAt(lvi.Index); // 按索引移除 
            }
        }

        private bool IsIn(List<CFileWatcher> lsFileWatches, CFileWatcher find)
        {
            foreach (CFileWatcher item in lsFileWatches)
            {
                if (item.GetWindowsPath() == find.GetWindowsPath()
                    && item._linux_root_path == find._linux_root_path)
                {
                    return true; 
                }
            }
            return false;
        }
        private void FManaDirectory_FormClosed(object sender, FormClosedEventArgs e)
        {

            // 保存数据
            CSynDirectory synDir = new CSynDirectory();
            foreach (ListViewItem li in lvDir.Items)
            {
                CSynDirectoryItem item = new CSynDirectoryItem();
                item.win_dir = li.Text;
                item.linux_dir = li.SubItems[1].Text;
                synDir.lsItems.Add(item);
            }
            _dbFile._synDirectory = synDir;
            _dbFile.SaveSynDirectory();

            // 把结果保存到同步目录当中
            List<CFileWatcher> lsFileWatches = _mainForm._lsFileWatches;
            foreach (CFileWatcher sss in lsFileWatches)
            {
                sss.Stop();
            }
            lsFileWatches.Clear();
            int i = 1;
            foreach (CSynDirectoryItem item in synDir.lsItems)
            {
                CFileWatcher fileWather = new CFileWatcher(_mainForm, item.win_dir, item.linux_dir);
                fileWather._allFileWatch = lsFileWatches;
                fileWather.id = i++;
                if (!IsIn(lsFileWatches, fileWather)) // 添加不在同步集合里面的
                {
                    lsFileWatches.Add(fileWather);
                    if (_mainForm._isSyn)
                        fileWather.Start();
                }

            }

        }

        private void lvDir_SelectedIndexChanged(object sender, EventArgs e)
        {
           foreach(ListViewItem item in  this.lvDir.SelectedItems)
           {
               this.txtWin.Text = item.SubItems[0].Text;
               this.txtLinux.Text = item.SubItems[1].Text;
           }
        }
    }
}
