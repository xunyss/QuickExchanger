
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace QuickExchanger
{
    /// <summary>
    /// 
    /// </summary>
    class ConnectionSetting
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DHCP = "__DHCP_ADDRESS__";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="ipset"></param>
        public static void SetIPSetting(Connection conn, IPSetting ipset)
        {

            NetshWrap.ExecChangeIP(conn.Name, ipset.AddrDHCP, ipset.DnsDHCP,
                ipset.Ipaddr, ipset.Subnet, ipset.Gateway, ipset.Dns1, ipset.Dns2);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static string getCurrentIP(string connName)
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (connName == adapter.Name)
                {
                    IPInterfaceProperties ipProps = adapter.GetIPProperties();
                    IPv4InterfaceProperties ipv4Props = ipProps.GetIPv4Properties();

                    if (ipv4Props.IsDhcpEnabled)
                    {
                        return DHCP;
                    }
                    else
                    {
                        foreach (UnicastIPAddressInformation ipInfo in ipProps.UnicastAddresses)
                        {
                            if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return ipInfo.Address.ToString();
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
