using AutoKey74.Configurations;
using AutoKey74.Utils;
using Unity;

namespace AutoKey74
{
    public sealed class UnityConfiguration
    {
        public static void Register(IUnityContainer container)
        {
            RegisterWindows(container);
            RegisterConfiguration(container);
        }

        private static void RegisterWindows(IUnityContainer container)
        {
            container.RegisterSingleton<MainWindow>();
        }

        private static void RegisterConfiguration(IUnityContainer container)
        {
            Pathmanager pathmanager = new Pathmanager();
            container.RegisterInstance(pathmanager);

            container.RegisterInstance(typeof(ApplicationConfiguration), ConfigurationLoader.Load<ApplicationConfiguration>(pathmanager.CombineConfigurationPath("application.configuration.json")));
        }

    }
}
