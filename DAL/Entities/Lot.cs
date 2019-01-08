using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Lot
    {
        [Key]
        public int Id { get; set; }

        public int? ProductID { get; set; }
        public virtual Product Product { get; set; }

        public DateTime BiddingStart { get; set; }
        public DateTime BiddingEnd { get; set; }
        public decimal ActualPrice { get; set; }

        public string UserID { get; set; }
        public virtual User Winner { get; set; }
    }
}
