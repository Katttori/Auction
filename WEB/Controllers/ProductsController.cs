using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WEB.Models;
using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;

namespace WEB.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;


        public ProductsController(IProductService productService, ICategoryService categoryService, IUserService userService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.userService = userService;
        }
        [HttpGet]
        [Route("get")]
        [Authorize(Roles = "Admin, Moderator")]
        public IHttpActionResult Get()
        {
            return Ok(Mapper.Map<IEnumerable<ProductDTO>, List<ProductModel>>(productService.GetAllProducts()));
        }

        [HttpGet]
        [Route("get/{id}")]
        [Authorize]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var product = productService.GetProduct(id);
                return Ok(Mapper.Map<ProductDTO, ProductModel>(product));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("get/user")]
        [Authorize]
        public IHttpActionResult GetFrowUser()
        {
            var id = userService.GetUserByName(User.Identity.Name).Id;
            try
            {
                var products = productService.GetProductsFromUser(id);
                return Ok(Mapper.Map<IEnumerable<ProductDTO>, List<ProductModel>>(products));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("get/confirmation")]
        [Authorize(Roles = "Admin, Moderator")]
        public IHttpActionResult GetToConfirm()
        {
          
                var products = productService.GetToConfirm();
                return Ok(Mapper.Map<IEnumerable<ProductDTO>, List<ProductModel>>(products));
            
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public IHttpActionResult CreateProduct([FromBody]ProductModel product)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");

            var newProduct = Mapper.Map<ProductModel, ProductDTO>(product);
            newProduct.Category = categoryService.GetCategoryForName(product.Category);
            newProduct.Owner = userService.GetUserByName(User.Identity.Name);
            try
            {
                productService.CreateProduct(newProduct);
                return Ok("Product created");
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Should give product");
            }
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public IHttpActionResult EditProduct([FromBody]ProductModel product)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");
            var newProduct = Mapper.Map<ProductModel, ProductDTO>(product);
            try
            {
                productService.EditProduct(newProduct);
                return Ok($"Product {product.Name} eddited succesfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Should give product");
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("update/{productId}/{categoryId}")]
        [Authorize]
        public IHttpActionResult ChangeProductCategory(int productId, int categoryId)
        {
            try
            {
                productService.ChangeProductCategory(productId, categoryId);
                return Ok($"Product category was successfuly update");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("confirm/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public IHttpActionResult ConfirmProduct(int id)
        {
            try
            {
                productService.ConfirmProduct(id);
                return Ok("Product confirmed successfuly");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize]
        public IHttpActionResult RemoveProduct(int id)
        {
            try
            {
                productService.RemoveProduct(id);
                return Ok("Product successfuly deleted");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}