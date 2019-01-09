using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WEB.Models;
using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Exceptions;

namespace WEB.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService service)
        {
            categoryService = service;
        }
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            return Ok(Mapper.Map<IEnumerable<CategoryDTO>, List<CategoryModel>>(categoryService.GetAllCategories()));
        }

        
        [HttpGet]
        [Route("get/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var category = categoryService.GetCategory(id);
                return Ok(Mapper.Map<CategoryDTO, CategoryModel>(category));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        
        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Admin, Moderator")]
        public IHttpActionResult CreateCategory([FromBody]CategoryModel category)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");
            var newCategory = Mapper.Map<CategoryModel, CategoryDTO>(category);
            try
            {
                categoryService.CreateCategory(newCategory);
                return Ok("Category created successfuly");
            }
            catch (ArgumentNullException)
            {
                return BadRequest("You should create category");
            }
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Admin, Moderator")]
        public IHttpActionResult UpdateCategory([FromBody] CategoryModel category)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");
            var newCategory = Mapper.Map<CategoryModel, CategoryDTO>(category);
            try
            {
                categoryService.EditCategory(newCategory);
                return Ok("Category updated successfuly");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("You shoud give category");
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public IHttpActionResult DeleteCategory(int id)
        {
            try
            {
                categoryService.RemoveCategory(id);
                return Ok("Category successfuly removed");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}