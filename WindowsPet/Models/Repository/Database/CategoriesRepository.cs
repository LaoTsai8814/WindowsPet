using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPet.Models.RepositoryInterface.Database;

namespace WindowsPet.Models.Repository.Database
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly AppDbContext _dbcontext;
        public CategoriesRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void AddCategory(int Id, string Name)
            =>_dbcontext.Categories.Add(new PetCategories { Id = Id, Type = Name });

        public void DeleteCategory(int Id)
            => _dbcontext.Categories.Remove(_dbcontext.Categories.FirstOrDefault(c => c.Id == Id) ?? throw new InvalidOperationException("Category not found."));

        public PetCategories GetCategoryNameById(int Id)
            => _dbcontext.Categories.FirstOrDefault(c => c.Id == Id) ?? throw new InvalidOperationException("Category not found.");

        public void Save()
            => _dbcontext.SaveChanges();

        //Not Written Property
        public void UpdateCategory(int Id, string Name)
           => _dbcontext.Categories.FirstOrDefault(c => c.Id == Id);




    }
}
