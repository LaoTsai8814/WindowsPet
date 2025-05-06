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
using Microsoft.Web.WebView2.Core;

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

            InitializeComponent();
            Loaded += GoogleAuthWindow_Loaded;

        }
        private async void GoogleAuthWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await OAuthBrowser.EnsureCoreWebView2Async();

                OAuthBrowser.CoreWebView2.NavigationCompleted += OAuthBrowser_NavigationCompleted;

                string oauthUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                                  $"client_id={clientId}&" +
                                  $"redirect_uri=http://localhost&" +
                                  $"response_type=code&" +
                                  $"scope=openid%20email%20profile&" +
                                  $"access_type=offline";

                OAuthBrowser.CoreWebView2.Navigate(oauthUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化 OAuth 瀏覽器時發生錯誤：{ex.Message}");
            }
        }
        private async void OAuthBrowser_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                var uri = new Uri(OAuthBrowser.Source.ToString());

                if (uri.Host == "localhost" && uri.Query.Contains("code="))
                {
                    var query = HttpUtility.ParseQueryString(uri.Query);
                    var code = query["code"];
                    if (!string.IsNullOrEmpty(code))
                    {
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

                            string? email = token.Payload.TryGetValue("email", out var emailObj) ? emailObj?.ToString() : null;
                            string? name = token.Payload.TryGetValue("name", out var nameObj) ? nameObj?.ToString() : null;

                            var userData = new GoogleUserData
                            {
                                Name = name,
                                Email = email,
                                Token = idToken
                            };

                            GoogleLoginStatus?.Invoke(userData);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("登入失敗，無法取得 id_token");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"OAuth 登入過程發生錯誤：\n{ex.Message}");
            }
        }
    }
}
