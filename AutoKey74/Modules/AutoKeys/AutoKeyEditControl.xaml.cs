using AutoKey74.Models;
using AutoKey74.Modules.ContextMenu;
using AutoKey74.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Unity;
using WindowsInput.Native;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;

namespace AutoKey74.Modules.AutoKeys
{
    /// <summary>
    /// Interaction logic for AutoKeyEditControl.xaml
    /// </summary>
    public partial class AutoKeyEditControl : UserControl, IModule
    {
        [Dependency]
        public MainWindow MainWindow { get; set; }

        [Dependency]
        public ContextMenuControl ContextMenuModul { get; set; }

        public bool IsOpen { get; private set; }

        private System.Timers.Timer ApplicationTimer { get; set; }

        private AutoKey AutoKey { get; set; }

        public AutoKeyEditControl()
        {
            InitializeComponent();
        }

        public AutoKey Add()
        {
            AutoKey = null;
            LabelTitle.Content = "Add";
            Initialize();
            MainWindow.ChangeTo<AutoKeyEditControl>();

            ApplicationTimer.Start();
            ContextMenuModul.Expanded = false;
            ContextMenuModul.IsEnabled = false;
            IsOpen = true;

            while (IsOpen) { DoEvents(); }

            ContextMenuModul.IsEnabled = true;

            MainWindow.ChangeTo<AutoKeysControl>();
            ResetForm();
            return AutoKey;
        }

        public void Edit(AutoKey autoKey)
        {
            LabelTitle.Content = "Edit";
            Initialize();
            MainWindow.ChangeTo<AutoKeyEditControl>();

            ApplicationTimer.Start();
            ContextMenuModul.Expanded = false;
            ContextMenuModul.IsEnabled = false;
            IsOpen = true;

            AutoKey = autoKey;
            TextBoxApplication.Text = AutoKey?.Application;
            TextBoxDuration.Text = AutoKey?.Duration.ToString();
            TextBoxIntervall.Text = AutoKey?.Intervall.ToString();
            CheckBoxEnabled.IsChecked = AutoKey?.Enabled;
            ComboBoxKeyMode.SelectedItem = AutoKey?.KeyMode;
            ComboBoxKeys.SelectedItem = VirtualKeyCode.NONAME;

            foreach (VirtualKeyCode key in AutoKey.Keys)
                AddKeyButton(key);

            while (IsOpen) { DoEvents(); }

            ContextMenuModul.IsEnabled = true;

            MainWindow.ChangeTo<AutoKeysControl>();
            ResetForm();
        }

        public void Initialize()
        {
            ComboBoxKeyMode.ItemsSource = Enum.GetValues(typeof(KeyModes));
            ComboBoxKeyMode.SelectedItem = KeyModes.Click;
            TextBoxDuration.IsEnabled = false;

            ComboBoxKeys.ItemsSource = Enum.GetValues(typeof(VirtualKeyCode));
            ComboBoxKeys.SelectedItem = VirtualKeyCode.NONAME;
            ApplicationTimer = new System.Timers.Timer(500);
            ApplicationTimer.Elapsed += UpdateApplication;
        }

        private void UpdateApplication(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                LabelCurrentApplication.Content = $"'{WinApi.GetCurrentWindowTitle()}'";
                if (WinApi.GetCurrentWindowTitle() == TextBoxApplication.Text)
                    LabelCurrentApplication.Foreground = Brushes.Green;
                else
                    LabelCurrentApplication.Foreground = Brushes.Black;
            });
        }

        private void TextBoxIntervall_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex(@"^\d+$").IsMatch(e.Text);
        }

        private void DoEvents()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            }
            catch
            {
                // When the window is closed a NullReference exception is thrown
            }

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxIntervall.Text)) return;
            if (StackPanelKey.Children.Count <= 0) return;
            List<VirtualKeyCode> keys = new List<VirtualKeyCode>();

            if (AutoKey is null)
            {

                AutoKey = new AutoKey()
                {
                    Application = TextBoxApplication.Text,
                    Duration = string.IsNullOrEmpty(TextBoxDuration.Text) ? 0 : int.Parse(TextBoxDuration.Text),
                    Intervall = int.Parse(TextBoxIntervall.Text),
                    Enabled = (bool)CheckBoxEnabled.IsChecked,
                    KeyMode = (KeyModes)ComboBoxKeyMode.SelectedItem,
                };
            }
            else
            {
                AutoKey.Application = TextBoxApplication.Text;
                AutoKey.Duration = string.IsNullOrEmpty(TextBoxDuration.Text) ? 0 : int.Parse(TextBoxDuration.Text);
                AutoKey.Intervall = int.Parse(TextBoxIntervall.Text);
                AutoKey.Enabled = (bool)CheckBoxEnabled.IsChecked;
                AutoKey.KeyMode = (KeyModes)ComboBoxKeyMode.SelectedItem;
            }

            foreach (Button button in StackPanelKey.Children)
                keys.Add((VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), button.Content.ToString()));

            AutoKey.Keys = keys;

            IsOpen = false;
        }

        private void ComboBoxKeyMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((KeyModes)ComboBoxKeyMode.SelectedItem == KeyModes.Hold)
                TextBoxDuration.IsEnabled = true;
            else
            {
                TextBoxDuration.IsEnabled = false;
                TextBoxDuration.Text = "";
            }
        }

        private void ComboBoxKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((VirtualKeyCode)ComboBoxKeys.SelectedItem == VirtualKeyCode.NONAME) return;

            AddKeyButton((VirtualKeyCode)ComboBoxKeys.SelectedItem);

            ComboBoxKeys.SelectedItem = VirtualKeyCode.NONAME;
        }

        private void AddKeyButton(VirtualKeyCode key)
        {
            foreach (Button b in StackPanelKey.Children)
                if (b.Content.ToString() == key.ToString()) return;

            Button button = new Button()
            {
                Content = key.ToString()
            };

            button.Click += (s, eArgs) =>
            {
                StackPanelKey.Children.Remove(button);
            };

            StackPanelKey.Children.Add(button);

        }

        private void ResetForm()
        {
            TextBoxApplication.Text = "";
            TextBoxDuration.Text = "";
            TextBoxIntervall.Text = "";
            CheckBoxEnabled.IsChecked = false;
            ComboBoxKeyMode.SelectedItem = KeyModes.Click;
            ComboBoxKeys.SelectedItem = Keys.None;
            StackPanelKey.Children.Clear();
            ApplicationTimer?.Stop();
            ApplicationTimer?.Dispose();
        }

    }
}
