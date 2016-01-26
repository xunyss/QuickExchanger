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
        /// 연결목록
        /// </summary>
        private List<Connection> connList;

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
            connList = confObj.connList;
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
            // 연결리스트
            int connIdx = 0;
            foreach (Connection conn in connList)
            {
                string name = conn.Alias;
                if (name == null || name.Length == 0)
                {
                    name = conn.Name;
                }

                foreach (IPSetting ipsetting in conn.Ipsets)
                {
                    menu.Items.Add(name + " - " + ipsetting.Name);
                }

                if (connList.Count - 1 > connIdx++)
                {
                    menu.Items.Add(new ToolStripSeparator());
                }
            }

            // 프록시서버리스트
            menu.Items.Add(new ToolStripSeparator());
            foreach (Proxy proxy in proxyList)
            {
                menu.Items.Add(proxy.Name).Tag = proxy.Index;
            }

            // 기본메뉴
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Activate Proxy Server").Name = "M00";   // 프록시서버사용여부
            menu.Items.Add("Clear Proxy Settings").Name = "M01";    // 프록시셋팅제거
            menu.Items.Add("Settings").Name = "M02";
            menu.Items.Add("About").Name = "M03";
            menu.Items.Add("Exit").Name = "M04";

            // 이벤트핸들러
            menu.ItemClicked += OnClickMenuItem;


            // 메뉴상태초기화
            GetMenuByName("M00").Checked = InternetSetting.GetProxyEnable();
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

                default:
                    DoExchangeProxy(menuItem);
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
        /// <param name="tag"></param>
        private void DoExchangeProxy(ToolStripMenuItem menuItem)
        {
            foreach (ToolStripItem item in menu.Items)
            {
                if (item.Tag != null)
                {
                    ((ToolStripMenuItem)item).Checked = false;
                }
            }

            object tag = menuItem.Tag;
            if (tag != null)
            {
                // 프록시서버사용여부체크
                GetMenuByName("M00").Checked = true;

                // 클릭한프록시서버체크
                menuItem.Checked = true;

                int proxyIndex = (int)tag;
                Proxy proxy = proxyList[proxyIndex];

                InternetSetting.SetProxySetting(proxy);
            }
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
