using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ICollection<ProductDTO> Products { get; set; }
        public CategoryDTO()
        {
            Products = new List<ProductDTO>();
        }
    }
}
