using System;
using System.Collections.Generic;
using System.Text;

namespace SyncChatClient
{
    public enum CmdType
    {
        /// <summary>
        /// 登陆
        /// </summary>
        Login = 1,
        /// <summary>
        /// 登出
        /// </summary>
        Logout = 2,
        /// <summary>
        /// 交谈
        /// </summary>
        Talk = 3,
    }
  
    /// <summary>
    /// 命令类,使用JSON序列化在客户端和服务器端之间消息传递
    /// </summary>
    class Command
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        public CmdType CmdType { get; set; }

        /// <summary>
        /// 聊天消息发送方
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 聊天消息接收方
        /// </summary>
        public string ChatReceiver { get; set; }

        /// <summary>
        /// 发送的消息
        /// </summary>
        public string Message { get; set; }

     

        //public static string GetSendMessage(string userName, string tagetUserName, string message)
        //{
        //    Command cmd = new Command();
        //    cmd.CmdType = CmdType.Talk;
        //    cmd.ChatReceiver = tagetUserName;
        //    cmd.Message = message;
        //    cmd.UserName = userName;
        //    return JsonConvert.SerializeObject(cmd);
        //}
    }
    struct SliunxDir
    {
        static public string key = "LiunxDir";
        public string dirName;
        public string ToString()
        {
            return dirName;
        }
    };
    struct TaskItem
    {
        public string winFileFullName;
        public string linuxDir;
    };
    struct Task
    {
        static public string key = "Task";
        List<TaskItem> list;
    }



    // 监控文件的变化，支持监控多个文件目录
    // =================================删除文件的的请求===================================
    class SCommReq<T>
    {
        public string method;
        public T param;
    }

    class SDelFileReq
    {
        public string file_name; // 要删除的文件名字
        public SDelFileReq()
        {
        }
    }
    class SDelFileRsp
    {
        public string msg;       // 删除的文件回应
    }

    class SRenameReq
    {
        public string old_name;
        public string new_name;
    }

    class SRenameRsp
    {
        public string msg;       // 删除的文件回应
    }

    class SCreateDirReq
    {
         public string dir;
    }
    class SCreateDirRsp
    {
        public string msg;
    }
    // 删除目录的请求
    class SDelDirReq
    {
        public string dir; // 要删除的文件名字
        public SDelDirReq()
        {
        }
    }
    class SDelDirRsp
    {
        public string msg;       // 删除的文件回应
    }


}
