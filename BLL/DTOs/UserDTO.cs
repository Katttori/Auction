using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public virtual ICollection<ProductDTO> OwnedProducts { get; set; }
        public virtual ICollection<LotDTO> WonLots { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }


        public UserDTO()
        {
            OwnedProducts = new List<ProductDTO>();
            WonLots = new List<LotDTO>();
        }
    }
}
