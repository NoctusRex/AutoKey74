﻿using System.Collections.Generic;
using WindowsInput.Native;

namespace AutoKey74.Models
{
    public class AutoKey
    {
        public string Application { get; set; }
        public bool Enabled { get; set; }
        public IEnumerable<VirtualKeyCode> Keys { get; set; }
        public KeyModes KeyMode { get; set; }
        public int Intervall { get; set; }
        public int Duration { get; set; }

        public override string ToString() => $"{Application} - {KeyMode} - {Intervall}ms - {Duration}ms - {string.Join(", ", Keys)}";

    }
}
