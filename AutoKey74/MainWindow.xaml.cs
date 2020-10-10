using AutoKey74.Configurations;
using AutoKey74.Hotkeys;
using AutoKey74.Modules.AutoKeys;
using AutoKey74.Modules.ContextMenu;
using System;
using System.Windows;
using System.Windows.Controls;
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

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

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
            ChangeTo<AutoKeysControl>();
        }

        private void InitializeHotkeys()
        {
            StartStopHotkey = new GlobalHotkey(HotkeyConstants.NOMOD, ApplicationConfiguration.StartStopHotKey, this, StartStopHotkeyPressed);
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

        #region Functions

        public void ChangeTo<T>() where T: UIElement
        {
            StackPanelModule.Children.Clear();
            StackPanelModule.Children.Add(UnityContainer.Resolve<T>());
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
            if (NotifyIcon is null) return;

            NotifyIcon.Visible = true;
            ShowInTaskbar = false;
            Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MinimizeToIcon();
            e.Cancel = true;
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
