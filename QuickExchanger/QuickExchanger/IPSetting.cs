using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickExchanger
{
    /// <summary>
    /// 
    /// </summary>
    class IPSetting
    {
        int connectionIndex;
        int index;
        string name;
        bool addrDHCP;
        string ipaddr;
        string subnet;
        string gateway;

        bool dnsDHCP;
        string dns1;
        string dns2;

        public int ConnectionIndex
        {
            get { return connectionIndex; }
            set { connectionIndex = value; }
        }
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public bool AddrDHCP
        {
            get { return addrDHCP; }
            set { addrDHCP = value; }
        }
        public string Ipaddr
        {
            get { return ipaddr; }
            set { ipaddr = value; }
        }
        public string Subnet
        {
            get { return subnet; }
            set { subnet = value; }
        }
        public string Gateway
        {
            get { return gateway; }
            set { gateway = value; }
        }
        public bool DnsDHCP
        {
            get { return dnsDHCP; }
            set { dnsDHCP = value; }
        }
        public string Dns1
        {
            get { return dns1; }
            set { dns1 = value; }
        }
        public string Dns2
        {
            get { return dns2; }
            set { dns2 = value; }
        }
    }
}
