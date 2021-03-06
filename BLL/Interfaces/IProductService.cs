﻿using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProductService
    {
       
        void EditProduct(ProductDTO newProduct);
        void CreateProduct(ProductDTO newProduct);
        void RemoveProduct(int id);
        IEnumerable<ProductDTO> GetAllProducts();
        ProductDTO GetProduct(int id);
        void ChangeProductCategory(int productId, string categoryName);
        void ConfirmProduct(int id);
        IEnumerable<ProductDTO> GetToConfirm();

        void Dispose();
        IEnumerable<ProductDTO> GetProductsFromUser(string userId);

    }
}
