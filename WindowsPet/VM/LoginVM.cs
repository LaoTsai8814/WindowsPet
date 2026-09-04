using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using WindowsPet.Command;
using WindowsPet.Models;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views.Tabs;

namespace WindowsPet.VM
{
    public class LoginVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ILoginManager _loginManager;
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        public Action? ChangeTab { get; set; }

        #region Normal Login Username and Password
        private string? _username;
        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Google Login
        public ICommand GoogleLoginCommand { get; set; }
        #endregion

        #region Login Command
        public ICommand LoginCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private async void  OnLoginButtonClicked(object? obj)
        {
            if (Username != null && Password != null)
            {
                LoginCommand cmd = new LoginCommand
                {
                    Name = "",
                    UserToken = Guid.Empty,
                    Password = Password,
                    Email = "",
                    accounttype = AccountType.Normal
                };
                if (!VerifyInput.IsValidEmailFormat(Username))
                {
                    cmd.Name = Username;
                }
                else
                {
                    cmd.Email = Username;
                }
                await _loginManager.NormalLogin(cmd);
            }
            Username = string.Empty;
            Password = string.Empty;
        }
        #endregion

        #region Register Command
        public ICommand RegisterCommand { get; set; }

        private void OnRegisterButtonClicked(object? obj)
        {
            Tab = _serviceProvider.GetRequiredService<RegisterTab>();
        }
        #endregion

        #region TabControl
        private object? _tab;
        public object? Tab
        {
            get
            {
                // 如果 Tab 還沒初始化，第一次存取時才去抓
                if (_tab == null)
                {
                    _tab = _serviceProvider.GetRequiredService<LoginTab>();
                }
                return _tab;
            }
            set
            {
                _tab = value;
                OnPropertyChanged();
            }
        }

        public void OnChangeToLoginTab()
        {
            Tab = _serviceProvider.GetRequiredService<LoginTab>();
        }
        #endregion

        public LoginVM(ILoginManager loginManager, INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _loginManager = loginManager;
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            GoogleLoginCommand = new RelayCommands((object? obj) =>
            {
                _loginManager.GoogleLogin();
            });

            MinimizeCommand = new RelayCommands((object? obj) =>
            {
                if (Application.Current.MainWindow != null)
                    Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });

            CloseCommand = new RelayCommands((object? obj) =>
            {
                Application.Current.Shutdown();
            });

            LoginCommand = new RelayCommands(OnLoginButtonClicked);
            RegisterCommand = new RelayCommands(OnRegisterButtonClicked);
            ChangeTab = OnChangeToLoginTab;

            
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
