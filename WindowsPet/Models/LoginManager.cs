using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WindowsPet.Models.Service;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views;
using WindowsPet.VM.TabsVM;
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

        
        public async Task NormalLogin(LoginCommand login)
        {
            await JsonSerialize.SerializeAndSendJson<LoginCommand>(login);
        }
        public void GoogleLogin()
        {
            var window1 = new GoogleAuthWindow();
            window1.GoogleLoginStatus += async (GoogleUserData userdata) =>
            {
                
                if (!string.IsNullOrEmpty(userdata.Name) || !string.IsNullOrEmpty(userdata.Email))
                {
                    // 處理邏輯
                   
                    #region Send It To Server
                    await JsonSerialize.SerializeAndSendJson<LoginCommand>(new LoginCommand
                    {
                        Email = userdata.Email!,
                        Name = userdata.Name!,
                        Password = string.Empty,
                        accounttype = AccountType.Google,
                        UserToken = Guid.Empty
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
            using (var scope = App.ServiceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                userService.RegisterUser(TempPersonalData);
            }
            // 這裡可以進行用戶登錄成功後的操作，例如更新界面或顯示消息
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
