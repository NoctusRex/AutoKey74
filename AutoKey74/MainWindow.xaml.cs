using AutoKey74.Configurations;
using AutoKey74.Hotkeys;
using AutoKey74.Models;
using AutoKey74.Modules.AutoKeys;
using AutoKey74.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Unity;

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

        private List<AutoKeyHandler> AutoKeyHandlers { get; set; }

        private bool AutoKeysEnabled { get; set; }

        #endregion

        #region Dependency Injection

        [Dependency]
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        [Dependency]
        public AutoKeyConfiguration AutoKeyConfiguration { get; set; }

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
            AutoKeyHandlers = new List<AutoKeyHandler>();
            AutoKeysEnabled = false;
            ImageWarning.Visibility = Visibility.Hidden;

            foreach (AutoKey autoKey in AutoKeyConfiguration.AutoKeys)
                AddAutoKey(autoKey);

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

        public void ChangeTo<T>() where T : UIElement
        {
            StackPanelModule.Children.Clear();
            StackPanelModule.Children.Add(UnityContainer.Resolve<T>());
        }

        public void AddAutoKey(AutoKey autoKey)
        {
            AutoKeyHandler handler = new AutoKeyHandler(autoKey);
            AutoKeyHandlers.Add(handler);

            if (AutoKeysEnabled) handler.Start();
        }

        public void RemoveAutoKey(AutoKey autoKey)
        {
            AutoKeyHandler handler = AutoKeyHandlers.First(x => x.AutoKey == autoKey);
            handler.Stop();
            AutoKeyHandlers.Remove(handler);
        }

        #endregion

        #region Events

        private void StartStopHotkeyPressed(object sender, EventArgs e)
        {
            AutoKeysEnabled = !AutoKeysEnabled;

            if (AutoKeysEnabled)
            {
                ImageWarning.Visibility = Visibility.Visible;
                foreach (AutoKeyHandler handler in AutoKeyHandlers)
                    handler.Start();
            }
            else
            {
                ImageWarning.Visibility = Visibility.Hidden;
                foreach (AutoKeyHandler handler in AutoKeyHandlers)
                    handler.Stop();
            }

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
            foreach (AutoKeyHandler handler in AutoKeyHandlers)
                handler.Dispose();
            AutoKeyHandlers = null;
        }

        #endregion

    }
}
