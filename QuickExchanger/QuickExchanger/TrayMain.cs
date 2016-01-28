using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickExchanger
{
    /// <summary>
    /// 
    /// </summary>
    class TrayMain
    {
        /// <summary>
        /// 
        /// </summary>
        protected class MenuKey
        {
            public const string IPSETTING = "menukey.ipsetting";
            public const string PROXY     = "menukey.proxy";

            public string type;
            public int index;

            public MenuKey(string type, int index)
            {
                this.type = type;
                this.index = index;
            }
        }

        /// <summary>
        /// 연결주소목록
        /// </summary>
        private List<IPSetting> ipsetList;

        /// <summary>
        /// 프록시서버목록
        /// </summary>
        private List<Proxy> proxyList;

        /// <summary>
        /// 트레이
        /// </summary>
        private NotifyIcon tray;
        /// <summary>
        /// 메뉴
        /// </summary>
        private ContextMenuStrip menu;


        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            // 설정파일 로드
            Config.ConfigObject confObj = Config.LoadConfigXML();
            ipsetList = confObj.ipsetList;
            proxyList = confObj.proxyList;

            // 메뉴생성
            menu = new ContextMenuStrip();
            CreateMenu();

            // 트레이생성
            tray = new NotifyIcon();
            tray.Icon = Properties.Resources.TrayIcon;
            tray.Text = Properties.Resources.TrayToolTip;
            tray.ContextMenuStrip = menu;
            tray.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateMenu()
        {
            // 메뉴-연결리스트
            int prevConnIndex = -1, connIndex = 0;
            foreach (IPSetting ipset in ipsetList)
            {
                connIndex = ipset.Conn.Index;
                string connName = ipset.Conn.Alias;
                if (connName == null || connName.Length == 0)
                {
                    connName = ipset.Conn.Name;
                }

                if (prevConnIndex > -1 && prevConnIndex != connIndex)
                {
                    menu.Items.Add(new ToolStripSeparator());
                }

                menu.Items.Add(connName + " - " + ipset.Name)
                    .Tag = new MenuKey(MenuKey.IPSETTING, ipset.Index);

                prevConnIndex = connIndex;
            }

            // 메뉴-프록시서버리스트
            menu.Items.Add(new ToolStripSeparator());
            foreach (Proxy proxy in proxyList)
            {
                menu.Items.Add(proxy.Name)
                    .Tag = new MenuKey(MenuKey.PROXY, proxy.Index);
            }

            // 기본메뉴
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Activate Proxy Server").Name = "M00";   // 프록시서버사용여부
            menu.Items.Add("Clear Proxy Settings").Name = "M01";    // 프록시셋팅제거
            menu.Items.Add("Settings").Name = "M02";
            menu.Items.Add("About").Name = "M03";
            menu.Items.Add("Exit").Name = "M04";

            // 메뉴상태초기화
            GetMenuByName("M00").Checked = InternetSetting.GetProxyEnable();
            // 주소메뉴상태초기화
            {
                foreach (ToolStripItem item in menu.Items)
                {
                    MenuKey menukey = (MenuKey)item.Tag;
                    if (menukey != null)
                    {
                        if (menukey.type == MenuKey.IPSETTING)
                        {
                            IPSetting ipset = ipsetList[menukey.index];
                            Connection conn = ipset.Conn;
                            string ipaddr = ConnectionSetting.getCurrentIP(conn.Name);

                            if ((ipaddr == ConnectionSetting.DHCP && ipset.AddrDHCP) ||
                                (ipaddr != null && ipaddr.Length > 0 && ipaddr == ipset.Ipaddr))
                            {
                                ((ToolStripMenuItem)item).Checked = true;
                            }
                        }
                    }
                }
            }


            // 이벤트핸들러
            menu.ItemClicked += OnClickMenuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickMenuItem(object sender, ToolStripItemClickedEventArgs e)
        {
        //  ContextMenuStrip menu = (ContextMenuStrip)sender;   // == this.menu
            if (e.ClickedItem.GetType() != typeof(ToolStripMenuItem))
            {
                return;
            }

            ToolStripMenuItem menuItem = (ToolStripMenuItem)e.ClickedItem;

            switch (menuItem.Name)
            {
                // 기본메뉴
                case "M00":     // Use Proxy Server
                    DoToggleProxyEnable(menuItem);
                    break;
                case "M01":     // Clear Internet Settings
                    DoClearProxySetting();
                    break;
                case "M02":     // Settings
                    new Settings().Show();
                    break;
                case "M03":     // About
                    new About().Show();
                    break;
                case "M04":     // About
                    Exit();
                    break;
                
                //
                default:
                    MenuKey menukey = (MenuKey)menuItem.Tag;
                    if (MenuKey.IPSETTING.Equals(menukey.type))
                    {
                        DoConnectionIPSetting(menuItem);
                    }
                    else if (MenuKey.PROXY.Equals(menukey.type))
                    {
                        DoExchangeProxy(menuItem);
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private ToolStripMenuItem GetMenuByName(string name)
        {
            foreach (ToolStripItem item in menu.Items)
            {
                if (name.Equals(item.Name))
                {
                    return ((ToolStripMenuItem)item);
                }
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuItem"></param>
        private void DoConnectionIPSetting(ToolStripMenuItem menuItem)
        {
            foreach (ToolStripItem item in menu.Items)
            {
                if (item.Tag != null)
                {
                    ((ToolStripMenuItem)item).Checked = false;
                }
            }

            MenuKey menukey = (MenuKey)menuItem.Tag;
            if (menukey != null)
            {
                // 클릭한연결주소설정체크
                menuItem.Checked = true;

                int ipsetIndex = menukey.index;
                IPSetting ipset = ipsetList[ipsetIndex];
                Connection conn = ipset.Conn;

                ConnectionSetting.SetIPSetting(conn, ipset);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuItem"></param>
        private void DoExchangeProxy(ToolStripMenuItem menuItem)
        {
            foreach (ToolStripItem item in menu.Items)
            {
                if (item.Tag != null)
                {
                    ((ToolStripMenuItem)item).Checked = false;
                }
            }

            MenuKey menukey = (MenuKey)menuItem.Tag;
            if (menukey != null)
            {
                // 프록시서버사용여부체크
                GetMenuByName("M00").Checked = true;

                // 클릭한프록시서버체크
                menuItem.Checked = true;

                int proxyIndex = menukey.index;
                Proxy proxy = proxyList[proxyIndex];

                InternetSetting.SetProxySetting(proxy);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuItem"></param>
        private void DoToggleProxyEnable(ToolStripMenuItem menuItem)
        {
            bool enable = !menuItem.Checked;
            menuItem.Checked = enable;

            InternetSetting.SetProxyEnable(enable);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoClearProxySetting()
        {
            foreach (ToolStripItem item in menu.Items)
            {
                if (item.Tag != null)
                {
                    ((ToolStripMenuItem)item).Checked = false;
                }
            }
            // 프록시서버사용여부체크해제
            GetMenuByName("M00").Checked = false;

            InternetSetting.ClearProxySetting();
        }


        /// <summary>
        /// 
        /// </summary>
        private void Exit()
        {
            menu.Dispose();
            tray.Dispose();
            Application.Exit();
        }
    }
}
