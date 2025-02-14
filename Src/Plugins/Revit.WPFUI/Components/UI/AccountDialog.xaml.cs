using System;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json.Linq;
using IdentityModel.Client;

namespace Revit.WPFUI.Components.UI
{
    public partial class AccountDialog : Window, INotifyPropertyChanged
    {
        private string _userName;
        private string _message;

        public string AccessToken { get; set; }
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));  // Notify UI of the property change
            }
        }
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));  // Notify UI of the property change
            }
        }

        // Constructor that accepts the accessToken
        public AccountDialog(string accessToken)
        {
            InitializeComponent();
            this.AccessToken = accessToken;
            this.DataContext = this; // Bind data to the UI
            GetUserInfoAsync(); // Fetch user info based on the access token
        }

        // Fetch user information using the access token
        private async void GetUserInfoAsync()
        {
            try
            {
                var client = new HttpClient();
                client.SetBearerToken(AccessToken);  // Set Authorization header with the access token

                // Make a request to the user info endpoint to fetch user details (e.g., name)
                var response = await client.GetStringAsync("https://localhost:44310/connect/userinfo");

                // Parse the response JSON (assuming it returns user information like name)
                var userInfo = JObject.Parse(response);
                UserName = userInfo["name"]?.ToString();  // Set the UserName property

                // Set a custom message for success or authentication
                Message = $"User logged in successfully: {UserName}";
            }
            catch (Exception ex)
            {
                Message = "Error fetching user info.";
                Console.WriteLine($"Error fetching user info: {ex.Message}");
            }
        }

        // Implement INotifyPropertyChanged to notify UI of property changes
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
