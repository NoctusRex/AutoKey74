using AutoKey74.Models;
using AutoKey74.Modules.ContextMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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

        private AutoKey AutoKey { get; set; }

        public AutoKeyEditControl()
        {
            InitializeComponent();
        }

        public AutoKey Add()
        {
            AutoKey = null;
            LabelTitle.Content = "Add";
            MainWindow.ChangeTo<AutoKeyEditControl>();

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
            MainWindow.ChangeTo<AutoKeyEditControl>();

            ContextMenuModul.Expanded = false;
            ContextMenuModul.IsEnabled = false;
            IsOpen = true;

            TextBoxApplication.Text = AutoKey?.Application;
            TextBoxDuration.Text = AutoKey?.Duration.ToString();
            TextBoxIntervall.Text = AutoKey?.Intervall.ToString();
            CheckBoxEnabled.IsChecked = AutoKey?.Enabled;
            ComboBoxKeyMode.SelectedItem = AutoKey?.KeyMode;
            ComboBoxKeys.SelectedItem = Keys.None;

            foreach (Keys key in autoKey.Keys)
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

            ComboBoxKeys.ItemsSource = Enum.GetValues(typeof(Keys));
            ComboBoxKeys.SelectedItem = Keys.None;
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

            List<Keys> keys = new List<Keys>();

            AutoKey = new AutoKey()
            {
                Application = TextBoxApplication.Text,
                Duration = string.IsNullOrEmpty(TextBoxDuration.Text) ? 0 : int.Parse(TextBoxDuration.Text),
                Intervall = int.Parse(TextBoxIntervall.Text),
                Enabled = CheckBoxEnabled.IsEnabled,
                KeyMode = (KeyModes)ComboBoxKeyMode.SelectedItem,
            };

            foreach (Button button in StackPanelKey.Children)
                keys.Add((Keys)Enum.Parse(typeof(Keys), button.Content.ToString()));

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
            if ((Keys)ComboBoxKeys.SelectedItem == Keys.None) return;

            AddKeyButton((Keys)ComboBoxKeys.SelectedItem);

            ComboBoxKeys.SelectedItem = Keys.None;
        }

        private void AddKeyButton(Keys key)
        {
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
        }

    }
}
