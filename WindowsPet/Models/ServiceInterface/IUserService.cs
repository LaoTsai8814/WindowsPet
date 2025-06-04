using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models.ServiceInterface
{
    public interface IUserService
    {
        Guid? GetTokenByEmail(string email);
        void RegisterUser(PersonalData data);

        void UpdateUserCredit(Guid? token, decimal credit);



    }
}
