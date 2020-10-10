using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AutoKey74.Modules.About
{
    /// <summary>
    /// Interaction logic for AboutControl.xaml
    /// </summary>
    public partial class AboutControl : UserControl, IModule
    {
        public AboutControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            LabelVersion.Content = $"Version {GetType().Assembly.GetName().Version}";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
