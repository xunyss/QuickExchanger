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
            // 프록시서버리스트 설정파일 로드
            proxyList = Config.LoadConfigXML();

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
            // 프록시서버사용여부
            menu.Items.Add("Use Proxy Server").Name = "M00";
            ((ToolStripMenuItem)menu.Items[0]).Checked = InternetSetting.GetProxyEnable();

            // 프록시서버리스트
            menu.Items.Add(new ToolStripSeparator());
            foreach (Proxy proxy in proxyList)
            {
                menu.Items.Add(proxy.Name).Tag = proxy.Index;
            }

            // 기본메뉴
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Clear Internet Settings").Name = "M01";
            menu.Items.Add("Settings").Name = "M02";
            menu.Items.Add("About").Name = "M03";
            menu.Items.Add("Exit").Name = "M04";

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
            ((ToolStripMenuItem)menu.Items[0]).Checked = false;

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
                ((ToolStripMenuItem)menu.Items[0]).Checked = true;
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
