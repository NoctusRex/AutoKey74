using AutoKey74.Models;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using WindowsInput;
using WindowsInput.Native;
using Timer = System.Timers.Timer;

namespace AutoKey74.Utils
{
    public class AutoKeyHandler : IDisposable
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public AutoKey AutoKey { get; private set; }
        private Timer Timer { get; set; }
        private InputSimulator Simulator { get; set; }
        private bool IsRunning { get; set; }

        public AutoKeyHandler(AutoKey autoKey)
        {
            AutoKey = autoKey;
            Timer = new Timer(autoKey.Intervall)
            {
                AutoReset = true,
                Enabled = false
            };
            Timer.Elapsed += Elapsed;
            Simulator = new InputSimulator();
        }

        public void Start() => Timer.Start();

        public void Stop()
        {
            Timer.Stop();
            while (IsRunning) continue;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            IsRunning = true;
            Timer.Interval = AutoKey.Intervall;

            if (!string.IsNullOrEmpty(AutoKey.Application) && GetCurrentWindowTitle(GetForegroundWindow()) != AutoKey.Application)
            {
                IsRunning = false;
                return;
            }
            if (!AutoKey.Enabled)
            {
                IsRunning = false;
                return;
            }

            if (AutoKey.KeyMode == KeyModes.Click)
                HandleClick();
            else
                HandleHold();

            IsRunning = false;
        }

        private void HandleClick()
        {
            if (AutoKey.Keys.Count() == 1)
                Simulator.Keyboard.KeyPress(AutoKey.Keys.Single());
            else
                Simulator.Keyboard.ModifiedKeyStroke(AutoKey.Keys.First(), AutoKey.Keys.Skip(1));
        }

        private void HandleHold()
        {
            if (AutoKey.Keys.Count() == 1)
            {
                Simulator.Keyboard.KeyDown(AutoKey.Keys.Single());
                Thread.Sleep(AutoKey.Duration);
                Simulator.Keyboard.KeyUp(AutoKey.Keys.Single());
            }
            else
            {
                foreach (VirtualKeyCode key in AutoKey.Keys)
                    Simulator.Keyboard.KeyDown(key);

                Thread.Sleep(AutoKey.Duration);

                foreach (VirtualKeyCode key in AutoKey.Keys)
                    Simulator.Keyboard.KeyUp(key);
            }
        }

        private string GetCurrentWindowTitle(IntPtr handle)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public void Dispose()
        {
            if (Timer != null)
            {
                Stop();
                Timer.Dispose();
            }
            Simulator = null;
        }
    }
}
