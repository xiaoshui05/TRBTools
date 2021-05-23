using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRBTools
{
    class MouseLeftClick
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
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
                Thread th = new Thread(LeftClick);
                th.Start();
            }
        }
        public static void LeftClick()
        {
            while (enabled)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Thread.Sleep(1);
            }
        }
    }
}
