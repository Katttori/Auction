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
using DAL.Identity.Interfaces;
using BLL.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using DAL.Identity.Entities;
using System.Security.Claims;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWorkIdentity databaseIdentity;
        private readonly IUnitOfWork database;

        public UserService(IUnitOfWork uow, IUnitOfWorkIdentity uowi)
        {
            database = uow;
            databaseIdentity = uowi;
        }
        public async Task<RegistrationDetails> CreateUserAsync(UserDTO newUser)
        {
            var user = await databaseIdentity.UserManager.FindByEmailAsync(newUser.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = newUser.Email, UserName = newUser.Email };
                var result = await databaseIdentity.UserManager.CreateAsync(user, newUser.Password);

                if (result.Errors.Count() > 0)
                    return new RegistrationDetails(false, result.Errors.FirstOrDefault(), "");

                await databaseIdentity.UserManager.AddToRoleAsync(user.Id, "user");
                User clientProfile = new User { Id = user.Id, Name = newUser.UserName };
                databaseIdentity.ClientManager.Create(clientProfile);
                await databaseIdentity.SaveAsync();
                return new RegistrationDetails(true, "register successful", "");
            }
            else
            {
                return new RegistrationDetails(false, "login already registered", "Email");
            }
        }

        public void Dispose()
        {
            database.Dispose();
        }

        public void EditUser(UserDTO newUser)
        {
            if (newUser == null)
                throw new ArgumentNullException();
            var user = database.Users.Get(newUser.Id);
            if (user == null)
                throw new NotFoundException();
            user.Name = newUser.Name;
            database.Users.Update(user);

        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var appUsers = databaseIdentity.UserManager.Users.ToList();
            var list = new List<UserDTO>();

            if (appUsers != null)
                foreach (var appUser in appUsers)
                    list.Add(CreateUserDTO(appUser));

            return list;
        }

        public UserDTO GetSingle(string id)
        {
            var user = database.Users.Get(id);
            if (user == null)
                throw new NotFoundException();
            return Mapper.Map<User, UserDTO>(user);
        }

        public async Task<Tuple<ClaimsIdentity, ClaimsIdentity>> FindAsync(string username, string password)
        {
            var appUser = await databaseIdentity.UserManager.FindAsync(username, password);

            if (appUser == null)
                throw new NotFoundException("The user name or password is incorrect.");

            ClaimsIdentity oAuthIdentity = await databaseIdentity.UserManager.CreateIdentityAsync(appUser, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await databaseIdentity.UserManager.CreateIdentityAsync(appUser, CookieAuthenticationDefaults.AuthenticationType);

            return new Tuple<ClaimsIdentity, ClaimsIdentity>(oAuthIdentity, cookiesIdentity);
        }
        public UserDTO GetUserByName(string name)
        {
            ApplicationUser appUser = databaseIdentity.UserManager.FindByName(name);

            if (appUser == null)
                throw new NotFoundException();

            return CreateUserDTO(appUser);
        }

        
        public async Task EditRole(string userId, string newRoleName)
        {
            var user = await databaseIdentity.UserManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException();

            var oldRole = GetRoleForUser(userId);

            if (oldRole != newRoleName)
            {
                await databaseIdentity.UserManager.RemoveFromRoleAsync(userId, oldRole);
                await databaseIdentity.UserManager.AddToRoleAsync(userId, newRoleName);

                await databaseIdentity.UserManager.UpdateAsync(user);
            }
        }

        public IEnumerable<string> GetRoles()
        {
            return databaseIdentity.RoleManager.Roles.Select(x => x.Name);
        }

        private UserDTO CreateUserDTO(ApplicationUser user)
        {
            var products = Mapper.Map<IEnumerable<Product>, List<ProductDTO>>(database.Products.Find(x => x.UserID == user.Id).ToList());
            var newUser = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.User.Name,
                Role = GetRoleForUser(user.Id),
                OwnedProducts = products
            };
            return newUser;
        }

        public string GetRoleForUser(string id)
        {
            var user = databaseIdentity.UserManager.FindById(id);
            var roleId = user.Roles.Where(x => x.UserId == user.Id).SingleOrDefault().RoleId;

            
            var role = databaseIdentity.RoleManager.FindById(roleId).Name;

            return role;
        }

        public void ChangeRole(string userId, string newRole)
        {
            var oldRoleId = databaseIdentity.UserManager.FindById(userId).Roles.SingleOrDefault().RoleId;
            var oldRoleName = databaseIdentity.RoleManager.FindById(oldRoleId).Name;
            if (oldRoleName != newRole)
            {
                databaseIdentity.UserManager.RemoveFromRoles(userId, oldRoleName);
                databaseIdentity.UserManager.AddToRole(userId, newRole);
            }
            databaseIdentity.SaveAsync();
            
        }
    }
}
