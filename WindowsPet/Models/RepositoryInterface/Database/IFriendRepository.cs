using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models.RepositoryInterface.DatabaseRepositoryInterface
{
    interface IFriendRepository
    {
        Friend GetByToken(Guid token);

        Friend GetByName(string name);

        List<Pet> GetFriendPets(Guid token);

        void Add(Friend friend);



        void Delete(Friend friend);

        void Save();

    }
}
