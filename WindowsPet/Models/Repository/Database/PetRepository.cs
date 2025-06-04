using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models.Repository
{
    public class PetRepository : IPetRepository
    {
        private readonly AppDbContext _context;
        public PetRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(Pet pet)
            => _context.Pets.Add(pet);

        public void Delete(Pet pet)
            => _context.Pets.Remove(pet);

        public List<Pet>? GetByCategory(Pet.PetCategorySet category)
            => _context.Pets.Where(p => p.PetCategory.Contains(category)).ToList();

        public Pet? GetById(Guid id)
            => _context.Pets.FirstOrDefault(p => p.Id == id);

        public Pet? GetByName(string name)
            =>_context.Pets.FirstOrDefault(p => p.Name == name);

        public decimal GetPriceById(Guid name)
        {
            throw new NotImplementedException();
        }

        public void Save()
            => _context.SaveChanges();
    }
}
