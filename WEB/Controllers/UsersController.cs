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
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IUserService userService;

        public UsersController(IUserService service)
        {
            userService = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("get")]
        public IHttpActionResult  Get()
        {
            var users = userService.GetAllUsers();
            return Ok(Mapper.Map<IEnumerable<UserDTO>, List<UserModel>>(users));
        }

        [HttpGet]
        [Route("info")]
        public IHttpActionResult GetUserInfo()
        {
            var id = userService.GetUserByName(User.Identity.Name).Id;

            try
            {
                var user = Mapper.Map<UserDTO, UserModel>(userService.GetSingle(id));
                return Ok(user);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("update")]
        public IHttpActionResult UpdateUser( [FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");
            var newUser = Mapper.Map<UserModel, UserDTO>(user);
            try
            {
                userService.EditUser(newUser);
                return Ok("User eddited successfuly");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Should give user");
            }
        }
    }
}