using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class LotDTO
    {
        public int Id { get; set; }

        public ProductDTO Product { get; set; }

        public DateTime BiddingStart { get; set; }
        public DateTime BiddingEnd { get; set; }
        public decimal ActualPrice { get; set; }

        public UserDTO Winner { get; set; }
    }
}
