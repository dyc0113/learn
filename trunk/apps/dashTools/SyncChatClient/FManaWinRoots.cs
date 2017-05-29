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
    public partial class FManaWinRoots : Form
    {
        private CDbFile _dbFile;
        public void Init(CDbFile dbFile)
        {
            _dbFile = dbFile;
        }
        public FManaWinRoots()
        {
            InitializeComponent();
        }

        private void FManaWinRoots_Load(object sender, EventArgs e)
        {
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "win根目录";   //设置列标题 
            ch.Width = 1000;    //设置列宽度 
            ch.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式 
            this.lvWinRoot.Columns.Add(ch);    //将列头添加到ListView控件。 

            lvWinRoot.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度  
            foreach (CDirItem item in _dbFile._dirs.synDirs)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = item.win_dir;
               // lvi.SubItems.Add();
                lvi.Tag = item;
                lvWinRoot.Items.Add(lvi);
            }
            lvWinRoot.EndUpdate();  //结束数据处理，UI界面一次性绘制。 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvWinRoot.SelectedItems)
            {
                lvWinRoot.Items.RemoveAt(lvi.Index); // 按索引移除 
                _dbFile._dirs.synDirs.Remove(lvi.Tag as CDirItem);
            }
            _dbFile._dirs.currentIndex = 0;
            _dbFile.SaveDirs();
        }
    }
}
