using Newtonsoft.Json;
using System;
using System.IO;

namespace AutoKey74.Configurations
{
    public static class ConfigurationLoader
    {
        public static T Load<T>(string path)
        {
            if (!File.Exists(path))
                File.WriteAllText(path, JsonConvert.SerializeObject(Activator.CreateInstance(typeof(T)), Formatting.Indented));

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
    }
}
