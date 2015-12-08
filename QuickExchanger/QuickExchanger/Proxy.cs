using System.Collections.Generic;

namespace QuickExchanger
{
    /// <summary>
    /// 
    /// </summary>
    class Proxy
    {
        int index;

        string name;
        string pac;
        string server;
        string http;
        string https;
        string ftp;
        string socks;
        List<string> exceptions = new List<string>();

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
        public string Pac
        {
            get { return pac; }
            set { pac = value; }
        }
        public string Server
        {
            get { return server; }
            set { server = value; }
        }
        public string Http
        {
            get { return http; }
            set { http = value; }
        }
        public string Https
        {
            get { return https; }
            set { https = value; }
        }
        public string Ftp
        {
            get { return ftp; }
            set { ftp = value; }
        }
        public string Socks
        {
            get { return socks; }
            set { socks = value; }
        }
        public List<string> Exceptions
        {
            get { return exceptions; }
            set { exceptions = value; }
        }
    }
}
