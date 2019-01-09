using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsSold { get; set; }

        public CategoryDTO Category { get; set; }

        public UserDTO Owner { get; set; }

        public ProductDTO()
        {
            IsConfirmed = false;
            IsSold = false;
        c}
    }
}
