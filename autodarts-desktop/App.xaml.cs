using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Linq;



namespace autodarts_desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var current_process = Process.GetCurrentProcess();
            var other_process = Process.GetProcessesByName(current_process.ProcessName).FirstOrDefault(p => p.Id != current_process.Id);

            if (other_process != null && other_process.MainWindowHandle != IntPtr.Zero)
            {
                if (IsIconic(other_process.MainWindowHandle))
                {
                    ShowWindow(other_process.MainWindowHandle, SW_RESTORE);
                }
                SetForegroundWindow(other_process.MainWindowHandle);
                Shutdown();
            }
        }

        [DllImport("user32")]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32")]
        static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        const int SW_RESTORE = 9;

        [DllImport("user32")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
