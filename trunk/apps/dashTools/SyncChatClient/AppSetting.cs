using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
namespace SyncChatClient
{
    /// <summary>
    /// App.config配置类
    /// </summary>
    public class AppSettings
    {
        public  string _configPath = "";
        public  string _configName = "App.config";
        public string _fullName;
        public AppSettings(string fileName, string configPath = "")
        {
            _configName = fileName;
            _configPath = configPath;
            if (_configPath != "")
            {
                _fullName = _configPath + "\\" + _configName;
            }
            else
                _fullName = _configName;
        }
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns></returns>
        public  string AppConfig()
        {
            //return System.IO.Path.Combine(Application.StartupPath, ConfigName);//此处配置文件在程序目录下，或者设置为指定的配置文件路径
            return _fullName;
        }
        /// <summary>
        /// 获取配置节点值
        /// </summary>
        /// <param name="appKey">节点key值</param>
        /// <returns></returns>
        public  string GetValue(string appKey)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(AppConfig());
                XmlNode xNode;
                XmlElement xElem;
                xNode = xDoc.SelectSingleNode("//appSettings");　　　　//补充，需要在你的app.config 文件中增加一下，<appSetting> </appSetting>
                xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
                if (xElem != null)
                    return xElem.GetAttribute("value");
                else
                    return "";
            }
            catch (Exception e)
            {
                MessageBox.Show("加载配置文件出错:" + AppConfig() + e.ToString());
            }
            return "";
        }
        /// <summary>
        /// 设置配置节点值
        /// </summary>
        /// <param name="AppKey">key</param>
        /// <param name="AppValue">value</param>
        public  void SetValue(string AppKey, string AppValue)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(AppConfig());
            XmlNode xNode;
            XmlElement xElem1;
            XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");
            xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
            if (xElem1 != null)
            {
                xElem1.SetAttribute("value", AppValue);
            }
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", AppKey);
                xElem2.SetAttribute("value", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(AppConfig());
        }
    }
}