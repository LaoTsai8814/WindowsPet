using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models.RepositoryInterface.Database
{
    public interface ICategoriesRepository
    {
        public void AddCategory(int Id,string Name);
        public PetCategories GetCategoryNameById(int Id);
        public void UpdateCategory(int Id, string Name);
        public void DeleteCategory(int Id);

        public void Save();
    }
}
