using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Revit.WPFUI.Components.UI
{
    public partial class UserCodeDialog : Window
    {
        public string VerificationUri { get; set; }
        public string UserCode { get; set; }

        public UserCodeDialog(string userCode, string verificationUri)
        {
            InitializeComponent();
            this.UserCode = userCode;
            this.VerificationUri = verificationUri;
            this.DataContext = this;  // Bind data to the UI
        }

        // Open the verification URL when clicked
        private void VerificationUri_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Uri.IsWellFormedUriString(VerificationUri, UriKind.Absolute))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = VerificationUri,
                    UseShellExecute = true
                });
            }
        }
    }
}
