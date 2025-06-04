using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsPet.Models.Pet;

namespace WindowsPet.Models.ServiceInterface
{
    public interface IPetService
    {
        void AddPetListToUser(Guid token, List<Pet> pets);
        void AddPetToUser(Guid token, Pet pet);

        void DeletePetFromUser(Guid token, Pet pet);

        void DeletePetListFromUser(Guid token, List<Pet> pets);

        void UpdatePetFromUser(Guid token, Pet pet);

        void UpdatePetListFromUser(Guid token, List<Pet> pets);

        void UpdatePetStatusFromUser(Guid token, Pet pet);

        bool IsPetOwnByUser(Guid UserId,Guid PetId);

        void AddPopularPetToTable(List<Pet>? petList);

        public bool IsPetPurchased(Guid token, string? petname);

    }
}
