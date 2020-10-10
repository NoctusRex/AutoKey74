using AutoKey74.Configurations;
using AutoKey74.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using Unity;

namespace AutoKey74.Modules.Settings
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl, IModule
    {
        [Dependency]
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        [Dependency]
        public Pathmanager Pathmanager { get; set; }

        public SettingsControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            StackPanelSettings.Children.Clear();
            LabelSaved.Visibility = Visibility.Hidden;

            foreach (PropertyInfo propertyInfo in ApplicationConfiguration.GetType().GetProperties())
                StackPanelSettings.Children.Add(new SettingControl(propertyInfo.Name, propertyInfo.GetValue(ApplicationConfiguration)));
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            bool save = false;

            foreach (SettingControl setting in StackPanelSettings.Children)
            {
                if (!setting.ValueChanged) continue;

                foreach (PropertyInfo propertyInfo in ApplicationConfiguration.GetType().GetProperties())
                {
                    if (setting.SettingName != propertyInfo.Name) continue;

                    propertyInfo.SetValue(ApplicationConfiguration, setting.Value);
                    save = true;
                }
            }

            if (!save) return;

            File.WriteAllText(Pathmanager.ApplicationConfiguration, JsonConvert.SerializeObject(ApplicationConfiguration, Formatting.Indented));
            Initialize();
            LabelSaved.Visibility = Visibility.Visible;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (SettingControl setting in StackPanelSettings.Children)
                setting.Reset();
        }
    }
}
