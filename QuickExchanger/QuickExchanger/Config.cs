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
        public class ConfigObject
        {
            public List<IPSetting> ipsetList;
            public List<Proxy> proxyList;
        }

        /// <summary>
        /// 
        /// </summary>
        private const string CONFIG_FILE_NAME = @"config.xml";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ConfigObject LoadConfigXML()
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

            ConfigObject confObj = new ConfigObject();
            confObj.ipsetList = ParseConnectionConfig(xml);
            confObj.proxyList = ParseProxyConfig(xml);

            return confObj;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        private static List<IPSetting> ParseConnectionConfig(XmlDocument xml)
        {
            List<IPSetting> ipsetList = new List<IPSetting>();

            int index = 0, ipsetidx = 0;
            XmlNodeList connNodeList = xml.GetElementsByTagName("connection");
            foreach (XmlNode connNode in connNodeList)
            {
                Connection conn = new Connection();
                conn.Index = index++;

                conn.Name = GetAttr(connNode, "name");
                conn.Alias = GetAttr(connNode, "alias");

                XmlNodeList ipsetNodeList = connNode.SelectNodes("ipsetting");
                foreach (XmlNode ipsetNode in ipsetNodeList)
                {
                    IPSetting ipsetting = new IPSetting();
                    ipsetting.Conn = conn;
                    ipsetting.Index = ipsetidx++;

                    ipsetting.Name     = GetAttr(ipsetNode, "name");
                    ipsetting.AddrDHCP = "dhcp" == GetAttr(ipsetNode, "address", "src");
                    ipsetting.DnsDHCP  = "dhcp" == GetAttr(ipsetNode, "dns", "src");
                    ipsetting.Ipaddr   = GetText(ipsetNode, "address/ipaddr");
                    ipsetting.Subnet   = GetText(ipsetNode, "address/subnet");
                    ipsetting.Gateway  = GetText(ipsetNode, "address/gateway");

                    int dnsidx = 0;
                    foreach (XmlNode dnsNode in ipsetNode.ChildNodes)
                    {
                        if (dnsNode.GetType() != typeof(XmlComment))
                        {
                            if ("dns" == dnsNode.Name && dnsidx < 2)
                            {
                                if (dnsidx == 0)
                                {
                                    ipsetting.Dns1 = dnsNode.InnerText;
                                }
                                else if (dnsidx == 1)
                                {
                                    ipsetting.Dns2 = dnsNode.InnerText;
                                }
                                dnsidx++;
                            }
                        }
                    }

                    conn.Ipsets.Add(ipsetting);
                    ipsetList.Add(ipsetting);
                }
            }

            return ipsetList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private static List<Proxy> ParseProxyConfig(XmlDocument xml)
        {
            List<Proxy> proxyList = new List<Proxy>();

            int index = 0;
            XmlNodeList proxyNodeList = xml.GetElementsByTagName("proxy");
            foreach (XmlNode proxyNode in proxyNodeList)
            {
                Proxy proxy = new Proxy();
                proxy.Index = index++;

                proxy.Name   = GetAttr(proxyNode, "name");
                proxy.Pac    = GetText(proxyNode, "pac");
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
                            proxy.Exceptions.Add("host" == hostNode.Name ? hostNode.InnerText : hostNode.Name);
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
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetAttr(XmlNode node, string name)
        {
            XmlAttribute attr = node.Attributes[name];
            if (attr != null)
            {
                return attr.Value;
            }
            return "";
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
    }
}
