using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SyncChatClient
{
  
    public class CFileWatcher
    {

        private System.IO.FileSystemWatcher _watcher = new FileSystemWatcher();
        
        public string _linux_root_path;
        public CSynFiles _synFiles;
        public Main_Form _mainForm;
        public List<string> _lsExcludeDir = new List<string>();
        public List<CFileWatcher> _allFileWatch; // 所有的同步文件对象
        public int id = 0;
        public string GetWindowsPath()
        {
            return _watcher.Path;
        }
        public CFileWatcher(Main_Form mainForm,  string winRootPath, string linuRootPath)
        {
            _mainForm = mainForm;
            _synFiles = new CSynFiles(mainForm, mainForm._dbFile); // 连接类
            _linux_root_path = linuRootPath;

            _watcher.Path = winRootPath;
            _watcher.EnableRaisingEvents = false;
           // _watcher.Filter = "*.h|*.cpp";
            _watcher.Changed += new FileSystemEventHandler(Changed);
            _watcher.Created += new FileSystemEventHandler(Created);
            _watcher.Deleted += new FileSystemEventHandler(Deleted);
            _watcher.Renamed += new RenamedEventHandler(Renamed);
            _watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName
                                   | NotifyFilters.LastWrite;
            _watcher.IncludeSubdirectories = true;

            // 同步的时候需要排除的目录列表
            _lsExcludeDir.Add(@"trunk\apps\penguin_game\common\jce");
            _lsExcludeDir.Add(@".svn");
            _lsExcludeDir.Add(@".git");
           
        }
        bool IsSyn(string path)
        {
            foreach (string s in _lsExcludeDir)
            {
                if (path.Contains(s))
                {
                    if (!path.Contains(".jce"))
                    {
                        Log("目录被排除，无需同步");
                        return false;
                    }
                }
            }
            int maxPath = 0;
            foreach (CFileWatcher cw in _allFileWatch)
            {
                string winPath = cw.GetWindowsPath();
                if (path.Contains(winPath))
                {
                    if (maxPath < winPath.Length)
                        maxPath = winPath.Length;
                }
            }

            if (this.GetWindowsPath().Length == maxPath)
                return true;
            else
                return false;
        }
        public void Start()
        {
            _synFiles.Connnet();
            _watcher.EnableRaisingEvents = true;
        }
        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _synFiles.CloseConnect();
        }
        // 文件修改被触发两次
        public  void Changed(object source, FileSystemEventArgs e)
        {
            _mainForm.Log("id[" + id + "]文件改变事件处理逻辑 " + e.ChangeType + e.FullPath);

            if (!IsSyn(e.FullPath))
                return;

            string linuxPath = e.FullPath.Replace(GetWindowsPath(), _linux_root_path);
            linuxPath = linuxPath.Replace("\\", "/");

            int index = linuxPath.LastIndexOf("/");

            if (index >= 0)
            {
                linuxPath = linuxPath.Substring(0, index);
            }
            _synFiles.SynFile(e.FullPath, linuxPath);
            Log("id["+ id + "]同步文件:" + e.FullPath + "->" + linuxPath);
            Log(Environment.NewLine);
            Log(Environment.NewLine);
        }
        public void Log(string strLog)
        {
            _mainForm.Log(strLog);
        }
        // 填充报文的头部, 返回报文的总长度
        public int FillReq(byte[] sendData, int flag, string strReq)
        {
            CSynFiles.ApendByte(sendData, 4, flag);      // 填充flag 标记
            int extLen = CSynFiles.ApendByte(sendData, 12, strReq);
            CSynFiles.ApendByte(sendData, 8, extLen);
            CSynFiles.ApendByte(sendData, 0, extLen + 12);
            return extLen + 12;
        }
        public string ConvertToLinuxPath(string winPath)
        {
            string filePath = winPath.Replace(GetWindowsPath(), _linux_root_path);
            filePath = filePath.Replace("\\", "/");
            return filePath;
        }
        public int RemoteRequest(string strReq, ref string stRsp)
        {
            byte[] sendData = new byte[1024];
            int flag = 1;
            // 填充报文
            int len = FillReq(sendData, flag, strReq);
            Log("data len " + len.ToString());
            // 发送数据
            _synFiles.SendData(sendData, len); // 发送数据
            // 接受数据
            _synFiles.ReceveData(ref stRsp);
            return 0;
        }
        /// <summary>
        /// 判断目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsDir(string path)
        {
            return false;
        }

        public void Deleted(object source, FileSystemEventArgs e)
        {
            _mainForm.Log("id[" + id + "]文件删除事件处理逻辑 " + e.ChangeType + e.FullPath);
            if (!IsSyn(e.FullPath))
                return;

            SCommReq<SDelFileReq> delReq = new SCommReq<SDelFileReq>();
            delReq.param = new SDelFileReq();
            string filePath = e.FullPath.Replace(GetWindowsPath(), _linux_root_path);
            filePath = filePath.Replace("\\", "/");
            delReq.method = "del_file";
            delReq.param.file_name = filePath;
            string jsonReq = JsonHelper.SerializeObject(delReq);
            // 填充报文

            string recevData = "";
            RemoteRequest(jsonReq, ref recevData); // 发送请求

            SDelFileRsp stRsp = JsonHelper.DeserializeJsonToObject<SDelFileRsp>(recevData);
            if (stRsp != null)
                Log(stRsp.msg);
            else
                Log("返回值异常");
        }
        public  void Created(object source, FileSystemEventArgs e)
        {
            _mainForm.Log("id[" + id + "]文件创建事件处理逻辑 " + e.ChangeType + e.FullPath );

            if (!IsSyn(e.FullPath))
                return;
            try
            {
                // 文件创建，处理逻辑
                if (File.GetAttributes(e.FullPath) != FileAttributes.Directory)
                {
                    string linuxPath = e.FullPath.Replace(GetWindowsPath(), _linux_root_path);
                    linuxPath = linuxPath.Replace("\\", "/");

                    int index = linuxPath.LastIndexOf("/");

                    if (index >= 0)
                    {
                        linuxPath = linuxPath.Substring(0, index);
                    }
                    _synFiles.SynFile(e.FullPath, linuxPath);
                    Log("id[" + id + "]同步文件:" + e.FullPath + "->" + linuxPath);
                }
                else// 创建目录
                {
                    SCommReq<SCreateDirReq> renameReq = new SCommReq<SCreateDirReq>();
                    renameReq.method = "create_dir";
                    renameReq.param = new SCreateDirReq();
                    renameReq.param.dir = ConvertToLinuxPath(e.FullPath);

                    string jsonReq = JsonHelper.SerializeObject(renameReq);
                    string recevData = "";
                    RemoteRequest(jsonReq, ref recevData); // 发送请求
                    SCreateDirRsp stRsp = JsonHelper.DeserializeJsonToObject<SCreateDirRsp>(recevData);
                    if (stRsp != null)
                        Log(stRsp.msg);
                    else
                        Log("返回值异常");
                    Log("\r\n");
                }
            }
            catch (FileNotFoundException ee)
            {
                Log("找不到文件:" + ee.ToString());
            }
        }
        // 获取文件夹目录
        public string GetDirName(string fullPath, ref string fileName)
        {
            int index = fullPath.LastIndexOf("\\");
            fileName = fullPath.Substring(index + 1, fullPath.Length - index - 1);
            return fullPath.Substring(0, index);
        }

        public void Renamed(object source, RenamedEventArgs e)
        {
            _mainForm.Log("id[" + id + "]文件重命名事件处理逻辑 " + e.ChangeType + e.FullPath);

            if (!IsSyn(e.FullPath))
                return;

            string jsonReq;
            string recevData;

            SCommReq<SRenameReq> renameReq = new SCommReq<SRenameReq>();
            if (File.GetAttributes(e.FullPath) != FileAttributes.Directory)
            {
                renameReq.method = "rename_file";
            }
            else
            {
                renameReq.method = "rename_dir";
            }

            renameReq.param = new SRenameReq();
            renameReq.param.old_name = ConvertToLinuxPath(e.OldFullPath);
            renameReq.param.new_name = ConvertToLinuxPath(e.FullPath);

            jsonReq = JsonHelper.SerializeObject(renameReq);
            // 发送数据
            recevData = "";
            RemoteRequest(jsonReq, ref recevData); // 发送请求
            SRenameRsp stRsp = JsonHelper.DeserializeJsonToObject<SRenameRsp>(recevData);
            if (stRsp != null)
                Log(stRsp.msg);
            else
                Log("返回值异常");
            Log("\r\n");
            Log("触发一次文件变化时间");
            string dirPath, fileName = "";
            dirPath = GetDirName(e.FullPath, ref fileName);
            FileSystemEventArgs fileChanngeEventArgs =
                new FileSystemEventArgs(WatcherChangeTypes.Changed, dirPath, fileName);
            Changed(source, fileChanngeEventArgs);
        }
    }
}
