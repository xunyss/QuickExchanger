using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace QuickExchanger
{
    /// <summary>
    /// 
    /// </summary>
    class Config
    {
        /// <summary>
        /// 
        /// </summary>
        private const string CONFIG_FILE_NAME = @"config.xml";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Proxy> LoadConfigXML()
        {
            if (!System.IO.File.Exists(CONFIG_FILE_NAME))
            {
                using (StreamWriter file = new StreamWriter(CONFIG_FILE_NAME))
                {
                    file.Write(Properties.Resources.ConfigTemplate);
                    file.Close();
                }
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(CONFIG_FILE_NAME);

            return ParseConfigXML(xml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private static List<Proxy> ParseConfigXML(XmlDocument xml)
        {
            List<Proxy> proxyList = new List<Proxy>();

            int index = 0;
            XmlNodeList proxyNodeList = xml.GetElementsByTagName("proxy");
            foreach (XmlNode proxyNode in proxyNodeList)
            {
                Proxy proxy = new Proxy();
                proxy.Index = index++;

                proxy.Name   = proxyNode.Attributes["name"].Value;
                proxy.Server = GetText(proxyNode, "server");
                proxy.Http   = GetAttr(proxyNode, "protocols/http" , "server");
                proxy.Https  = GetAttr(proxyNode, "protocols/https", "server");
                proxy.Ftp    = GetAttr(proxyNode, "protocols/ftp"  , "server");
                proxy.Socks  = GetAttr(proxyNode, "protocols/socks", "server");

                XmlNode exceptionsNode = proxyNode.SelectSingleNode("exceptions");
                if (exceptionsNode != null)
                {
                    foreach (XmlNode hostNode in exceptionsNode.ChildNodes)
                    {
                        if (hostNode.GetType() != typeof(XmlComment))
                        {
                            proxy.Exceptions.Add("host".Equals(hostNode.Name) ? hostNode.InnerText : hostNode.Name);
                        }
                    }
                }

                proxyList.Add(proxy);
            }

            return proxyList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseNode"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private static string GetText(XmlNode baseNode, string xpath)
        {
            XmlNode node = baseNode.SelectSingleNode(xpath);
            if (node != null)
            {
                return node.InnerText;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseNode"></param>
        /// <param name="xpath"></param>
        /// <param name="strAttr"></param>
        /// <returns></returns>
        private static string GetAttr(XmlNode baseNode, string xpath, string strAttr)
        {
            XmlNode node = baseNode.SelectSingleNode(xpath);
            if (node != null)
            {
                XmlAttribute attr = node.Attributes[strAttr];
                if (attr != null)
                {
                    return attr.Value;
                }
            }

            return null;
        }
    }
}
