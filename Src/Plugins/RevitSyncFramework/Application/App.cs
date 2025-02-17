using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace RevitSyncFramework.Application
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Define tab and panel names
            string tabName = "DesignSphere";
            string dataPanelName = "Data";
            string generalPanelName = "General";

            // Ensure the custom tab exists
            application.CreateRibbonTab(tabName);

            // Create the Data panel
            RibbonPanel dataPanel = application.CreateRibbonPanel(tabName, dataPanelName);

            // Add "Send" button to Data panel
            AddPushButton(dataPanel, "SyncDoorData", "Send",
                "RevitSyncFramework.Commands.SyncDoorDataCommand",
                "Resources/send_image.png",
                "Sync door data with external API");

            // Add "Receive" button to Data panel
            AddPushButton(dataPanel, "ReceiveDoorData", "Retrieve",
                "RevitSyncFramework.Commands.ReceiveProjectCommand",
                "Resources/retrieve_image.png",
                "Retrieve Projects");

            // Create the General panel
            RibbonPanel generalPanel = application.CreateRibbonPanel(tabName, generalPanelName);

            // Add "Account" button to General panel
            AddPushButton(generalPanel, "AccountSettings", "Account",
                "RevitSyncFramework.Commands.AccountCommand",
                "Resources/account_general.png",
                "Manage account settings");

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private void AddPushButton(RibbonPanel panel, string name, string text, string commandNamespace, string imageName, string tooltip)
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string imagePath = Path.Combine(Path.GetDirectoryName(assemblyPath), imageName);

            PushButtonData buttonData = new PushButtonData(name, text, assemblyPath, commandNamespace)
            {
                ToolTip = tooltip,
                LargeImage = LoadBitmapFromFile(imagePath)
            };

            panel.AddItem(buttonData);
        }

        private BitmapImage LoadBitmapFromFile(string filePath)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
    }
}

