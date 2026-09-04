using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views;

namespace WindowsPet.Models
{
    /// <summary>
    /// LoginManager manages the login process for the application.
    /// NormalLogin and Google Login are supported.
    /// </summary>
    public class LoginManager : ILoginManager
    {
        private readonly INetworkManager _networkManager;
        private readonly IUserService _userService;
        private readonly IServiceProvider _serviceProvider;

        public LoginManager(INetworkManager networkManager, IUserService userService, IServiceProvider serviceProvider)
        {
            _networkManager = networkManager;
            _userService = userService;
            _serviceProvider = serviceProvider;
        }

        public async Task NormalLogin(LoginCommand login)
        {
            await _networkManager.SendJsonAsync(login);
        }

        public void GoogleLogin()
        {
            var window = _serviceProvider.GetService<GoogleAuthWindow>() ?? new GoogleAuthWindow();
            window.GoogleLoginStatus += async (GoogleUserData userdata) =>
            {
                if (!string.IsNullOrEmpty(userdata.Name) || !string.IsNullOrEmpty(userdata.Email))
                {
                    await _networkManager.SendJsonAsync(new LoginCommand
                    {
                        Email = userdata.Email ?? string.Empty,
                        Name = userdata.Name ?? string.Empty,
                        Password = string.Empty,
                        accounttype = AccountType.Google,
                        UserToken = Guid.Empty
                    });
                }
            };
            window.ShowDialog();
        }

        public async Task RegisterationRequest(RegisterCommand command)
        {
            await _networkManager.SendJsonAsync(command);
        }

        public void UserLoggedInSuccess(PersonalData tempPersonalData)
        {
            _userService.RegisterUser(tempPersonalData);
        }
    }

    public class GoogleUserData
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
