﻿using AutoKey74.Modules.AutoKeys;
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
using Unity;

namespace AutoKey74.Modules.ContextMenu
{
    /// <summary>
    /// Interaction logic for ContextMenuControl.xaml
    /// </summary>
    public partial class ContextMenuControl : UserControl, IModule
    {
        [Dependency]
        public MainWindow MainWindow { get; set; }

        private bool expanded;
        public bool Expanded
        {
            get => expanded; 
            set
            {
                expanded = value;
                ChangeMenuState(Expanded);
            }
        }

        public ContextMenuControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            Margin = new Thickness(0, MainWindow.StackPanelModule.Margin.Top, MainWindow.StackPanelModule.Margin.Top, 0);
            MainWindow.GridMain.Children.Add(this);
            Expanded = false;
        }

        private void AutoKeysButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeTo<AutoKeysControl>();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeTo<ContextMenuControl>();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            Expanded = !Expanded;
        }

        private void ChangeMenuState(bool expand)
        {
            if (expand)
                GridContextMenu.Visibility = Visibility.Visible;
            else
                GridContextMenu.Visibility = Visibility.Collapsed;
        }

   
    }
}
