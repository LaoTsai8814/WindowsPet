using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.VM
{
    public class RegisterVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly ILoginManager _loginManager;

        #region UI User Registration information
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

        private string? _confirmpasswd;
        public string? ConfirmPassword
        {
            get => _confirmpasswd;
            set
            {
                _confirmpasswd = value;
                OnPropertyChanged();
            }
        }

        private string? _email;
        public string? Email
        {
            get => _email;
            set
            {
                _email = value;
                if (_email != null && !VerifyInput.IsValidEmailFormat(_email))
                {
                    Email = null;
                }
                OnPropertyChanged();
            }
        }

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
        #endregion

        public ICommand RegisterCommand { get; set; }

        public RegisterVM(ILoginManager loginManager)
        {
            _loginManager = loginManager;
            RegisterCommand = new RelayCommands(OnRegisterButtonClicked);
        }

        private async void OnRegisterButtonClicked(object? obj)
        {
            if (VerifyInput.IsPasswordEqual(Password, ConfirmPassword) && VerifyInput.IsStrongPassword(Password))
            {
                await _loginManager.RegisterationRequest(DataFormat.GetRegisterCommand(Username, Email, Password));
            }
            else if (!VerifyInput.IsPasswordEqual(Password, ConfirmPassword))
            {
                ErrorHandle.ShowError("Passwords do not match.");
            }
            else
            {
                ErrorHandle.ShowError("Password is not strong enough.");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
