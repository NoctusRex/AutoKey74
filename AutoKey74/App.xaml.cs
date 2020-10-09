using AutoKey74.Modules;
using AutoKey74.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Unity;
using Unity.Lifetime;

namespace AutoKey74
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;
        private bool handleExceptions = false;

        internal static IUnityContainer UnityContainer { get; } = new UnityContainer();

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = handleExceptions;

            ErrorWindow.Show(e.Exception);
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            try
            {
                CheckAlreadyRunning();
                UnityConfiguration.Register(UnityContainer);

                foreach (IModule module in UnityContainer.Registrations.
                    Where(x => typeof(IModule).IsAssignableFrom(x.RegisteredType)).
                    Select(x => UnityContainer.Resolve(x.RegisteredType)))
                    module.Initialize();

                UnityContainer.Resolve<MainWindow>().Show();
            }
            catch (Exception ex)
            {
                ErrorWindow.Show(ex);
                Application.Current.Shutdown();
            }
            finally
            {
                handleExceptions = true;
            }

        }

        private void CheckAlreadyRunning()
        {
            mutex = new Mutex(false, "AutoKey74 - 133742069");

            if (!mutex.WaitOne(0, false))
                throw new Exception("The application is already running.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mutex?.Close();
            mutex?.Dispose();
            mutex = null;

            UnityContainer.Dispose();

            base.OnExit(e);
        }
    }
}
