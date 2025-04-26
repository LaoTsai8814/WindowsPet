using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Windows;
using System.IdentityModel.Tokens.Jwt;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsPet.VM;
using WindowsPet.Models;

namespace WindowsPet.Views
{
    /// <summary>
    /// GoogleLoginView.xaml 的互動邏輯
    /// </summary>

    public partial class GoogleAuthWindow : Window
    {
        private string clientId = $@"189087676030-5qoad2gdpvm1p9u25u8h01vvod0suuq8.apps.googleusercontent.com";
        private string clientSecret = $@"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";
        
        public event Action<GoogleUserData>? GoogleLoginStatus;

        public GoogleAuthWindow()
        {
            InitializeComponent();

            string oauthUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                         $"client_id={clientId}&" +
                         $"redirect_uri=http://localhost&" +
                         $"response_type=code&" +
                         $"scope=openid%20email%20profile&" +
                         $"access_type=offline";

            OAuthBrowser.Navigated += OAuthBrowser_Navigated;
            OAuthBrowser.Navigate(oauthUrl);

        }
        private async void OAuthBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Uri.AbsoluteUri.StartsWith("http://localhost") && e.Uri.Query.Contains("code="))
            {
                var query = HttpUtility.ParseQueryString(e.Uri.Query);
                var code = query["code"];
                if (code != null)
                {
                    // 取得 access_token
                    using var http = new HttpClient();
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "code", code },
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "redirect_uri", "http://localhost" },
                    { "grant_type", "authorization_code" }
                });

                    var response = await http.PostAsync("https://oauth2.googleapis.com/token", content);
                    var json = await response.Content.ReadAsStringAsync();
                    var tokenData = JsonSerializer.Deserialize<JsonElement>(json);

                    if (tokenData.TryGetProperty("id_token", out var idTokenElement))
                    {
                        var idToken = idTokenElement.GetString();
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(idToken);
                        string? email = token?.Payload["email"]?.ToString();
                        string? name = token?.Payload["name"]?.ToString();
                        // Pack email and name into a class
                        var userData = new GoogleUserData
                        {
                            Name = name,
                            Email = email,
                            Token = idToken
                        };
                        //Give it to LoginManager
                        GoogleLoginStatus?.Invoke(userData);
                        //MessageBox.Show($"登入成功！\n\nID Token:\n{idToken}");
                        //MessageBox.Show($"Email:{email}\n\n{name}");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("登入失敗，無法取得 id_token");
                    }

                }
            }
        }
    }
}
