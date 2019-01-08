using DAL.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string Name { get; set; }
        public virtual ICollection<Product> OwnedProducts { get; set; }
        public virtual ICollection<Lot> WonLots { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public User()
        {
            OwnedProducts = new List<Product>();
            WonLots = new List<Lot>();
        }
    }
}
