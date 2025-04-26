using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WindowsPet.Models
{

    /// <summary>
    /// This class is used to define the data format for the application.
    /// </summary>
    /// 
    [Serializable]
    public class Command
    {
        public required string UserToken;
        public string CommandName { get; }
        public Command()
        {
            CommandName = GetType().Name;
        }

    }
    [Serializable]
    public class RegisterCommand : GoogleLoginCommand
    {
        public required string Password;
    }
    public class GoogleLoginCommand : Command
    {
        public required string Name;
        public required string Email;
    }
    public class LoginCommand : Command
    {
        public required string Name;
        public required string Password;
        public required string Email;
    }
    public class ServerRespondStatus : Command
    {
        public required string RequestName;
        public required bool RequestStatus;
        public object? RespondParameter;
    }
    public class  UserDataRequest :Command
    {
    }
    public class UserDataRespond : Command
    {
        
    }
    public class  UserPets
    {
        private string image;

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public UserPets(string name, string image)
        {
            Name = name;
            Image = image;
        }
    }
    internal class DataFormat{}
}
