using System;
using System.Windows.Forms;

namespace InetSetting
{
    static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        const string REFRESH = "/refresh";


        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                processOption(REFRESH);
            }
            else
            {
                processOption(args[0]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        static void processOption(string option)
        {
            switch (option)
            {
                case REFRESH:
                    WinInetWrap.RefreshInternetSettings();
                    break;

                default:
                    break;
            }
        }
    }
}
