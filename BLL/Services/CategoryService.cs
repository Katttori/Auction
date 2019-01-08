using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Exceptions;

namespace BLL.Services
{
    public class CategoryService : ICategoryService

    {
        private readonly IUnitOfWork database;

        public CategoryService(IUnitOfWork uow)
        {
            database = uow;
        }

        public void CreateCategory(CategoryDTO newCategory)
        {
            if (newCategory == null)
                throw new ArgumentNullException();
            database.Categories.Create(new Category { Name = newCategory.Name });
        }

        public void Dispose()
        {
            database.Dispose();
        }

        public void EditCategory(CategoryDTO newCategory)
        {
            if (newCategory == null)
                throw new ArgumentNullException();

            var oldCategory = database.Categories.Get(newCategory.Id);

            if (oldCategory == null)
                throw new NotFoundException();

            oldCategory.Name = newCategory.Name;
            database.Categories.Update(oldCategory);
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            var categories = database.Categories.GetAll().ToList();
            return Mapper.Map<IEnumerable<Category>, List<CategoryDTO>>(categories);
        }

        public CategoryDTO GetCategory(int id)
        {
            var category = database.Categories.Get(id);
            if (category == null)
                throw new NotFoundException();
            return Mapper.Map<Category, CategoryDTO>(category);
        }

        public CategoryDTO GetCategoryForName(string name)
        {
            var category = database.Categories.Find(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
            if (category == null)
                throw new NotFoundException();
            return Mapper.Map<Category, CategoryDTO>(category);
        }

        public void RemoveCategory(int id)
        {
            if (id == 1)
                throw new InvalidOperationException("You cant delete default category");
            var category = database.Categories.Get(id);
            if (category == null)
                throw new NotFoundException();
            if (category.Products != null)
                {
                var defaultCategory = database.Categories.Get(1);
                foreach (var elem in category.Products)
                    elem.Category = defaultCategory;
                }
            database.Categories.Delete(id);
        }
    }
}
