using System;
using System.Runtime.InteropServices;

namespace VirtualAllocExOne
{
    class Program
    {
        // Allocate memory as read/write, not executable
        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAlloc(IntPtr lpAddress, int dwSize, UInt32 flAllocationType, UInt32 flProtect);

        // Use a managed thread instead of CreateThread
        static void Main(string[] args)
        {
            int size = 1024;
            IntPtr mem = VirtualAlloc(IntPtr.Zero, size, (UInt32)TYPE.MEM_COMMIT, (UInt32)PROTECTION.PAGE_READWRITE);
            Console.WriteLine($"Allocated {size} bytes at {mem}");

            Thread t = new Thread(() =>
            {
                Console.WriteLine("Hello from a new thread!");
            });
            t.Start();
            t.Join();
        }

        public enum TYPE
        {
            MEM_COMMIT = 0x00001000
        }

        public enum PROTECTION
        {
            PAGE_READWRITE = 0x04
        }
    }
}