using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimplePInvokeMessageBox
{
    class Program
    {
        // Here we import our DLL that holds our `MessageBox` Function.
        [DllImport("user32.dll")]
        public static extern int MessageBox(IntPtr hWnd, string lpText, 
            string lpCaption, uint uType);

        static void Main(string[] args)
        {
            // Now we can call it using our required parameters.
            MessageBox(IntPtr.Zero, "Hey there!", "Hello from P/Invoke!", 0);
        }
    }
}
