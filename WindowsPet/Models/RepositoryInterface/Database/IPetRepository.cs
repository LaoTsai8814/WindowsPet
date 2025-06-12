using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsPet.Models.Pet;

namespace WindowsPet.Models.Repository
{
    public interface IPetRepository
    {
        Pet? GetById(Guid id);
        Pet? GetByName(string name);

        decimal GetPriceById(Guid PetId);
        List<Pet>? GetByCategory(PetCategories category);
        void Add(Pet pet);
        void Save();
        void Delete(Pet pet);
    }
}
