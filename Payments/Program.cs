using Payments.Models;
using System;
using System.Windows.Forms;

namespace Payments
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Creates a singleton for the views.
            MainViewModel.GetInstance().Login = new Login();
            //Starts application in the log in view.
            Application.Run(MainViewModel.GetInstance().Login);
        }
    }
}