using AutoKey74.Configurations;
using AutoKey74.Models;
using AutoKey74.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using Unity;

namespace AutoKey74.Modules.AutoKeys
{
    /// <summary>
    /// Interaction logic for AutoKeysControl.xaml
    /// </summary>
    public partial class AutoKeysControl : UserControl, IModule
    {
        [Dependency]
        public AutoKeyConfiguration AutoKeyConfiguration { get; set; }

        [Dependency]
        public AutoKeyEditControl AutoKeyEditModule { get; set; }

        [Dependency]
        public Pathmanager Pathmanager { get; set; }

        public AutoKeysControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            StackPanelAutoKeys.Children.Clear();
            StackPanelAutoKeys.Children.Add(new AddAutoKeyControl(AddAutoKey));

            if (AutoKeyConfiguration.AutoKeys is null) return;

            foreach (AutoKey autoKey in AutoKeyConfiguration.AutoKeys)
                StackPanelAutoKeys.Children.Add(new AutoKeyDisplayControl(autoKey, EditAutoKey, RemoveAutoKey, ToggleAutoKey));

        }

        private void AddAutoKey()
        {
            AutoKey autoKey = AutoKeyEditModule.Add();
            if (autoKey is null) return;

            if (AutoKeyConfiguration.AutoKeys is null) AutoKeyConfiguration.AutoKeys = new AutoKey[] { };

            AutoKeyConfiguration.AutoKeys = AutoKeyConfiguration.AutoKeys.Append(autoKey);
            Save();
        }

        private void EditAutoKey(AutoKey autoKey)
        {
            AutoKeyEditModule.Edit(autoKey);
            Save();
        }

        private void RemoveAutoKey(AutoKey autoKey)
        {
            AutoKeyConfiguration.AutoKeys = AutoKeyConfiguration.AutoKeys.Except(new AutoKey[] { autoKey });
            Save();
        }

        private void ToggleAutoKey(AutoKey autoKey)
        {
            Save();
        }

        private void Save()
        {
            File.WriteAllText(Pathmanager.AutoKeyConfiguration, JsonConvert.SerializeObject(AutoKeyConfiguration, Formatting.Indented));
            Initialize();
        }

    }
}
