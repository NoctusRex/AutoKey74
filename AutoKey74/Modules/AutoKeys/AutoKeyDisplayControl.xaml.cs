using AutoKey74.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AutoKey74.Modules.AutoKeys
{
    /// <summary>
    /// Interaction logic for AutoKeyDisplayControl.xaml
    /// </summary>
    public partial class AutoKeyDisplayControl : UserControl
    {
        private Action<AutoKey> EditAutoKey { get; set; }
        private Action<AutoKey> RemoveAutoKey { get; set; }
        private Action<AutoKey> ToggleAutoKey { get; set; }
        public AutoKey AutoKey { get; private set; }

        public AutoKeyDisplayControl(AutoKey autoKey, Action<AutoKey> editAutoKey, Action<AutoKey> removeAutoKey, Action<AutoKey> toggleAutoKey)
        {
            InitializeComponent();
            EditAutoKey = editAutoKey;
            RemoveAutoKey = removeAutoKey;
            ToggleAutoKey = toggleAutoKey;
            AutoKey = autoKey;
            LabelInfo.Content = AutoKey.ToString();
            ButtonEdit.IsEnabled = !AutoKey.Enabled;
            ButtonDelete.IsEnabled = !AutoKey.Enabled;
            CheckBoxEnabled.IsChecked = AutoKey.Enabled;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            RemoveAutoKey.Invoke(AutoKey);
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            EditAutoKey(AutoKey);
        }

        private void CheckBoxEnabled_Click(object sender, RoutedEventArgs e)
        {
            ButtonEdit.IsEnabled = !(bool)CheckBoxEnabled.IsChecked;
            ButtonDelete.IsEnabled = !(bool)CheckBoxEnabled.IsChecked;
            AutoKey.Enabled = (bool)CheckBoxEnabled.IsChecked;
            ToggleAutoKey(AutoKey);
        }
    }
}
