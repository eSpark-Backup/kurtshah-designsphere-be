using Autodesk.Revit.UI;
using Revit.WPFUI.Components.UI;
using System;
using System.Diagnostics;

namespace RevitSyncFramework.Commands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AccountCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            string signInUrl = "https://your-authentication-page-url.com";

            // Open the default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = signInUrl,
                UseShellExecute = true
            });


            var dialog = new AccountDialog();
            dialog.ShowDialog();
            return Result.Succeeded;
        }
    }
}
