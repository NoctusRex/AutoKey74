using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoKey74.Models
{
    public class AutoKey
    {
        public string Application { get; set; }
        public bool Enabled { get; set; }
        public IEnumerable<Keys> Keys { get; set; }
        public KeyModes KeyMode { get; set; }
        public int Intervall { get; set; }
        public int Duration { get; set; }
    }
}
