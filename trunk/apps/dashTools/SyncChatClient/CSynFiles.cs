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
    public class CSynFiles
    {
        static Main_Form _mainForm;
        CDbFile _dbFile;
        private const int N = 4 * 1024 * 1024;
        private byte[] _sendData = new byte[N];
        private int _len; // 报文长度
        private byte[] _receData = new byte[N]; // 接受数据

        private TcpClient client = null;
        private BinaryReader br;
        private BinaryWriter bw;

        public CSynFiles(Main_Form mainFrom, CDbFile dbFile)
        {
            _mainForm = mainFrom;
            _dbFile = dbFile;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="i"></param>
        /// <param name="count">0 不限制长度, 最大可填充的报文大小</param>
        /// <param name="str"></param>
        /// 返回最大填充的位置，从零开始
        static public int ApendByte(byte[] data, int i, int count, string str)
        {
            byte[] tmpData = Encoding.UTF8.GetBytes(str);
            if (i + tmpData.Length > data.Length)
            {
                _mainForm.Log("数据超出范围限制");
                throw new Exception(" 数据超出范围限制");
            }
            if (tmpData.Length > count)
            {
                _mainForm.Log("数据太长，无法填充。");
                throw new Exception(" 数据太长，无法填充");
            }
            int j = 0;
            if (count != 0)
            {
                for (j = 0; j < tmpData.Length && j < count; j++)
                {
                    data[i + j] = tmpData[j];
                }
                return i + j;
            }
            else
            {
                for (j = 0; j < tmpData.Length; j++)
                {
                    data[i + j] = tmpData[j];
                }
                return i + j;
            }
        }
        /// <summary>
        /// 填充报文str, 并返回填充的长度
        /// </summary>
        /// <param name="data"></param>
        /// <param name="i"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        static public int ApendByte(byte[] data, int i, string str )
        {
            byte[] tmpData = Encoding.UTF8.GetBytes(str);
            if (i + tmpData.Length > data.Length)
            {
                _mainForm.Log("数据超出范围限制");
                throw new Exception(" 数据超出范围限制");
            }
            int j = 0;
            for (j = 0; j < tmpData.Length; j++)
            {
                data[i + j] = tmpData[j];
            }
            return tmpData.Length;
        }
        static public void ApendByte(byte[] data, int i, int value)
        {
            byte[] tmpByte = BitConverter.GetBytes(value);
            for (int j = i; j < i + sizeof(int); j++)
            {
                data[j] = tmpByte[j - i];
            }
        }
        static public void ApendByte(byte[] data, int i, UInt64 value)
        {
            byte[] tmp = BitConverter.GetBytes(value);
            BitConverter.GetBytes(value).CopyTo(data, i);
        }

        void FillMessage(string dir, string fileName, UInt64 update_tm, string data)
        {
            try
            {
                _len = 0;
                Array.Clear(_sendData, 0, _sendData.Length);
                // 填充数据
                ApendByte(_sendData, 4, 0);
                ApendByte(_sendData, 4 + 4, 512, dir);
                ApendByte(_sendData, 4 + 4 + 512, 512, fileName);
                ApendByte(_sendData, 4 + 4 + 512 + 512, update_tm);
                _len = ApendByte(_sendData, 4 + 4 + 512 + 512 + 8, N - 1036, data);
                // 填充报文长度
                _mainForm.Log("开始填充报文长度:" + _len);
                ApendByte(_sendData, 0, _len);  // 填充报文长度
            }
            catch (Exception e)
            {
                _mainForm.Log(e.Message);
            }
        }

        public void SynFile(string fileFullName, string linuxDir)
        {

            if (!File.Exists(fileFullName))  // 文件不存在。
                return;
            if (fileFullName.LastIndexOf('\\') == fileFullName.Length - 1)
            {
                _mainForm.Log("不支持同步目录");
                return;
            }
            System.IO.FileInfo file = new System.IO.FileInfo(fileFullName);
            if (file.Length > N)
            {
                MessageBox.Show("同步的文件过大" + fileFullName);
                return;
            }
            _mainForm.Log(SynCommon.ShortFilePath(fileFullName) + " -> " + linuxDir);
            try
            {
                // 判断文件编码方式
                Encoding encode = EncodingType.GetType(fileFullName);
                StreamReader srNew = null;
                if (encode != Encoding.UTF8)
                    srNew = new StreamReader(fileFullName, Encoding.Default);
                else
                    srNew = new StreamReader(fileFullName, Encoding.UTF8);

                string con = srNew.ReadToEnd();
                // 把windows换行符转换成linux换行符
                con = con.Replace("\r\n", "\n");
                // 如果文件名称后缀已.cpp .h 结尾 则 转换成utf8编码
                if (fileFullName.ToLower().EndsWith(".cpp")
                    || fileFullName.ToLower().EndsWith(".h")
                    || fileFullName.ToLower().EndsWith(".jce")
                    )
                {
                    if (encode != Encoding.UTF8)
                    {
                        con = CFileComm.gb2312_utf8(con);
                    }
                }
                srNew.Close();
                UInt64 last_update_tm = SynCommon.ToUTCSec(file.LastWriteTime);
                FillMessage(linuxDir, file.Name, last_update_tm, con);
                // 想服务器发送数据
                SendData(_sendData);
                RecevData();
            }
            catch (IOException e)
            {
                _mainForm.Log(e.ToString()); 
            }
        }

        /// </summary>
        /// <param name="message"></param>
        public void SendData(byte[] stReq)
        {
            try
            {
                bw.Write(stReq, 0, _len);
                bw.Flush();
            }
            catch
            {
                _mainForm.Log("发送消息失败");
            }
        }
        public void SendData(byte[] stReq, int len)
        {
            try
            {
                bw.Write(stReq, 0, len);
                bw.Flush();
            }
            catch
            {
                _mainForm.Log("发送消息失败");
            }
        }
        public int ReceveData(ref string strRecev)
        {
            int iRet = 0;
            _receData = br.ReadBytes(8);
            if (_receData.Length != 8)
            {
                _mainForm.Log("读取开头8字节失败!");
                return -1;
            }
            int dataLen = SynCommon.bytesToInt(_receData, 0);
            // 读取剩余数据
            int extLen = SynCommon.bytesToInt(_receData, 4);

            _receData = br.ReadBytes(dataLen - 8);   // 长度，和返回值其余部分

            strRecev = System.Text.Encoding.UTF8.GetString(_receData);

            _mainForm.Log("服务器: 长度:" + dataLen + "返回值:" + extLen + "  msg:" + strRecev + Environment.NewLine + " " + Environment.NewLine + "  ");
            return iRet;
        }
        private int RecevData()
        {
            int iRet = 0;
            _receData = br.ReadBytes(8);
            if (_receData.Length != 8)
            {
                _mainForm.Log("读取开头8字节失败!");
                return -1;
            }
            int dataLen = SynCommon.bytesToInt(_receData, 0);
            // 读取剩余数据
            int retCode = SynCommon.bytesToInt(_receData, 4);

            _receData = br.ReadBytes(dataLen - 8);   // 长度，和返回值其余部分

            string msg = System.Text.Encoding.UTF8.GetString(_receData);

            _mainForm.Log("服务器: 长度:" + dataLen + "返回值:" + retCode + "  msg:" + msg + Environment.NewLine + " " + Environment.NewLine + "  ");
            return iRet;
        }
        /// <summary>
        /// 登陆，连接服务器
        /// </summary>
        public int Connnet()
        {
            try
            {
                // 重置连接
                if (client != null)
                {
                    br.Close();
                    bw.Close();
                    client.Close();
                }
                client = new TcpClient(_dbFile.GetTargetIp(), 44445);
                _mainForm.Log("连接成功");
            }
            catch(Exception e)
            {
                _mainForm.Log("连接失败" + e.Message);
                return -1;
            }
            //获取网络流
            NetworkStream m_NetStream = client.GetStream();
            //将网络流作为二进制读写对象
            bw = new BinaryWriter(m_NetStream);
            br = new BinaryReader(m_NetStream);
            return 0;
        }
        public void CloseConnect()
        {
            //未与服务器连接前client为null
            if (client != null)
            {
                br.Close();
                bw.Close();
                client.Close();
            }
            
        }
    }
}
