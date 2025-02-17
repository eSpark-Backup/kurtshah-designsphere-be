using Autodesk.Revit.UI;
using Revit.WPFUI.Components.UI;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using System.Threading;
using RevitSyncFramework.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using RevitSyncFramework.Utilities;
using System.Windows.Threading;

namespace RevitSyncFramework.Commands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AccountCommand : IExternalCommand
    {
        private static DiscoveryDocumentResponse _discoveryDocument;
        private static IConfiguration _configuration;

        public AccountCommand()
        {
            if (_configuration == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                _configuration = builder.Build();
            }

            if (_discoveryDocument == null)
            {
                var task = Task.Run(async () => await GetDiscoveryDocumentAsync());
                task.Wait();
                _discoveryDocument = task.Result;
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Result result = Result.Failed;
            string resultMessage = string.Empty;

            var task = Task.Run(async () =>
            {
                var executeResult = await ExecuteAsync(commandData);
                result = executeResult.result;
                resultMessage = executeResult.message;
            });

            task.GetAwaiter().GetResult();

            message = resultMessage;
            return result;
        }
        private async Task<(Result result, string message)> ExecuteAsync(ExternalCommandData commandData)
        {
            var tokenData = GetExistingTokenData();
            if (tokenData != null && tokenData.Expiration > DateTime.UtcNow)
            {
                ShowAccountDialog(tokenData.AccessToken);
                return (Result.Succeeded, "Authentication successful!");
            }

            var deviceCode = await GetDeviceCodeAsync();
            if (deviceCode == null)
            {
                return (Result.Failed, "Failed to request device code.");
            }

            UserCodeDialog userCodeDialog = null;
            var uiThread = new Thread(() =>
            {
                userCodeDialog = new UserCodeDialog(deviceCode.UserCode, deviceCode.VerificationUri);
                userCodeDialog.ShowDialog();
            });
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();
            uiThread.Join(); // Wait for the dialog to close

            var accessToken = await PollForTokenAsync(deviceCode);
            if (accessToken != null)
            {
                ShowAccountDialog(accessToken);

                return (Result.Succeeded, "Authentication successful!");
            }
            else
            {
                return (Result.Failed, "Authentication failed or timeout occurred.");
            }
        }


        private async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync()
        {
            var client = new HttpClient();
            var discoveryDocument = await client.GetDiscoveryDocumentAsync(_configuration["IdentityServer:DiscoveryUrl"]);

            if (discoveryDocument.IsError)
            {
                // Log error if the discovery document request fails
                Console.WriteLine($"Error retrieving discovery document: {discoveryDocument.Error}");
                return null;
            }

            return discoveryDocument;
        }

        private async Task<DeviceAuthorizationResponse> GetDeviceCodeAsync()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            var deviceCodeRequest = new DeviceAuthorizationRequest
            {
                Address = _discoveryDocument.DeviceAuthorizationEndpoint,
                ClientId = _configuration["IdentityServer:ClientId"],
                ClientSecret = _configuration["IdentityServer:ClientSecret"],
                Scope = "openid profile ds_mainapiscope IdentityServerApi"
            };

            try
            {
                // Request the device authorization (device code)
                var deviceCodeResponse = await client.RequestDeviceAuthorizationAsync(deviceCodeRequest);

                if (deviceCodeResponse.IsError)
                {
                    // Handle any errors during the device code request
                    Console.WriteLine($"Error requesting device code: {deviceCodeResponse.Error}");
                    return null;
                }

                // Return the device code response to start polling
                return deviceCodeResponse;
            }
            catch (Exception ex)
            {
                // Log any other exceptions
                Console.WriteLine($"An error occurred while requesting device code: {ex.Message}");
                return null;
            }
        }

        private async Task<string> PollForTokenAsync(DeviceAuthorizationResponse deviceCode)
        {
            var client = new HttpClient();
            var tokenRequest = new TokenRequest
            {
                Address = _configuration["IdentityServer:TokenEndpoint"],
                ClientId = _configuration["IdentityServer:ClientId"],
                GrantType = "urn:ietf:params:oauth:grant-type:device_code",
                ClientSecret = _configuration["IdentityServer:ClientSecret"],
                Parameters =
                {
                    { "device_code", deviceCode.DeviceCode }
                }
            };

            while (true)
            {
                // Poll the token endpoint to check if the user has authenticated
                var tokenResponse = await client.RequestTokenAsync(tokenRequest);

                if (tokenResponse.IsError)
                {
                    if (tokenResponse.Error == "authorization_pending")
                    {
                        // Wait and try again if the user has not authenticated yet
                        await Task.Delay(1000); // Poll every second
                    }
                    else
                    {
                        // Handle errors (e.g., invalid device code, expired, etc.)
                        Console.WriteLine($"Error getting token: {tokenResponse.Error}");
                        return null;
                    }
                }
                else
                {
                    // Access token response includes `expires_in` field (e.g., 3600 seconds)
                    var expiresIn = tokenResponse.ExpiresIn;
                    var expirationTime = DateTime.UtcNow.AddSeconds(expiresIn); // Calculate expiration time

                    // Store the access token securely with its actual expiration time
                    StoreTokenData(new TokenData { AccessToken = tokenResponse.AccessToken, Expiration = expirationTime });

                    // Return the access token once authentication is successful
                    return tokenResponse.AccessToken;
                }
            }
        }

        private TokenData GetExistingTokenData()
        {
            var encryptedTokenData = LoadEncryptedTokenData();
            if (encryptedTokenData != null)
            {
                var decryptedTokenData = TokenEncryptionUtility.DecryptTokenData(encryptedTokenData);
                return decryptedTokenData;
            }
            return null;
        }

        private void StoreTokenData(TokenData tokenData)
        {
            var encryptedTokenData = TokenEncryptionUtility.EncryptTokenData(tokenData);
            SaveEncryptedTokenData(encryptedTokenData);
        }

        private byte[] LoadEncryptedTokenData()
        {
            // Load the encrypted token data from a local file or other secure location
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "encryptedToken.dat");

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }

            return null;
        }

        private void SaveEncryptedTokenData(byte[] encryptedTokenData)
        {
            // Save the encrypted token data to a local file or other secure location
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "encryptedToken.dat");
            File.WriteAllBytes(filePath, encryptedTokenData);
        }

        private void ShowAccountDialog(string accessToken)
        {
            AccountDialog accountDialog = null;
            var accountuiThread = new Thread(() =>
            {
                accountDialog = new AccountDialog(accessToken);
                accountDialog.ShowDialog();
            });
            accountuiThread.SetApartmentState(ApartmentState.STA);
            accountuiThread.Start();
            accountuiThread.Join();
        }
    }
}
