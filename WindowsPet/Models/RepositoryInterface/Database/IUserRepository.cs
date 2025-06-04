using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models.Repository
{
    public interface IUserRepository
    {
        PersonalData? GetByToken(Guid? UserId);
        PersonalData? GetByEmail(string email);
        void Add(PersonalData user);
        void Save();
    }
}
