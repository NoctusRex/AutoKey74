using AutoKey74.Configurations;
using AutoKey74.Hotkeys;
using System;
using System.Windows;
using System.Windows.Forms;
using Unity;
using MessageBox = System.Windows.MessageBox;

namespace AutoKey74
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {

        #region Properties

        private GlobalHotkey StartStopHotkey { get; set; }
        private NotifyIcon NotifyIcon { get; set; }

        #endregion


        #region Dependency Injection
        
        [Dependency]
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        #endregion

        #region Initialize

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeHotkeys();
            InitializeNotifyIcon();
        }

        private void InitializeHotkeys()
        {
            StartStopHotkey = new GlobalHotkey(HotkeyConstants.NOMOD, Keys.Pause, this, StartStopHotkeyPressed);
            StartStopHotkey.Register();
        }

        private void InitializeNotifyIcon()
        {
            NotifyIcon = new NotifyIcon()
            {
                Visible = ApplicationConfiguration.StartMinimized,
                BalloonTipText = "Open AutoKey74",
                BalloonTipTitle = "AutoKey74",
                Text = "AutoKey74",
                Icon = Properties.Resources.Icon
            };
            NotifyIcon.DoubleClick += NotifyIconDoubleClick;

            if (ApplicationConfiguration.StartMinimized)
            {
                ShowInTaskbar = false;
                Hide();
            }
        }

        #endregion

        #region Events

        private void StartStopHotkeyPressed(object sender, EventArgs e)
        {

        }

        private void NotifyIconDoubleClick(object sender, EventArgs e)
        {
            NotifyIcon.Visible = false;
            ShowInTaskbar = true;
            Show();
            WindowState = WindowState.Normal;
            BringIntoView();
        }

        private void MinimizeToIcon()
        {
            NotifyIcon.Visible = true;
            ShowInTaskbar = false;
            Hide();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) MinimizeToIcon();
        }

        #endregion

        #region Disposable Support

        public void Dispose()
        {
            StartStopHotkey.Dispose();
        }

        #endregion

    }
}
