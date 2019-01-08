using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public decimal Price { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsSold { get; set; }

        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public string UserID { get; set; }
        public virtual User Owner { get; set; }

        public Product()
        {
            IsConfirmed = false;
            IsSold = false;
        }

    }
}
