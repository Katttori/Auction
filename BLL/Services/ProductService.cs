using BLL.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs;
using DAL.Interfaces;
using DAL.Entities;
using BLL.Exceptions;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork database;
        public ProductService(IUnitOfWork uow)
        {
            database = uow;
        }
        public void ChangeProductCategory(int productId, int categoryId)
        {
            var product = database.Products.Get(productId);
            var category = database.Categories.Get(categoryId);
            if (product == null || category == null)
                throw new NotFoundException();
            product.Category = category;
            database.Products.Update(product);
        }

        public void CreateProduct(ProductDTO newProduct)
        {
            if (newProduct == null)
                throw new ArgumentNullException();
            Product product = new Product
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Image = newProduct.Image,
                Price = newProduct.Price,
                Category = newProduct.Category == null ? database.Categories.Get(1) : database.Categories.Get(newProduct.Category.Id),
                Owner = database.Users.Get(newProduct.Owner.Id)
            };
            database.Products.Create(product);
        }

        public void Dispose()
        {
            database.Dispose();
        }

        public void EditProduct(ProductDTO newProduct)
        {
            if (newProduct == null)
                throw new ArgumentNullException();
            var oldProduct = database.Products.Get(newProduct.Id);
            if (oldProduct == null)
                throw new NotFoundException();
            if (oldProduct.IsConfirmed)
                throw new InvalidOperationException("Cant change lots");

            oldProduct.Name = newProduct.Name;
            oldProduct.Description = newProduct.Description;
            oldProduct.Image = newProduct.Image;
            oldProduct.Price = newProduct.Price;
            database.Products.Update(oldProduct);
        }

        public IEnumerable<ProductDTO> GetAllProducts()
        {
            return Mapper.Map<IEnumerable<Product>, List<ProductDTO>>(database.Products.GetAll());
        }

        public ProductDTO GetProduct(int id)
        {
            var product = database.Products.Get(id);
            if (product == null)
                throw new NotFoundException();
            return Mapper.Map<Product, ProductDTO>(product);
        }

        public IEnumerable<ProductDTO> GetProductsFromUser(string userId)
        {
            var user = database.Users.Get(userId);
            if (user == null)
                throw new NotFoundException();

            var products = database.Products.Find(x => x.UserID == userId).ToList();
            return Mapper.Map<IEnumerable<Product>, List<ProductDTO>>(products);
        }

        public void RemoveProduct(int id)
        {
            var product = database.Products.Get(id);
            if (product == null)
                throw new NotFoundException();
            database.Products.Delete(id);
        }

        public void ConfirmProduct(int id)
        {
            var product = database.Products.Get(id);
            if (product == null)
                throw new NotFoundException();
            product.IsConfirmed = true;
            database.Products.Update(product);
        }
    }
}
