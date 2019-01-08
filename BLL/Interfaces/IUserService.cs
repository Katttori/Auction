using BLL.DTOs;
using BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<RegistrationDetails> CreateUserAsync(UserDTO newUser);
        
        void EditUser(UserDTO newUser);
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetSingle(string id);


        Task<Tuple<ClaimsIdentity, ClaimsIdentity>> FindAsync(string username, string password);
        UserDTO GetUserByName(string name);
        Task EditRole(string userId, string newRoleName);
        IEnumerable<string> GetRoles();

        void Dispose();
    }
}
