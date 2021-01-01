using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRBTools
{
    class AutoSave
    {
        static volatile bool saveDoing = false;
        //static byte[] searchCode = new byte[] { 0x00, 0xb0, 0x67, 0xff, 0xb5, 0xfa, 0x7f, 0x00, 0x00, 0x01, 0x01, 0x00 };
        static byte[] markCode = new byte[] { 0xfe, 0xe8, 0xe9, 0xe8, 0xe9, 0xe8, 0xe9, 0xe8, 0xfe, 0xe8, 0xe9, 0xe8, 0xe9, 0xe8, 0xe9, 0xe8 };//16
        static byte[] funSearchCode = new byte[]
        {
            0x48,0x8b,0x00,
            0x83,0x78,0x18,0x00,
            0x7e,0x08,
            0x48,0x83,0xc4,0x20,
            0x5b,
            0x5e,
            0x5f,
            0xc3,
            0x8b,0x86,0x84,0x02,0x00,0x00,
            0x85,0xc0,
            0x75,0x21,
        };
        static byte[] injectCodeTemplatePart1 = new byte[]
        {
            0xe9,0x00,0x00,0x00,0x00,0x90
        };
        /*
        48 B8 2A008439FC7F0000 - mov rax,00007FFC3984002A
        8B 18                 - mov ebx,[rax]
        81 FB 01000000        - cmp ebx,00000001
        75 0B                 - jne 7FFC3984001F
        C7 00 00000000        - mov [rax],00000000
        E9 FC8ADCFF           - jmp 7FFC39608B1B
        8B 86 84020000        - mov eax,[rsi+00000284]
        E9 118ADCFF           - jmp 7FFC39608A3B
         */
        static byte[] injectCodeTemplatePart2 = new byte[]
        {
            0x48,0xb8,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x8b,0x18,
            0x81,0xfb,0x01,0x00,0x00,0x00,
            0x75,0x0b,
            0xc7,0x00,0x00,0x00,0x00,0x00,
            0xe9,0x00,0x00,0x00,0x00,// 跳转到保存函数
            0x8b,0x86,0x84,0x02,0x00,0x00,
            0xe9,0x00,0x00,0x00,0x00,// 跳转到原函数
        };
        static int injectMemSize = 16 + 42 + 8;
        public static Form form1;
        static byte[] makeSaveMark = new byte[] { 0x01, 0x00, 0x00, 0x00 };//1
        public static void HotKey(IntPtr hWnd, bool register)
        {
            if (register)
            {
                bool result = Tools.RegisterHotKey(hWnd, 1, 0, Keys.F3);
                if (result == false)
                {
                    //MessageBox.Show("注册快捷键失败");
                    form1.SetTextBox1Value("注册快捷键失败");
                }
            }
            else
            {
                Tools.UnregisterHotKey(hWnd, 1);
            }

        }

        public static void Run()
        {
            if (saveDoing) return;
            Thread th = new Thread(MakeSave);
            th.Start();
        }
        public static void MakeSave()
        {
            form1.SetTextBox1Value("Doing.....");
            saveDoing = true;
            Tools tools = new Tools();

            IntPtr injectMemBaseAddress = tools.GetMarkAddressByMarkCode(markCode);
            if (tools.log != "")
            {
                form1.SetTextBox1Value(tools.log);
                saveDoing = false;
                return;
            }

            if (injectMemBaseAddress == IntPtr.Zero)
            {
                injectMemBaseAddress = InjectFunCode(tools, injectMemBaseAddress);
                if (injectMemBaseAddress == IntPtr.Zero)
                {
                    form1.SetTextBox1Value(tools.log);
                    saveDoing = false;
                    return;
                }
            }
            int injectSucessSize = tools.InjectCode(injectMemBaseAddress+16+42, makeSaveMark);
            if (injectSucessSize != makeSaveMark.Length)
            {
                tools.Log("注入存档标记失败");
                form1.SetTextBox1Value(tools.log);
                saveDoing = false;
                return;
            }

            Thread.Sleep(8000);
            BackupFile();
            form1.SetTextBox1Value("Done.");
            saveDoing = false;
        }
        public static IntPtr InjectFunCode(Tools tools, IntPtr injectMemBaseAddress)
        {
            IntPtr funAddress = tools.GetFunAdderssBySearchCode(funSearchCode);
            if (funAddress == IntPtr.Zero)
            {
                tools.Log("定位函数失败");
                return IntPtr.Zero;
            }
            funAddress += 17;
            injectMemBaseAddress = tools.InitializesMemory(funAddress, injectMemSize);

            if (injectMemBaseAddress == IntPtr.Zero)
            {
                tools.Log("初始化内存空间失败");
                return IntPtr.Zero;
            }

            if (Math.Abs(injectMemBaseAddress.ToInt64() - funAddress.ToInt64()) > (long)int.MaxValue)
            {
                tools.Log("函数位置和新内存空间距离太大");
                return IntPtr.Zero;
            }

            byte[] injectCodePart1 = injectCodeTemplatePart1;
            long rva = injectMemBaseAddress.ToInt64() + 16 - funAddress.ToInt64() - 5;
            for (int i = 0; i < 4; i++)
            {
                injectCodePart1[i + 1] = (byte)((rva >> (i * 8)) & 255);
            }
            byte[] injectCodePart2 = injectCodeTemplatePart2;
            rva = funAddress.ToInt64() + 6 - injectMemBaseAddress.ToInt64() - 16 - 37 - 5;
            for (int i = 0; i < 4; i++)
            {
                injectCodePart2[i + 38] = (byte)((rva >> (i * 8)) & 255);
            }
            rva = funAddress.ToInt64() + 0xe6 - injectMemBaseAddress.ToInt64() - 16 - 26 - 5;
            for (int i = 0; i < 4; i++)
            {
                injectCodePart2[i + 27] = (byte)((rva >> (i * 8)) & 255);
            }
            long dataAddress = injectMemBaseAddress.ToInt64() + 16 + 42;
            for (int i = 0; i < 8; i++)
            {
                injectCodePart2[i + 2] = (byte)((dataAddress >> (i * 8)) & 255);
            }
            int injectSucessSize = tools.InjectCode(injectMemBaseAddress + 16, injectCodePart2);
            if (injectSucessSize != injectCodePart2.Length)
            {
                tools.Log("注入新函数代码失败");
                return IntPtr.Zero;
            }

            injectSucessSize = tools.InjectCode(funAddress, injectCodePart1);
            if (injectSucessSize != injectCodePart1.Length)
            {
                tools.Log("替换旧函数代码失败");
                return IntPtr.Zero;
            }

            injectSucessSize = tools.InjectCode(injectMemBaseAddress, markCode);
            if (injectSucessSize != markCode.Length)
            {
                tools.Log("注入锚点失败");
                return IntPtr.Zero;
            }
            return injectMemBaseAddress;
        }

        public static void BackupFile()
        {
            String basePath = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders").GetValue("Personal").ToString();
            if (basePath[basePath.Length - 1] == '\\') basePath = basePath.Substring(0, basePath.Length - 1);
            string path = basePath + "\\My Games\\They Are Billions\\Saves";

            string backupPath = path + "\\TRBToolsBackup";

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            backupPath = backupPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss");

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            string[] zxcheckArray = Directory.GetFiles(path, "*.zxcheck");
            string[] zxsavArray = Directory.GetFiles(path, "*.zxsav");
            foreach (string zxcheck in zxcheckArray)
            {
                string fName = zxcheck.Substring(path.Length + 1);
                File.Copy(Path.Combine(path, fName), Path.Combine(backupPath, fName));
            }
            foreach (string zxsav in zxsavArray)
            {
                string fName = zxsav.Substring(path.Length + 1);
                File.Copy(Path.Combine(path, fName), Path.Combine(backupPath, fName));
            }
        }
    }
}
