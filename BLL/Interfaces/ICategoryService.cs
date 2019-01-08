using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        void CreateCategory(CategoryDTO newCategory);
        void RemoveCategory(int id);
        void EditCategory(CategoryDTO newCategory);
        IEnumerable<CategoryDTO> GetAllCategories();
        CategoryDTO GetCategory(int id);
        CategoryDTO GetCategoryForName(string name);

        void Dispose();
    }
}
