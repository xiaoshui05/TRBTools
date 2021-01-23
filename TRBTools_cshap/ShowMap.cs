using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TRBTools
{
    class ShowMap
    {
        private readonly byte[] MarkCode = new byte[] 
        {
            0x48,0x83,0xc4,0x28,
            0x5e,
            0x5f,
            0xc3,
            0x80,0xbe,0x20,0x02,0x00,0x00,0x00,
            0x0f,0x84,0x15,0x01,0x00,0x00,
        };
        private readonly byte[] OriginalCode = new byte[]
        {
            0x80,0xbe,0x20,0x02,0x00,0x00,0x00,
            0x0f,0x84,0x15,0x01,0x00,0x00,
        };
        private readonly byte[] NewCode = new byte[]
        {
            0xc6,0x86,0x20,0x02,0x00,0x00,0x01,
            0xe9,0x16,0x01,0x00,0x00,
            0x90,
        };
        private readonly string LogHead = "全图: ";

        public void Run()
        {
            Tools tools = new Tools();
            IntPtr funAddr = tools.GetFunAdderssBySearchCode(MarkCode);

            if(funAddr == IntPtr.Zero)
            {
                tools.Log("定位全图函数失败");
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }

            int sucessSize = tools.InjectCode(funAddr + 7, this.NewCode);
            if(sucessSize != this.NewCode.Length)
            {
                tools.Log("注入新代码失败");
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }
            Thread.Sleep(1000);
            sucessSize = tools.InjectCode(funAddr + 7, this.OriginalCode);
            if (sucessSize != this.OriginalCode.Length)
            {
                tools.Log("还原代码失败");
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }
            Data.form1.SetTextBox1Value(LogHead + "Done.");
        }
    }
}
