using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TRBTools
{
    class ShowBlood
    {
        [DllImport("user32.dll", EntryPoint = "PostMessageW")]
        public static extern bool PostMessage(
           IntPtr hWnd,        // 信息发往的窗口的句柄
           int Msg,            // 消息ID
           int wParam,         // 参数1
           int lParam            // 参数2
        );
        private static readonly int WM_KEYDOWN = 0x0100;
        private static readonly int WM_KEYUP = 0x0101;
        private static readonly int VK_ALT = 0x12;

        public static bool enabled = false;
        public static void Run()
        {
            if (enabled)
            {
                enabled = false;
            }
            else
            {
                enabled = true;
                Thread th = new Thread(ALT_DOWN);
                th.Start();
            }
        }
        private static void ALT_DOWN()
        {
            Data.form1.SetTextBox1Value("");
            Tools tools = new Tools();
            if(tools.process == null || tools.process.MainWindowHandle == IntPtr.Zero)
            {
                Data.form1.SetTextBox1Value("显血：未找游戏进程");
                enabled = false;
                return;
            }
            IntPtr hWnd = tools.process.MainWindowHandle;
            while (enabled)
            {
                PostMessage(hWnd, WM_KEYDOWN, VK_ALT, 0);
                Thread.Sleep(1000);
            }
            PostMessage(hWnd, WM_KEYUP, VK_ALT, 0);
        }
    }
}
