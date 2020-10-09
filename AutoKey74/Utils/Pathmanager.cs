using System.IO;
using System.Reflection;

namespace AutoKey74.Utils
{
    public class Pathmanager
    {
        public string StartupDirectory { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }

        public string ConfigurationDirectory
        {
            get
            {
                string configPath = Path.Combine(StartupDirectory, "Configuration");
                if (!File.Exists(configPath)) { Directory.CreateDirectory(configPath); }
                return configPath;
            }
        }

        public string CombineConfigurationPath(string file) => Path.Combine(ConfigurationDirectory, file);

    }
}
