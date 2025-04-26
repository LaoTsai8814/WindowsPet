using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;
using WindowsPet.Views;
using WindowsPet.Views.Tabs;

namespace WindowsPet.VM
{
    internal class LoginVM: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Action ChangeTab;
        #region Normal Login Username and Password
        private string? _username;
        public string? Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        private string? _password;
        public string? Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Google Login
        public ICommand GoogleLoginCommand { get; set; } = new RelayCommands((object obj) =>
        {       
            ///<summary>
            ///Going Into Google Login Manager and Let it Handle the Logic
            /// </summary>
            LoginManager.Instance.GoogleLogin();

        });
        #endregion
        #region Login Command
        public ICommand LoginCommand { get; set; } 
        private void OnLoginButtonClicked(object obj) 
        {
            if (Username!=null&&Password!=null)
            {
                LoginCommand cmd = new LoginCommand
                {
                    Name = "",
                    UserToken = "",
                    Password = Password,
                    Email = "",
                };
                if(!VerifyInput.IsValidEmailFormat(Username))
                {
                    cmd.Name = Username;
                }
                else
                {
                    cmd.Email = Username;
                }
                LoginManager.Instance.NormalLogin(cmd);
                
            }
            Username = string.Empty;
            Password = string.Empty;

        }
        #endregion
        #region Register Command
        public ICommand RegisterCommand { get; set; }
        private void OnRegisterButtonClicked(object obj)
        {
            Tab = new RegisterTab();
        }
        #endregion
        #region TabControl
        private object? _tab;

        public object? Tab
        {
            get { return _tab; }
            set 
            {
                
                _tab = value;
                OnPropertyChanged();

            }
        }
        public void OnChangeToLoginTab()
        {
            Tab = new LoginTab();

        }
        #endregion
        public LoginVM()
        {
            AppDbContext.Instance.ConnectToDB();
            Task.Run(async () =>
            {
                ErrorHandle.Instance.StartHandler();
                await NetworkManager.Instance.CreateAsync();
            });
            LoginCommand = new RelayCommands(OnLoginButtonClicked);
            Tab = new LoginTab();
            RegisterCommand = new RelayCommands(OnRegisterButtonClicked);
            ChangeTab = OnChangeToLoginTab;
        }
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
