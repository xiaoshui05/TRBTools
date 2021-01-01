using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRBTools
{
    class Speed
    {
        byte[] funCode = new byte[] {
            0x84,0xC0,
            0x74,0x07,
            0xC4,0xE1,0x78,0x57,0xC0,
            0xEB,0x09,
            0xC4,0xE1,0x7B,0x10,0x05,0x14,0x00,0x00,0x00,
            0xC4,0xE1,0x7B,0x11,0x86,0xD8,0x01,0x00,0x00,
            0x48,0x83,0xC4,0x20,
            0x5E,
            0xC3,
        };
        private Form form1;
        private double speed;
        public Speed(Form form1,double speed)
        {
            this.form1 = form1;
            this.speed = speed;
        }
        public void Run()
        {
            Tools tools = new Tools();
            IntPtr funCodeAddress = tools.GetFunAdderssBySearchCode(funCode);
            if(funCodeAddress == IntPtr.Zero)
            {
                tools.Log("定位函数失败");
                form1.SetTextBox1Value("速度:" + tools.log);
                return;
            }
            int injectSucessSize = tools.InjectCode(funCodeAddress+0x28, BitConverter.GetBytes(speed));
            if (injectSucessSize != 8)
            {
                tools.Log("注入代码失败");
                form1.SetTextBox1Value("速度:" + tools.log);
            }
        }
    }
}
