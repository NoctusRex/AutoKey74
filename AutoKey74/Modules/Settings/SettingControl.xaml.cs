using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoKey74.Modules.Settings
{
    /// <summary>
    /// Interaction logic for SettingControl.xaml
    /// </summary>
    public partial class SettingControl : UserControl
    {
        public string SettingName { get; private set; }
        public Type Type { get; private set; }
        private object OriginalValue { get; set; }
        public bool ValueChanged
        {
            get
            {
                if (Type == typeof(bool))
                {
                    return CheckBoxInput.IsChecked != (bool)OriginalValue;
                }
                else if (Type == typeof(string))
                {
                    return TextBoxInput.Text != (string)OriginalValue;
                }
                else if (Type.IsEnum)
                {
                    return ComboBoxInput.SelectedItem != OriginalValue;
                }

                return false;
            }
        }
        public object Value { get; private set; }

        public SettingControl(string settingName, object setting)
        {
            InitializeComponent();

            SettingName = settingName;
            Type = setting.GetType();
            OriginalValue = setting;

            LabelSettingName.Content = settingName;
            ImageHasChanged.Visibility = Visibility.Hidden;

            if (Type == typeof(bool))
            {
                TextBoxInput.Visibility = Visibility.Collapsed;
                ComboBoxInput.Visibility = Visibility.Collapsed;
                CheckBoxInput.IsChecked = (bool)setting;
            }
            else if (Type == typeof(string))
            {
                ComboBoxInput.Visibility = Visibility.Collapsed;
                CheckBoxInput.Visibility = Visibility.Collapsed;
                TextBoxInput.Text = (string)setting;
            }
            else if (Type.IsEnum)
            {
                TextBoxInput.Visibility = Visibility.Collapsed;
                CheckBoxInput.Visibility = Visibility.Collapsed;
                ComboBoxInput.ItemsSource = Enum.GetValues(setting.GetType());
                ComboBoxInput.SelectedItem = setting;
            }
            else throw new Exception($"Invalid setting '{SettingName}' of type '{Type.Name}'.");
        }

        public void Reset()
        {
            if (!ValueChanged) return;

            if (Type == typeof(bool))
            {
                 CheckBoxInput.IsChecked = (bool)OriginalValue;
            }
            else if (Type == typeof(string))
            {
                 TextBoxInput.Text = (string)OriginalValue;
            }
            else if (Type.IsEnum)
            {
                 ComboBoxInput.SelectedItem = OriginalValue;
            }

            ShowChangedImage();
        }

        private void TextBoxInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Value = TextBoxInput.Text;
            ShowChangedImage();
        }

        private void CheckBoxInput_Click(object sender, RoutedEventArgs e)
        {
            Value = CheckBoxInput.IsChecked;
            ShowChangedImage();
        }

        private void ComboBoxInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Value = ComboBoxInput.SelectedItem;
            ShowChangedImage();
        }

        private void ShowChangedImage()
        {
            if (ValueChanged)
                ImageHasChanged.Visibility = Visibility.Visible;
            else
                ImageHasChanged.Visibility = Visibility.Hidden;
        }
    }
}
