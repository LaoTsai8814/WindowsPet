using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;

namespace WindowsPet.Models
{
    /// <summary>
    /// This class is used to define the data format for the application.
    /// </summary>
    public enum PetType
    {
        Non_Define,
        PopularPet,
        UserPet,
    }
    public enum AccountType
    {
        Normal,
        Google,
    }

    #region Network Json Format
    [Serializable]
    public class Command
    {
        public required Guid UserToken;
        public string CommandName { get; }
        public Command()
        {
            CommandName = GetType().Name;
        }
    }

    [Serializable]
    public class RegisterCommand : Command
    {
        public required string Password;
        public required string Name;
        public required string Email;
        public required AccountType accounttype;
    }

    

    [Serializable]
    public class LoginCommand : Command
    {
        public required string Name;
        public required string Password;
        public required string Email;
        public required AccountType accounttype;
    }

    [Serializable]
    public class ServerRespondStatus : Command
    {
        public required string RequestName;
        public required bool RequestStatus;
        public string StatusDescription = "";
        public object? RespondParameter;
    }

    [Serializable]
    public class UserDataRequestCommand : Command
    {
       
        private List<FriendRequest> _pendingfriendlist =new();

        public List<FriendRequest> PendingFriendList
        {
            get { return _pendingfriendlist; }
            set { _pendingfriendlist = value; }
        }

        private List<Friend> _friendlist = new();

        public List<Friend> FriendList
        {
            get { return _friendlist; }
            set { _friendlist = value; }
        }

        public decimal Credit { get; set; }
    }
    [Serializable]
    
    
    public class  PetPurchaseCommand : Command
    {
        public required int PetId; 
        public required decimal Credit;
        public required decimal Price;

    }


    [Serializable]
    public class SearchFriendRequest : Command
    {
        public string Token = "";
        
        public PersonalData Friend = new();
    }
    [Serializable]
    public class AcceptFriendRequest : Command
    {
        public required string Token;
        public  Friend Friend = new();
    }
    public class DeniedFriendRequest : Command
    {
        public required string Token;
        public  Friend Friend = new();
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
        [Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumConverter))]
        public enum PetCategorySet
        {
            Non_Define,
            PopularPet,
            UserPet,
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImagePath { get; set; }

        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public string? Description { get; set; } = string.Empty;
        public bool IsAdopted { get; set; } = false;

        public HashSet<PetCategorySet> PetCategory { get; set; } = new HashSet<PetCategorySet>() { PetCategorySet.Non_Define };

        public List<string>? GifPath { get; set; } = new();

        public Pet(Guid id, string name,decimal price=0)
        {
            Id = id;
            Name = name;
            Price = price;
            /* Determine the path of the image and gif file
             * Previous Edition
            // 根據 IsPopular 決定資料夾名稱
            if (IsPopular)
            {
                var pngresult = FileManager.Instance.PopularPetFileLocationDict[name].Where(str => str.EndsWith(".png"));
                if (pngresult.Count() > 0)
                {
                    ImagePath = pngresult.First();
                }
                var gifresult = FileManager.Instance.PopularPetFileLocationDict[name].Where(str => str.EndsWith(".gif"));
                if(gifresult.Count() > 0)
                {
                    foreach (var file in gifresult)
                    {
                        GifPath.Add(file);
                    }
                }
            }
            else
            {
                var pngresult = FileManager.Instance.UserPetFileLocationDict[name].Where(str => str.EndsWith(".png"));
                if (pngresult.Count() > 0)
                {
                    ImagePath = pngresult.First();
                }
                var gifresult = FileManager.Instance.UserPetFileLocationDict[name].Where(str => str.EndsWith(".gif"));
                if (gifresult.Count() > 0)
                {
                    foreach (var file in gifresult)
                    {
                        GifPath.Add(file);
                    }
                }
            }*/



        }
        [Newtonsoft.Json.JsonIgnore]
        public List<PersonalData> Owner { get; set; } = new();

    }

    

    #endregion

    #region UI Data Format
    public class UIPets
    {
        private string? image;

        public string? Image
        {
            get { return image; }
            set { image = value; }
        }

        private string? name;

        public string? Name
        {
            get { return name; }
            set { name = value; }
        }

        public UIPets(string name, string image)
        {
            Name = name;
            Image = image;
        }
        List<string> gifpath = new();
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
                    UserToken = Guid.Empty,
                    Name = name,
                    Email = email,
                    Password = password
                    ,accounttype = AccountType.Normal
                };
            }
            catch (Exception ex)
            {
                ErrorHandle.ShowError(ex.Message);
                throw new Exception("Error creating RegisterCommand: " + ex.Message);

            }

        }



    }
    #endregion
}
