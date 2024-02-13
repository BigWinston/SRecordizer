using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SRecordizer
{
    static class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main(string[] args)
        {
            List<string> fileNames = new List<string>();

            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    Console.WriteLine("Argument: " + arg);
                    fileNames.Add(arg);
                }
            }

            Application.Run(new SRecordizer(fileNames.ToArray()));
        }

    }
}
