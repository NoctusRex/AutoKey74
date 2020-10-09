using AutoKey74.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AutoKey74
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;
        private bool handleExceptions = false;

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = handleExceptions;

            ErrorWindow.Show(e.Exception);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            CheckAlreadyRunning();

            base.OnStartup(e);
        }

        private void CheckAlreadyRunning()
        {
            mutex = new Mutex(false, "AutoKey74 - 133742069");

            if (!mutex.WaitOne(0, false))
                throw new Exception("The application is already running.");

            handleExceptions = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mutex?.Close();
            mutex?.Dispose();
            mutex = null;

            base.OnExit(e);
        }
    }
}
