using System;
using System.Windows.Forms;

namespace QuickExchanger
{
    /// <summary>
    /// 
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // TrayMain 시작
            new TrayMain().Run();

            Application.Run();
        }
    }
}
