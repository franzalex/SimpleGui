using System;
using System.Windows.Forms;

namespace SimpleGuiApplication
{
    partial class Program
    {
        /// <summary>The simple GUI instance used in the program.</summary>
        private SimpleGui.SimpleGui simpleGui = new SimpleGui.SimpleGui();

        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // create an instance of the Program class and initialize it
            var p = new Program();
            p.Initialize();

            // start the simpleGui form from the Program instance
            p.simpleGui.Start();
        }
    }
}