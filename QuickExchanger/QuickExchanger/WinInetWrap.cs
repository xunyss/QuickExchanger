using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace QuickExchanger
{
    class WinInetWrap
    {
        /// <summary>
        /// 
        /// </summary>
        private const int INTERNET_OPTION_REFRESH = 37;
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hInternet"></param>
        /// <param name="dwOption"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="lpdwBufferLength"></param>
        /// <returns></returns>
        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);




        /// <summary>
        /// 
        /// </summary>
        public static void RefreshInternetSettings()
        {
            /*
             * 특정 윈도우즈 환경에서 InternetSetOption 함수 수행 이후
             * INTERNET_OPTION_REFRESH 이 정상적으로 동작하지 않는 현상 발생.
             * 대부분의 경우엔 문제 없음.
             * 
             * 이 경우도 다른프로세스를 별도로 띠우지 않고 해결할 수 있도록 프로그램 수정이 필요함.
             * 아래는 임시방편 -> 프록시 설정 변경시 마다 새로운 프로세스를 사용하여 인터정설정 갱신.
             */
            try
            {
                Process.Start("InetSetting.exe", "/refresh");
            }
            catch {}
            

            /*
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
            */
        }
    }
}
