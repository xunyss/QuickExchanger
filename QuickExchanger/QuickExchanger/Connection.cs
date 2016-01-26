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
    class Connection
    {
        int index;
        string name;
        string alias;
        List<IPSetting> ipsets = new List<IPSetting>();

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
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }
        public List<IPSetting> Ipsets
        {
            get { return ipsets; }
            set { ipsets = value; }
        }
    }
}
