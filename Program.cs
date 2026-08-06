// ================================================
// PROGRAM.cs
using System;
using System.Windows.Forms;

namespace ScheduledDiscordRPC
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Catch anything unhandled so this background/tray app can't just silently vanish on
            // an unexpected exception, leaving the user confused about why their Discord status
            // stopped updating. Instead, show what happened and exit cleanly.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => ReportFatalError(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e.ExceptionObject is Exception ex) ReportFatalError(ex);
            };

            // To customize application configuration such as high DPI settings,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        private static void ReportFatalError(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Unhandled exception: {ex}");
            MessageBox.Show(
                $"An unexpected error occurred and Scheduled Discord RPC needs to close:\n\n{ex.Message}",
                "Scheduled Discord RPC - Fatal Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
