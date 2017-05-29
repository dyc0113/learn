using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
// 文件系统，保存和加载结果
namespace SyncChatClient
{
    public class CDirItem
    {
        public string win_dir;
        public List<string> lsLinuDirs = new List<string> ();
    }
    // 所有window目录
    public class CDirs
    {
        public  List<CDirItem> synDirs= new List<CDirItem> ();        // 所有要同步的目录
        public int currentIndex;

    };

    // 目录同步
    public class CSynDirectoryItem
    {
        public string win_dir;
        public string linux_dir;
    };
    public class CSynDirectory
    {
        public List<CSynDirectoryItem> lsItems = new List<CSynDirectoryItem>();
    };

    public class CDbFile
    {
        string _configDir = "E:\\dashdong_work\\config";
        AppSettings _appTasks ;
        AppSettings _appDirs ;  // 保存window 和 linux 同步的目录结构 
        AppSettings _appConfig;  // 配置IP地址等信息
        bool _init = false;

        public CDirs _dirs =new CDirs();  // 同步的目录集合
        public CDirItem GetCurrentItem()
        {
            if (_dirs.currentIndex < _dirs.synDirs.Count)
            {
                return _dirs.synDirs[_dirs.currentIndex];
            }
            return null;
        }
        // 目录同步
        public CSynDirectory _synDirectory = new CSynDirectory();
        public void LoadSynDirectory()
        {
            string value = _appConfig.GetValue("syn_directory");
            if (!string.IsNullOrEmpty(value))
            {
                _synDirectory = JsonHelper.DeserializeJsonToObject<CSynDirectory>(value);
            }
        }
        public void SaveSynDirectory()
        {
            string json = JsonHelper.SerializeObject(_synDirectory);
            _appConfig.SetValue("syn_directory", json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">window下目录，目录作为所有的前缀</param>
        public CDbFile(int index)  
        {
            if (!File.Exists("app.config"))
            {
                MessageBox.Show("存储文件不存在");
                return;
            }
            _appTasks = new AppSettings("app.config", _configDir);
            _appDirs = new AppSettings("app.config", _configDir);  // 保存window 和 linux 同步的目录结构 
            _appConfig = new AppSettings("app.config", _configDir);
            //判断 winPaht是否存在，存在则加载粗来，不存在，则保存
            string dirsJson = _appDirs.GetValue("dirs");
            if (!string.IsNullOrEmpty(dirsJson))
            {
                _dirs = JsonHelper.DeserializeJsonToObject<CDirs>(dirsJson);
            }
            _init = true; 
        }
        public void SaveDirs()
        {
            string json = JsonHelper.SerializeObject(_dirs);
            _appDirs.SetValue("dirs", json);
        }
        //@brief 加载任务
        public  void LoadTask(ListView ls)
        {
            if (!_init)
                return;
            string value = _appTasks.GetValue("Tasks");
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            ls.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度  
            string[] arr = value.Split(';');
            foreach (string s in arr)
            {
                string[] arr2 = s.Split(',');
                if (arr2.Length != 2)
                {
                    continue;
                }
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = arr2[0];
                string filePath = arr2[0];

                lvi.Text = SynCommon.ShortFilePath(filePath);
                lvi.SubItems.Add(arr2[1]);
                ls.Items.Add(lvi);
            }
            ls.EndUpdate();  //结束数据处理，UI界面一次性绘制。 
        }
        //#brief 保存任务
        public  void SaveTask(ListView ls)
        {
            if (!_init)
                return;
            string value = "";
            bool isFirst = true;
            foreach (ListViewItem item in ls.Items)
            {
                string strItem = "";
                strItem = item.Tag + "," + item.SubItems[1].Text;
                if (isFirst)
                {
                    value += strItem;
                    isFirst = false;
                }
                else
                {
                    value += ";" + strItem;
                }

            }
            _appTasks.SetValue("Tasks", value);

        }

        public void LoadDir(ListBox ls)
        {
            if (!_init)
                return;
            if (_dirs.synDirs.Count > 0)
            {

                CDirItem item = _dirs.synDirs[_dirs.currentIndex];
                ls.Items.Clear();
                foreach (string s in item.lsLinuDirs)
                {
                    ls.Items.Add(s);
                    ls.SelectedIndex = ls.Items.Count - 1;
                    ls.ClearSelected();
                }
            } 
            SaveDirs();  // 
        }
        public void SaveLinuxDirs(ListBox lbx_linux_dirs)
        {
            if (!_init)
                return;
            if (_dirs.currentIndex < _dirs.synDirs.Count)
            {
                string value = "";
                bool isFirst = true; 
                CDirItem item = _dirs.synDirs[_dirs.currentIndex];
                item.lsLinuDirs.Clear();
                foreach (string s in lbx_linux_dirs.Items)
                {
                  
                    item.lsLinuDirs.Add(s);
                }
                SaveDirs();  // 
            }
       
        }
        public void SetWindowRootPath(string path)
        {
            if (!_init)
                return;
            if (!string.IsNullOrEmpty(path))
            {
                int i;
                for (i = 0; i < _dirs.synDirs.Count; i++)
                {
                     CDirItem item = _dirs.synDirs[i];
                     if (item.win_dir == path)
                     {
                         _dirs.currentIndex = i;
                         break;
                     }
                }
                if (i >= _dirs.synDirs.Count)
                {
                    // 新建一个项目
                    CDirItem item = new CDirItem();
                    item.win_dir = path;
                    _dirs.synDirs.Add(item);
                    _dirs.currentIndex = _dirs.synDirs.Count - 1;
                }
                SaveDirs();
            }

           // _appDirs.SetValue("SelectPath", path);
        }
        public string GetWindowRootPath()
        {
            if (!_init)
                return "";
            if (_dirs.currentIndex < _dirs.synDirs.Count)
            {
                CDirItem item = _dirs.synDirs[_dirs.currentIndex];
                return item.win_dir;
            }
            else
                return "";
            //return _appDirs.GetValue("SelectPath");
        }
        public void SetSelectIndex(int index)
        {
            _dirs.currentIndex = index;
            SaveDirs();
        }
        public string GetTargetIp()
        {
            return _appConfig.GetValue("IP"); 
        }
    }
}
