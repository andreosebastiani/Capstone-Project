// Program 3
// CIS 200-10
// Summer 2015
// Due: 6/05/2015
// By: Andreo Sebastiani

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prog3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Prog3Form());
        }
    }
}
