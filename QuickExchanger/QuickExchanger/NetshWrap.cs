using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace QuickExchanger
{
    /// <summary>
    /// 
    /// </summary>
    class NetshWrap
    {
        /// <summary>
        /// 
        /// </summary>
        const string CRLF = "\r\n";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connName"></param>
        /// <param name="addrDHCP"></param>
        /// <param name="dnsDHCP"></param>
        /// <param name="ipaddr"></param>
        /// <param name="subnet"></param>
        /// <param name="gateway"></param>
        /// <param name="dns1"></param>
        /// <param name="dns2"></param>
        public static void ExecChangeIP(string connName,
            bool addrDHCP, bool dnsDHCP,
            string ipaddr, string subnet, string gateway,
            string dns1, string dns2)
        {
            // 임시쉘파일명
            string tmpCmdName = "_tmp"
                + DateTime.Now.ToFileTime()
                + ".cmd";

            // 임시쉘파일생성
            using (StreamWriter file = new StreamWriter(tmpCmdName,
                false,
                Encoding.GetEncoding("euc-kr")))
            {
                string cl = "";

                if (addrDHCP)
                {
                    cl += "netsh interface ip set address name=\"" + connName + "\" "
                        + "source=dhcp";
                }
                else
                {
                    cl += "netsh interface ip set address name=\"" + connName + "\" "
                       + "source=static " + ipaddr + " " + subnet + " " + gateway + " 1";
                }
                cl += CRLF;

                if (dnsDHCP)
                {
                    cl += "netsh interface ip set dns \"" + connName + "\" dhcp";
                }
                else
                {
                    cl += "netsh interface ip delete dns \"" + connName + "\" all";
                    cl += CRLF;

                    if (dns1 != null && dns1.Length > 0)
                    {
                        cl += "netsh interface ip add dns \"" + connName + "\" " + dns1;
                        cl += CRLF;
                    }
                    if (dns2 != null && dns2.Length > 0)
                    {
                        cl += "netsh interface ip add dns \"" + connName + "\" " + dns2;
                        cl += CRLF;
                    }
                }

                file.WriteLine(cl);
                file.Close();
            }

            // 쉘실행(최대30초대기)
            Process.Start(tmpCmdName)
                .WaitForExit(30 * 1000);

            // 임시쉘파일삭제
            File.Delete(tmpCmdName);
        }
    }
}
