using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRBTools
{
    class FinalSwarm
    {
        private readonly byte[] MarkCode = new byte[] // ZX.ZXLevelState #=z8XSdBFo=()
        {
            0x48,0x8b,0x10,
            0x48,0x85,0xd2,
            0x75,0x03,
            0x33,0xc0,
            0xc3,
            0x48,0x8b,0x42,0x18,
            0xc3,
        };
        private readonly byte[] BytesIntZero = new byte[]
        {
            0,0,0,0
        };
        private readonly byte[] BytesNullPtr = new byte[]
        {
            0,0,0,0,0,0,0,0,
        };
        private string LogHead = "最后一波:";
        public bool extend = false;
        public bool gameWinMark = false;

        public void Run()
        {
            if (gameWinMark)
            {
                LogHead = "一键胜利：";
            }
            Tools tools = new Tools();
            IntPtr funCodeAddress = tools.GetFunAdderssBySearchCode(MarkCode);
            if (funCodeAddress == IntPtr.Zero)
            {
                tools.Log("定位函数失败");
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }
            byte[] tempbytes = tools.ReadProcessMemory(funCodeAddress - 0x8, 8);

            if (tempbytes == null)
            {
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }
            IntPtr tempIntPtr = tools.BytesToTntPtr(tempbytes);
            if (tempIntPtr == IntPtr.Zero)
            {
                tools.log = "读取rax失败";
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }

            tempbytes = tools.ReadProcessMemory(tempIntPtr, 8);
            tempIntPtr = tools.BytesToTntPtr(tempbytes);
            if (tempIntPtr == IntPtr.Zero)
            {
                tools.log = "读取rax的值失败";
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }

            tempbytes = tools.ReadProcessMemory(tempIntPtr + 0x18, 8);
            tempIntPtr = tools.BytesToTntPtr(tempbytes);
            if (tempIntPtr == IntPtr.Zero)
            {
                tools.log = "读取[rax]+0x18的值失败";
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }

            tempbytes = tools.ReadProcessMemory(tempIntPtr + 0xC8, 8);
            tempIntPtr = tools.BytesToTntPtr(tempbytes);
            if (tempIntPtr == IntPtr.Zero)
            {
                tools.log = "读取LevelEvents的地址失败";
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }

            tempbytes = tools.ReadProcessMemory(tempIntPtr + 0x8, 8);
            tempIntPtr = tools.BytesToTntPtr(tempbytes);
            if (tempIntPtr == IntPtr.Zero)
            {
                tools.log = "读取LevelEvents的地址失败2";
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }

            IntPtr levelEventsListAddr = tempIntPtr;

            tempbytes = tools.ReadProcessMemory(levelEventsListAddr + 8, 8);
            if (tempbytes == null)
            {
                tools.log = "读取List大小失败";
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }
            long size = 0;
            for (int i = 0; i < tempbytes.Length; i++)
            {
                size += ((long)tempbytes[i]) << (i * 8);
            }
            if (size == 0)
            {
                tools.log = "LevelEvents的大小为0";
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }

            List<IntPtr> LevelEventAddrList = new List<IntPtr>();

            for (int i = 0; i < size; i++)
            {
                byte[] levelEventBytes = tools.ReadProcessMemory(levelEventsListAddr + (i + 2) * 8, 8);
                IntPtr levelEventAddr = tools.BytesToTntPtr(levelEventBytes);
                if (tempIntPtr == IntPtr.Zero)
                {
                    tools.log = "读取第" + (i + 1) + "个失败";
                    Data.form1.SetTextBox1Value(LogHead + tools.log);
                    continue;
                }
                // FinalSwarm
                byte[] finalSwarm = tools.ReadProcessMemory(levelEventAddr + 0xae, 1);
                if (finalSwarm == null)
                {
                    tools.log = "读取FinalSwarm的值失败";
                    Data.form1.SetTextBox1Value(LogHead + tools.log);
                    continue;
                }

                // 事件发生游戏胜利
                byte[] gameWon = tools.ReadProcessMemory(levelEventAddr + 0xad, 1);
                if (gameWon == null)
                {
                    tools.log = "读GameWon的值失败";
                    Data.form1.SetTextBox1Value(LogHead + tools.log);
                    continue;
                }

                if (gameWinMark)
                {
                    if (gameWon[0] != 1) continue;
                }
                else
                {
                    if (finalSwarm[0] == 0 || gameWon[0] == 1) continue;
                }


                LevelEventAddrList.Add(levelEventAddr);
            }

            if (LevelEventAddrList.Count == 0)
            {
                tools.log = "没有找到FinalSwarm==true的选项";
                Data.form1.SetTextBox1Value(LogHead + tools.log);
                return;
            }

            foreach (IntPtr levelEventAddr in LevelEventAddrList)
            {
                if (extend)
                {
                    tools.InjectCode(levelEventAddr + 0x28, BytesNullPtr);// 清空规则
                }
                tools.InjectCode(levelEventAddr + 0x6c, BytesIntZero);
                tools.InjectCode(levelEventAddr + 0x80, BytesIntZero);
            }



            Data.form1.SetTextBox1Value(LogHead + "Done");
        }
    }
}
