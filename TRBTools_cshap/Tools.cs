using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TRBTools
{
    class Tools
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess,
            IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("Kernel32", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, Keys vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public AllocationProtectEnum AllocationProtect;
            public IntPtr RegionSize;
            public StateEnum State;
            public AllocationProtectEnum Protect;
            public TypeEnum Type;
        }
        public enum AllocationProtectEnum : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        public enum StateEnum : uint
        {
            MEM_COMMIT = 0x1000,
            MEM_FREE = 0x10000,
            MEM_RESERVE = 0x2000
        }
        public enum TypeEnum : uint
        {
            MEM_IMAGE = 0x1000000,
            MEM_MAPPED = 0x40000,
            MEM_PRIVATE = 0x20000
        }
        // privileges
        const int PROCESS_CREATE_THREAD = 0x0002;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_READ = 0x0010;

        // used for memory allocation
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;

        public Process process;
        IntPtr processHandle = IntPtr.Zero;

        IntPtr maxAddress = (IntPtr)0x7fffffffffff;
        public String log = "";

        public Tools()
        {
            process = getProcess();
            if (process == null)
            {
                Log("找不到进程");
                return;
            }
        }
        public IntPtr GetFunAdderssBySearchCode(byte[] searchCode)
        {
            return this.GetAdderssBySearchCode(searchCode, 0, AllocationProtectEnum.PAGE_NOACCESS,
                AllocationProtectEnum.PAGE_EXECUTE_READWRITE,
                StateEnum.MEM_COMMIT,
                TypeEnum.MEM_PRIVATE);
        }
        public IntPtr GetFunAdderssBySearchCode(byte[] searchCode, int ignore)
        {
            return this.GetAdderssBySearchCode(searchCode, ignore, AllocationProtectEnum.PAGE_NOACCESS,
                AllocationProtectEnum.PAGE_EXECUTE_READWRITE,
                StateEnum.MEM_COMMIT,
                TypeEnum.MEM_PRIVATE);
        }
        public IntPtr GetMarkAddressByMarkCode(byte[] searchCode)
        {
            return this.GetAdderssBySearchCode(searchCode, 0, AllocationProtectEnum.PAGE_NOACCESS,
                AllocationProtectEnum.PAGE_EXECUTE_READWRITE,
                StateEnum.MEM_COMMIT,
                TypeEnum.MEM_PRIVATE);
        }

        public IntPtr GetAdderssBySearchCode(byte[] searchCode, int ignore,
            AllocationProtectEnum allocationProtect,
            AllocationProtectEnum protect,
            StateEnum state,
            TypeEnum type)
        {
            IntPtr result = IntPtr.Zero;

            if (process == null || process.Id == 0)
            {
                return IntPtr.Zero;
            }

            if (processHandle == IntPtr.Zero)
            {
                processHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, process.Id);
                if (processHandle == IntPtr.Zero)
                {
                    Log("打开进程句柄失败");
                }
            }

            if (processHandle == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            IntPtr address = IntPtr.Zero;
            Int64 RegionSizeLast = 0;
            int RegionSizeRepeatCount = 0;
            do
            {
                MEMORY_BASIC_INFORMATION m;
                VirtualQueryEx(processHandle, address, out m, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
                if ((m.AllocationProtect == allocationProtect)
                    && (m.Protect == protect)
                    && (m.State == state)
                    && (m.Type == type))
                {
                    int maxBufferSize = 1024 * 500;//500M
                    long memSearchIndex = 0;
                    do
                    {
                        int bufferSize = maxBufferSize;
                        if (maxBufferSize + memSearchIndex > m.RegionSize.ToInt64())
                        {
                            bufferSize = (int)(m.RegionSize.ToInt64() - memSearchIndex);
                        }
                        byte[] buffer = new byte[bufferSize];
                        int byteRead = 0;
                        ReadProcessMemory(processHandle, (IntPtr)(address.ToInt64() + memSearchIndex), buffer, buffer.Length, ref byteRead);
                        int locate = Locate(buffer, searchCode, ignore);
                        if (locate != -1)
                        {
                            result = (IntPtr)(address.ToInt64() + memSearchIndex) + locate;
                        }
                        memSearchIndex += maxBufferSize;
                        memSearchIndex -= searchCode.Length;
                    } while (memSearchIndex + searchCode.Length < m.RegionSize.ToInt64() && result == IntPtr.Zero);

                }
                address = (IntPtr)(address.ToInt64() + m.RegionSize.ToInt64());

                if (m.RegionSize.ToInt64() == RegionSizeLast)// 修复win7 maxAddress=0x7ffffffffff的问题
                {
                    RegionSizeRepeatCount++;
                }
                else
                {
                    RegionSizeRepeatCount = 0;
                    RegionSizeLast = m.RegionSize.ToInt64();
                }
                if (RegionSizeRepeatCount >= 100)
                {
                    Log("RegionSizeRepeatCount大于100");
                }
            } while (address.ToInt64() < maxAddress.ToInt64() && result == IntPtr.Zero && RegionSizeRepeatCount < 100);

            return result;
        }

        public byte[] ReadProcessMemory(IntPtr address, int size)
        {
            int byteRead = 0;
            byte[] buffer = new byte[size];

            ReadProcessMemory(processHandle, address, buffer, size, ref byteRead);
            if (byteRead != size)
            {
                Log("读取数据失败");
                return null;
            }
            return buffer;
        }

        public IntPtr InitializesMemory(IntPtr address, int size)
        {
            IntPtr result = IntPtr.Zero;
            IntPtr tempAddress = IntPtr.Zero;
            MEMORY_BASIC_INFORMATION m;
            do
            {
                VirtualQueryEx(processHandle, tempAddress, out m, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
                if (address.ToInt64() < m.BaseAddress.ToInt64()
                    && m.State == StateEnum.MEM_RESERVE
                    //&& m.Protect == AllocationProtectEnum.PAGE_NOACCESS 
                    && m.RegionSize.ToInt64() > size)
                {
                    break;
                }


                tempAddress = (IntPtr)(tempAddress.ToInt64() + m.RegionSize.ToInt64());

            } while (tempAddress.ToInt64() < maxAddress.ToInt64());

            if (tempAddress.ToInt64() >= maxAddress.ToInt64())
            {
                //MessageBox.Show("没有找到可用的内存区段");
                Log("没有找到可用的内存区段");
                return result;
            }

            int temp = int.MaxValue - 0x2000;
            if (m.RegionSize.ToInt64() < (long)int.MaxValue)
            {
                temp = m.RegionSize.ToInt32() - 0x2000;
            }

            temp = m.RegionSize.ToInt32() - 0x2000;

            return VirtualAllocEx(processHandle, m.BaseAddress + temp, (uint)size, (int)StateEnum.MEM_COMMIT, (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE);
            //return VirtualAllocEx(processHandle, IntPtr.Zero, (uint)size, (int)StateEnum.MEM_COMMIT, (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE);
        }

        //VirtualAllocEx

        public int InjectCode(IntPtr address, byte[] injectCode)
        {
            Console.WriteLine("代码注入地址：{0:x}", address.ToInt64());
            UIntPtr lpNumberOfBytesWritten;
            WriteProcessMemory(processHandle, address, injectCode, (uint)injectCode.Length, out lpNumberOfBytesWritten);
            return (int)lpNumberOfBytesWritten;
        }

        ~Tools()
        {
            if (processHandle != IntPtr.Zero)
            {
                CloseHandle(processHandle);
            }
        }

        public int Locate(byte[] self, byte[] pattern, int ignore)
        {
            if ((self == null)
                || (pattern == null)
                || (self.Length == 0)
                || (pattern.Length == 0)
                || (self.Length < pattern.Length)
                || (ignore == 1))
                return -1;
            for (int i = 0; i < self.Length - pattern.Length; i++)
            {

                if (self[i] == pattern[0])
                {
                    int j = 1;
                    for (; j < pattern.Length; j++)
                    {
                        if ((self[i + j] != pattern[j])
                            && ((1 << j & ignore) == 0)) break;
                    }
                    if (j == pattern.Length) return i;
                }
            }


            return -1;
        }
        public static Process getProcess()
        {
            Process[] processArray = Process.GetProcessesByName("TheyAreBillions");
            if ((processArray == null)
                || (processArray.Length != 1))
            {
                return null;
            }
            return processArray[0];
        }

        public void Log(String str)
        {
            if (log == "")
            {
                log = str;
            }
        }

        public IntPtr BytesToTntPtr(byte[] bytes)
        {
            if (bytes == null)
                return IntPtr.Zero;
            Int64 result = 0;
            for (int i = 0; i < 8; i++)
            {
                result += ((long)bytes[i]) << (i * 8);
            }
            return (IntPtr)result;
        }
    }
}
