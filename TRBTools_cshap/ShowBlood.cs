using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
        private static readonly int VK_ALT = 0x12;

        public static void Run()
        {
            Tools tools = new Tools();
            if (tools.processHandle == IntPtr.Zero)
            {
                Data.form1.SetTextBox1Value("显血：" + tools.log);
                return;
            }
            PostMessage(tools.process.MainWindowHandle, WM_KEYDOWN, VK_ALT, 0);
        }
    }
}
