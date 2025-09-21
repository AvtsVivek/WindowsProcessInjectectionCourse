using System;
using System.Runtime.InteropServices;

namespace UnmanagedThreadFuncPointer
{
    class Program
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, IntPtr lpStartAddress,
            IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);

        [DllImport("kernel32.dll")]
        private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SimpleDelegate();

        public static void MyThreadFunction()
        {
            Console.WriteLine("Hello from unmanaged thread!");
        }

        static void Main(string[] args)
        {
            SimpleDelegate del = new SimpleDelegate(MyThreadFunction);
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(del);
            UInt32 threadId = 0;
            IntPtr hThread = CreateThread(0, 0, funcPtr, IntPtr.Zero, 0, ref threadId);
            WaitForSingleObject(hThread, 0xFFFFFFFF);
        }
    }
}