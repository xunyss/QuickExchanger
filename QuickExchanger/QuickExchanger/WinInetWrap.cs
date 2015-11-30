using System;
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
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);




        /// <summary>
        /// 
        /// </summary>
        public static void RefreshInternetSettings()
        {
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }
    }
}
