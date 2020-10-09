using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace AutoKey74.Windows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        /// <summary>
        /// Shows the error window as dialog
        /// </summary>
        /// <param name="ex"></param>
        public static void Show(Exception ex) => new ErrorWindow(ex).ShowDialog();

        public ErrorWindow()
        {
            InitializeComponent();
        }

        public ErrorWindow(Exception ex) : this()
        {
            TextBlockError.Text = ex.Message;
            TextBlockStackTrace.Text = ex.StackTrace;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
