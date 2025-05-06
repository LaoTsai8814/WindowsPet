using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WindowsPet.Models
{
    /// <summary>
    /// This class is used to define the data format for the application.
    /// </summary>
    #region Network Json Format
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

        /// <summary>
        /// Constructor for RegisterCommand
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="token"></param>
        
        public RegisterCommand()
        {
            
        }
    }

    
    [Serializable]
    public class GoogleLoginCommand : Command
    {
        public required string Name;
        public required string Email;
    }
    [Serializable]
    public class LoginCommand : Command
    {
        public required string Name;
        public required string Password;
        public required string Email;
    }
    [Serializable]
    public class ServerRespondStatus : Command
    {
        public required string RequestName;
        public required bool RequestStatus;
        public object? RespondParameter;
    }
    [Serializable]
    public class UserDataRequest : Command
    {
    }
    [Serializable]
    public class UserDataRespond : Command
    {
        List<Pet>? _usrpets;
        public List<Pet>? UserPet
        {
            get => _usrpets;
            set => _usrpets = value;
        }

    }
    #endregion

    #region Database Format
    /// <summary>
    /// The Pet should be given a name and id
    /// and the picture format is .png
    /// GIFlist list all GifFile in that dir
    /// </summary>
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImagePath { get; set; }

        public List<string>? GifPath { get; set; } = new();

        public Pet(int id, string name)
        {
            Id = id;
            Name = name;
            ImagePath = Path.Combine(Path.GetTempPath(),$@"{name}",$@"{name}.png");
            
            if(Directory.Exists(Path.Combine(Path.GetTempPath(), $@"{name}")))
            {
                string[] GifFolder = Directory.GetFiles(Path.Combine(Path.GetTempPath(), $@"{name}"));

                foreach (var file in GifFolder)
                {
                    if(file!=null&&file.EndsWith(".gif"))
                        GifPath.Add(file);
                }
            }  
        }
        public List<PersonalData> Owner { get; set; } = new();



    }

    #endregion

    #region UI Data Format
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
    #endregion

    #region   Default
    internal class DataFormat
    {
        public static RegisterCommand GetRegisterCommand(string? name, string? email, string? password)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("Name, email, and password cannot be null or empty.");
                }
                return new RegisterCommand()
                {
                    UserToken = "",
                    Name = name,
                    Email = email,
                    Password = password
                };
            }
            catch(Exception ex)
            {
                ErrorHandle.ShowError(ex.Message);
                throw new Exception("Error creating RegisterCommand: " + ex.Message);
                
            }

        }



    }
    #endregion
}
