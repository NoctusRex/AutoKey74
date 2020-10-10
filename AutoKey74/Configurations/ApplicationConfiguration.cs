using System.Windows.Forms;

namespace AutoKey74.Configurations
{
    public class ApplicationConfiguration
    {
        public bool StartMinimized { get; set; } = false;
        public Keys StartStopHotKey { get; set; } = Keys.Pause;
    }
}
