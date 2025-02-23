using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.IO;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;

namespace Revit.core.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;
        private static IConfiguration _configuration;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //if (_configuration == null)
            //{
            //    var builder = new ConfigurationBuilder()
            //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) 
            //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //    _configuration = builder.Build();
            //}

           // _apiEndpoint = _configuration["ApiService:ApiEndpoint"];
            _apiEndpoint = "http://localhost:5204/v1/test/SaveDoorData";
        }

        public async Task<bool> SendDoorDataAsync(string jsonRequestData)
        {
            var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

            try
            {
                // Uncomment and implement GetAccessToken if needed
                // var accessToken = GetAccessToken();
                // if (!string.IsNullOrEmpty(accessToken))
                // {
                //     _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                // }

                var response = await _httpClient.PostAsync(_apiEndpoint, content).ConfigureAwait(false);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error from service", $"Exception: {ex.Message}");
                return false;
            }
        }

        // Uncomment and implement GetAccessToken if needed
        // public string GetAccessToken()
        // {
        //     try
        //     {
        //         var encryptedTokenData = GetEncryptedTokenData();
        //         if (encryptedTokenData != null)
        //         {
        //             var tokenData = TokenEncryptionUtility.DecryptTokenData(encryptedTokenData);
        //             if (tokenData.Expiration > DateTime.UtcNow)
        //             {
        //                 return tokenData.AccessToken;
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         TaskDialog.Show("Error", $"Failed to load or decrypt token: {ex.Message}");
        //     }
        //     return null;
        // }

        // private byte[] GetEncryptedTokenData()
        // {
        //     // Retrieve the encrypted data from the DPAPI (Windows Data Protection API)
        //     string filePath = GetTokenFilePath();
        //     if (File.Exists(filePath))
        //     {
        //         byte[] encryptedData = File.ReadAllBytes(filePath);
        //         return encryptedData;
        //     }
        //     return null;
        // }

        // private string GetTokenFilePath()
        // {
        //     // Determine the path to store the encrypted token
        //     string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //     string directoryPath = Path.Combine(appDataPath, "RevitSyncFramework");
        //     if (!Directory.Exists(directoryPath))
        //     {
        //         Directory.CreateDirectory(directoryPath);
        //     }

        //     return Path.Combine(directoryPath, "AccessToken.dat");
        // }
    }
}
