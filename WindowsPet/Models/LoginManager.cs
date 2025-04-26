using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsPet.Views;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WindowsPet.Models
{
    /// <summary>
    /// LoginManager is a singleton class that manages the login process for the application.
    /// NormalLogin and Google Login are supported.
    /// GoogleLogin will open a new window for the user to login with their Google account.
    /// GoogleLogin and Normal Login will Both 
    /// </summary>
    internal class LoginManager
    {
        private static LoginManager? _loginmanager;

        public static LoginManager Instance => _loginmanager ??= new();

        
        public async void NormalLogin(LoginCommand login)
        {
            await JsonSerialize.SerializeAndSendJson<LoginCommand>(login);

        }
        public void GoogleLogin()
        {
            var window1 = new GoogleAuthWindow();
            window1.GoogleLoginStatus += async (GoogleUserData userdata) =>
            {
                string token = "";
                if (!string.IsNullOrEmpty(userdata.Name) || !string.IsNullOrEmpty(userdata.Email))
                {
                    // 處理邏輯
                   
                    #region Send It To Server
                    await JsonSerialize.SerializeAndSendJson<GoogleLoginCommand>(new GoogleLoginCommand
                    {
                        Email = userdata.Email,
                        Name = userdata.Name,
                        UserToken = token
                    });
                    #endregion
                }
            };
            window1.ShowDialog();

        }
        public async Task RegisterationRequest(RegisterCommand command)
        {
            await JsonSerialize.SerializeAndSendJson<RegisterCommand>(command);

        }
        public void UserLoggedInSuccess(PersonalData TempPersonalData)
        {
            AppDbContext.Instance.AddUser(TempPersonalData);
            AppDbContext.Instance.SaveChangesToDB();
        }

        LoginManager() { }
    }
    public class GoogleUserData
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
