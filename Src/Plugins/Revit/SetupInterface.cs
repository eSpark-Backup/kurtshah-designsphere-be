using Autodesk.Revit.UI;
using Revit.core.Commands.DataPanelCommand;
using Revit.core.Commands.GeneralPanel;
using Revit.ui.Revit;

namespace Revit
{
 
    /// <summary>
    /// Setup whole plugins interface with tabs, panels, buttons,...
    /// </summary>
    public class SetupInterface
    {
        #region constructor

        /// <summary>
        /// Default constructor.
        /// Initializes a new instance of the <see cref="SetupInterface"/> class.
        /// </summary>
        public SetupInterface()
        {

        }

        #endregion

        #region public methods

        /// <summary>
        /// Initializes all interface elements on custom created Revit tab.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Initialize(UIControlledApplication app)
        {
            // Create ribbon tab.
            string tabName = "DesignSphere";
            app.CreateRibbonTab(tabName);

            // Create the ribbon panels.
            var dataCommandsPanel = app.CreateRibbonPanel(tabName, "Data");
            var generalCommandsPanel = app.CreateRibbonPanel(tabName, "General");

            #region data

            // Populate button data model.
            var dataPanelButton = new RevitPushButtonDataModel
            {
                Label = "Send",
                Panel = dataCommandsPanel,
                Tooltip = "Sync door data with external API",
                CommandNamespacePath = SyncDoorDataCommand.GetPath(),
                IconImageName = "send_image.png",
            };

            // Create button from provided data.
            var dataPanelBtn = RevitPushButton.Create(dataPanelButton);

            #endregion

            #region General

            var generalPanelButton = new RevitPushButtonDataModel
            {
                Label = "Account",
                Panel = generalCommandsPanel,
                Tooltip = "Manage account settings",
                CommandNamespacePath = AccountCommand.GetPath(),
                IconImageName = "account_general.png",
            };

            // Create button from provided data.
            var GeneralPanelButton = RevitPushButton.Create(generalPanelButton);
            #endregion
        }

        #endregion
    }
}
