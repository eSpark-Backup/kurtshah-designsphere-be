
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using Autodesk.Revit.UI;
using Newtonsoft.Json;

namespace RevitSyncFramework.Services
{
    public class ApiService
    {
        private readonly string _apiEndpoint;

        public ApiService()
        {
          //  _apiEndpoint = ConfigurationManager.AppSettings["ApiEndpoint"];
            _apiEndpoint = "http://localhost:5204/v1/test/SaveDoorData";
        }

        public bool SendDoorData(string jsonRequestData)
        {

            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");


                try
                {
                    var response = client.PostAsync(_apiEndpoint, content).GetAwaiter().GetResult();
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error from service", $"Exception: {ex.Message}");
                    return false;
                }
            }
        }
    }
}

