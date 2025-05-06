using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;

namespace WindowsPet.VM
{
    internal class RegisterVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        #region UI User Registration infomation
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

        private string? _confirmpasswd;

        public string? ConfirmPassword
        {
            get { return _confirmpasswd; }
            set
            {
                _confirmpasswd = value;
                OnPropertyChanged();
            }
        }

        private string? _email;

        public string? Email
        {
            get { return _email; }
            set
            {
                _email = value;
                if(_email!=null&&!VerifyInput.IsValidEmailFormat(_email))
                {
                    Email = null;
                }
                OnPropertyChanged();
            }
        }
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
        #endregion
        public ICommand RegisterCommand { get; set; }
        public RegisterVM()
        {
            // Constructor logic here
            RegisterCommand = new RelayCommands(OnRegisterButtonClicked);
        }
        /// <summary>
        /// On Register Button Clicked 
        /// </summary>
        private async void OnRegisterButtonClicked(object obj)
        {
            #region Get All Property In This Class(Current Not Used)
            //Type type = this.GetType();
            //PropertyInfo[] properties = type.GetProperties();
            //foreach (PropertyInfo property in properties)
            //{
            //    var value = property.GetValue(this);
            //    if (value == null)
            //    {
            //        // Handle null value
            //        Console.WriteLine($"Property {property.Name} is null.");
            //        return;
            //    }
            //}
            #endregion
            
            #region Check Password And Email and send to server
            if (VerifyInput.IsPasswordEqual(Password, ConfirmPassword)&& VerifyInput.IsStrongPassword(Password))
            {
                // Call the registration logic here
                await LoginManager.Instance.RegisterationRequest(DataFormat.GetRegisterCommand(Username,Email,Password));                
            }
            else if(!VerifyInput.IsPasswordEqual(Password, ConfirmPassword))
            {
                ErrorHandle.ShowError("Passwords does not match.");
            }
            else
            {
                ErrorHandle.ShowError("Password is not strong enough.");
            }
            #endregion
        }
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
