using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models.ConvertTools
{
    public class Request2PersonalData
    {
        #region Command To PersonalData Convertion Funtions
        
        public static PersonalData CommandPersonalDataConvertion(LoginCommand login)
        {
            CurrentUser.Token = login.UserToken;
            return new PersonalData
            {
                Name = login.Name,
                Token = login.UserToken,
                Email = login.Email,
                UserPassword = login.Password,

            };
        }
        #endregion 
    }
}
