// Program 2
// CIS 200-10
// Summer 2015
// Due: 5/31/2015
// By: Andreo Sebastiani

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prog2
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
            Application.Run(new Prog2Form());
        }
    }
}
