using System.ComponentModel;
using System.Windows;
using Newtonsoft.Json;
using System.Text;
using Revit.WPFUI.Helpers;

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

        private void GetUserInfoAsync()
        {
            try
            {
                // Decode the JWT token
                var parts = AccessToken.Split('.');
                if (parts.Length == 3)
                {
                    var payload = parts[1];
                    var json = TokenHelper.Base64UrlDecode(payload);

                    // Deserialize the payload into a dictionary
                    var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    // Extract the "fullname" claim
                    if (jsonObject.ContainsKey("fullname"))
                    {
                        UserName = jsonObject["fullname"].ToString();  // Set the UserName property
                        Message = $"User logged in successfully: {UserName}";
                    }
                    else
                    {
                        Message = "Invalid token.";
                    }
                }
                else
                {
                    Message = "Invalid token format.";
                }
            }
            catch (Exception ex)
            {
                Message = "Error decoding token.";
                Console.WriteLine($"Error decoding token: {ex.Message}");
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
