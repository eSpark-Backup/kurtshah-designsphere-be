using System.Diagnostics;
using System.Windows;

namespace Revit.WPFUI.Components.UI
{
    /// <summary>
    /// Interaction logic for AccountDialog.xaml
    /// </summary>
    public partial class AccountDialog : Window
    {
        private const string SignInUrl = "https://your-authentication-page-url.com";

        public AccountDialog()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = SignInUrl,
                UseShellExecute = true
            });
        }
    }
}



