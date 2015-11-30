using System;
using Microsoft.Win32;

namespace QuickExchanger
{
    /// <summary>
    /// 
    /// </summary>
    class InternetSetting
    {
        /// <summary>
        /// 
        /// </summary>
        private const string PROXY_ENABLE = "ProxyEnable";
        private const string PROXY_SERVER = "ProxyServer";
        private const string PROXY_OVERRIDE = "ProxyOverride";


        /// <summary>
        /// 
        /// </summary>
        public static void ClearProxySetting()
        {
            RegistryKey regKey = OpenRegistryKey();

            regKey.DeleteValue(PROXY_SERVER, false);
            regKey.DeleteValue(PROXY_OVERRIDE, false);

            regKey.Close();

            // 프록시서버사용안함
            SetProxyEnable(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool GetProxyEnable()
        {
            RegistryKey regKey = OpenRegistryKey();

            object value = regKey.GetValue(PROXY_ENABLE, 0);
            regKey.Close();

            return (int)value == 0 ? false : true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        public static void SetProxyEnable(bool enable)
        {
            RegistryKey regKey = OpenRegistryKey();

            regKey.SetValue(PROXY_ENABLE, enable ? 1 : 0);
            regKey.Close();

            // 인터넷설정정보갱신
            WinInetWrap.RefreshInternetSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyServer"></param>
        /// <param name="proxyOverride"></param>
        public static void SetProxySetting(string proxyServer, string proxyOverride)
        {
            RegistryKey regKey = OpenRegistryKey();

            if (proxyServer != null && proxyServer.Length > 0)
            {
                regKey.SetValue(PROXY_SERVER, proxyServer);
            }
            else
            {
                regKey.DeleteValue(PROXY_SERVER, false);
            }

            if (proxyOverride != null && proxyOverride.Length > 0)
            {
                regKey.SetValue(PROXY_OVERRIDE, proxyOverride);
            }
            else
            {
                regKey.DeleteValue(PROXY_OVERRIDE, false);
            }
            
            regKey.Close();

            // 프록시서버사용
            SetProxyEnable(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        public static void SetProxySetting(Proxy proxy)
        {
            string proxyServer = proxy.Server;
            string proxyOverride = null;

            if (proxyServer == null || proxyServer.Length == 0)
            {
                proxyServer = GetProxyServerStringByProtocol(proxy);
            }

            if (proxy.Exceptions.Count > 0)
            {
                proxyOverride = GetProxyOverride(proxy);
            }

            SetProxySetting(proxyServer, proxyOverride);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        private static string GetProxyServerStringByProtocol(Proxy proxy)
        {
            string proxyServer = "";

            string http  = proxy.Http;
            string https = proxy.Https;
            string ftp   = proxy.Ftp;
            string socks = proxy.Socks;

            if (http != null && http.Length > 0)
            {
                proxyServer = AddProxyServers(proxyServer, "http", http);
            }
            if (https != null && https.Length > 0)
            {
                proxyServer = AddProxyServers(proxyServer, "https", https);
            }
            if (ftp != null && ftp.Length > 0)
            {
                proxyServer = AddProxyServers(proxyServer, "ftp", ftp);
            }
            if (socks != null && socks.Length > 0)
            {
                proxyServer = AddProxyServers(proxyServer, "socks", socks);
            }

            return proxyServer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyServer"></param>
        /// <param name="protocol"></param>
        /// <param name="serverByProtocol"></param>
        /// <returns></returns>
        private static string AddProxyServers(string proxyServer, string protocol, string serverByProtocol)
        {
            if (proxyServer.Length > 0)
            {
                proxyServer += ",";
            }

            return proxyServer + protocol + "=" + serverByProtocol;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        private static string GetProxyOverride(Proxy proxy)
        {
            string proxyOverride = "";

            foreach (string host in proxy.Exceptions)
            {
                if (proxyOverride.Length > 0)
                {
                    proxyOverride += ";";
                }

                switch (host)
                {
                    case "local":
                        proxyOverride += "<local>";
                        break;
                    case "loopback":
                        proxyOverride += "<-loopback>";
                        break;
                    default:
                        proxyOverride += host;
                        break;
                }
            }

            return proxyOverride;
        }


        /// <summary>
        /// 
        /// </summary>
        private static RegistryKey OpenRegistryKey()
        {
            // ProxySettingsPerUser 정책이 설정되어 있을경우 (Default)
            String regPath = @"Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(regPath, true);

            return regKey;
        }
    }
}
