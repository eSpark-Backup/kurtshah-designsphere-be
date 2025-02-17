using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using RevitSyncFramework.Models;
using RevitSyncFramework.Services;

namespace RevitSyncFramework.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class SyncDoorDataCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get the active Revit document
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;

            // Get project name and file name
            var projectName = doc.Title; // This is the project name
            var fileName = doc.PathName; // This is the full file path

            var collector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType(); // Only actual doors, not door types

            // Prepare a list to store door data
            var doorDataList = collector
                .OfType<FamilyInstance>()
                .Select(door => new DoorData
                {
                    DoorId = door.Id.ToString(),
                    DoorType = door.Symbol.Name,
                    FloorName = doc.GetElement(door.LevelId)?.Name,
                    DoorWidth = door.LookupParameter("Width") != null ? door.LookupParameter("Width").AsValueString() : "N/A",
                    DoorHeight = door.LookupParameter("Height") != null ? door.LookupParameter("Height").AsValueString() : "N/A",
                    Location = new Location
                    {
                        X = (door.Location as LocationPoint)?.Point.X ?? 0,
                        Y = (door.Location as LocationPoint)?.Point.Y ?? 0,
                        Z = (door.Location as LocationPoint)?.Point.Z ?? 0
                    }
                })
                .ToList();

            // Create the request body with project and door data
            var requestData = new RequestDto
            {
                ProjectName = projectName,
                FileName = fileName,
                DoorPlans = doorDataList
            };

            // Serialize the request data to JSON
            var jsonRequestData = JsonConvert.SerializeObject(requestData);

            // Send the data to the API
            using (var httpClient = new HttpClient())
            {
                var apiService = new ApiService(httpClient);
                var result = apiService.SendDoorDataAsync(jsonRequestData).Result;

                if (result)
                {
                    TaskDialog.Show("Success", "Door data sent successfully!");
                    return Result.Succeeded;
                }
                else
                {
                    TaskDialog.Show("Error", "Failed to send door data.");
                    return Result.Failed;
                }
            }
        }
    }
}