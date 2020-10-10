using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoKey74.Modules.AutoKeys
{
    /// <summary>
    /// Interaction logic for AddAutoKeyControl.xaml
    /// </summary>
    public partial class AddAutoKeyControl : UserControl
    {
        private Action OnClick { get; set; }

        public AddAutoKeyControl(Action onClick)
        {
            InitializeComponent();
            OnClick = onClick;
        }

        private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnClick.Invoke();
        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BorderBrush = Brushes.Orange;
            LabelAdd.Foreground = Brushes.Orange;
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BorderBrush = Brushes.Black;
            LabelAdd.Foreground = Brushes.Black;
        }
    }
}
