using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace AutoKey74.Hotkeys
{
    public class GlobalHotkey : IDisposable
    {
        public event EventHandler Triggered;

        private int Modifier { get; set; }
        private int Key { get; set; }
        private IntPtr Hwnd { get; set; }
        private int Id { get; set; }
        private HwndSource Source { get; set; }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public GlobalHotkey(int modifier, Keys key, Window window, Action<object, EventArgs> triggeredEvent)
        {
            Modifier = modifier;
            Key = (int)key;
            Hwnd = new WindowInteropHelper(window).Handle;
            Source = HwndSource.FromHwnd(Hwnd);
            Source.AddHook(HwndHook);
            Id = GetHashCode();
            Triggered += new EventHandler(triggeredEvent);
        }

        public bool Register() => RegisterHotKey(Hwnd, Id, Modifier, Key);

        public bool Register(int modifier, Keys key, Window window)
        {
            Modifier = modifier;
            Key = (int)key;
            Hwnd = new WindowInteropHelper(window).Handle;
            Source = HwndSource.FromHwnd(Hwnd);
            Source.AddHook(HwndHook);
            Id = GetHashCode();

            return Register();
        }

        public bool Unregister()
        {
            Source.RemoveHook(HwndHook);
            Source = null;
            return UnregisterHotKey(Hwnd, Id);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg != HotkeyConstants.WM_HOTKEY_MSG_ID) return IntPtr.Zero;
            if (wParam.ToInt32() != Id) return IntPtr.Zero;

            Triggered(this, new EventArgs());
            handled = true;

            return IntPtr.Zero;
        }

        public override int GetHashCode() => Modifier ^ Key ^ Hwnd.ToInt32();

        public void Dispose() => Unregister();
    }
}
