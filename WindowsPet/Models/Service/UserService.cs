using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPet.Models.Repository;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.Models.Service
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

       
        public Guid? GetTokenByEmail(string email)
        {
            var user = _userRepo.GetByEmail(email);
            return user.Token;
        }
        
        public void RegisterUser(PersonalData data)
        {
            // Add a new user to the database

            try
            {
                
                var user = _userRepo.GetByToken(data.Token);
                if (user != null)
                {
                    // User already exists, handle accordingly
                    CurrentUser.Token = user.Token;
                    CurrentUser.Credit = data.Credit;
                    return;
                }
                _userRepo.Add(data);
                _userRepo.Save();
                CurrentUser.Token = data.Token;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void UpdateUser(PersonalData data)
        {
            // Update user information in the database
            try
            {
                var user = _userRepo.GetByToken(data.Token);
                if (user != null)
                {
                    user.Name = data.Name;
                    user.Email = data.Email;
                    user.Credit = data.Credit;
                    _userRepo.Save();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void UpdateUserCredit(Guid? UserToken, decimal UserCredit)
        {
            var user = _userRepo.GetByToken(UserToken);

            if (user != null)
            {
                user.Credit = UserCredit;
            }
            _userRepo.Save();

            //throw new NotImplementedException();
        }
    }
}
