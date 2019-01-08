using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public class LotModel
    {
        public int Id { get; set; }
        public ProductModel Product { get; set; }
        public string BiddingEnd { get; set; }
        public decimal ActualPrice { get; set; }
        public UserModel Winner { get; set; }
    }
}