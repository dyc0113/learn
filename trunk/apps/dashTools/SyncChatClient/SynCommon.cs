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
namespace SyncChatClient
{
    public class SynCommon
    {
        public static void GetFiles(string filePath, TreeNode node, ListView lvTask)
        {
            DirectoryInfo folder = new DirectoryInfo(filePath);
            node.Text = "【目录】" + folder.Name;
            node.Tag = folder.FullName;
            try
            {
                DirectoryInfo[] chldFolders = folder.GetDirectories();
                if (chldFolders == null)
                {

                }

                foreach (DirectoryInfo chldFolder in chldFolders)
                {
                    TreeNode chldNode = new TreeNode();
                    node.Nodes.Add(chldNode);
                    GetFiles(chldFolder.FullName, chldNode, lvTask);
                }
                FileInfo[] chldFiles = folder.GetFiles("*.*");
                foreach (FileInfo chlFile in chldFiles)
                {
                    TreeNode chldNode = new TreeNode();
                    chldNode.Text = chlFile.Name;
                    chldNode.Tag = chlFile.FullName;

                    for (int i = 0; i < lvTask.Items.Count; ++i)
                    {
                        if ((lvTask.Items[i].Tag as string) == chlFile.FullName)
                        {
                            chldNode.Checked = true;
                            break;
                        }
                    }

                    node.Nodes.Add(chldNode);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("当前目录无效:" + filePath);
                return;
            }

        }
        public static bool IsFile(string filePath)
        {
            if (File.Exists(filePath))
                return true;
            else
                return false;
        }
      
       
        public static string ShortFilePath(string fullPath)
        {
            int cnt = 0;
            for (int i = fullPath.Length - 1; i >= 0; i--)
            {
                if (fullPath[i] == '\\')
                {
                    cnt++;
                }
                if (cnt == 2)
                {
                    return fullPath.Substring(i+1, fullPath.Length -i-1);
                }
            }
            return fullPath;
        }
     
        // 把当前时间转换成utc 时间从 1970 1. 1 到现在的秒数
        public static UInt64 ToUTCSec(DateTime dt)
        {
                
             TimeSpan ts = dt - new DateTime(1970, 1, 1, 0, 0, 0, 0);

             UInt64 last_update_tm = Convert.ToUInt64(ts.TotalSeconds - 8 * 3600);
             return last_update_tm;
        }


        public static byte[] intToBytes(int value)
        {
            byte[] src = new byte[4];
            src[3] = (byte)((value >> 24) & 0xFF);
            src[2] = (byte)((value >> 16) & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);
            src[0] = (byte)(value & 0xFF);
            return src;
        }
        /**  
           * 将int数值转换为占四个字节的byte数组，本方法适用于(高位在前，低位在后)的顺序。  和bytesToInt2（）配套使用 
           */
        public static byte[] intToBytes2(int value)
        {
            byte[] src = new byte[4];
            src[0] = (byte)((value >> 24) & 0xFF);
            src[1] = (byte)((value >> 16) & 0xFF);
            src[2] = (byte)((value >> 8) & 0xFF);
            src[3] = (byte)(value & 0xFF);
            return src;
        }

        /**  
    * byte数组中取int数值，本方法适用于(低位在前，高位在后)的顺序，和和intToBytes（）配套使用 
    *   
    * @param src  
    *            byte数组  
    * @param offset  
    *            从数组的第offset位开始  
    * @return int数值  
    */
        public static int bytesToInt(byte[] src, int offset)
        {
            int value;
            
            value = (int)((src[offset] & 0xFF)
                    | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24));
            return value;
        }

        /**  
           * byte数组中取int数值，本方法适用于(低位在后，高位在前)的顺序。和intToBytes2（）配套使用 
           */
        public static int bytesToInt2(byte[] src, int offset)
        {
            int value;
            value = (int)(((src[offset] & 0xFF) << 24)
                    | ((src[offset + 1] & 0xFF) << 16)
                    | ((src[offset + 2] & 0xFF) << 8)
                    | (src[offset + 3] & 0xFF));
            return value;
        }  
    }

}
