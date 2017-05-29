using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace SyncChatClient
{
    public partial class FCommTools : Form
    {
        public FCommTools()
        {
            InitializeComponent();
        }

        private void txtSelectDir_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.SelectedPath = this.txtDir.Text;
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtDir.Text = this.folderBrowserDialog1.SelectedPath;

            }
        }
        int GetDirs(string path, List<string> lsPath)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            try
            {
                DirectoryInfo[] chldFolders = folder.GetDirectories();
                foreach (DirectoryInfo chldFolder in chldFolders)
                {
                    lsPath.Add(chldFolder.FullName);
                    GetDirs(chldFolder.FullName, lsPath);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("当前目录无效:" + path);
                return -1;
            }
            return 0;
        }
        private void btnGetDir_Click(object sender, EventArgs e)
        {
            List<string> lsPath = new List<string>();
            GetDirs(this.txtDir.Text.Trim(), lsPath);



            string str = "";
            foreach (string s in lsPath)
            {
                str += s + ";";
            }
            this.txtLeft.Text = str;
        }

        private void splitContainer1_Panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }
    }
}
