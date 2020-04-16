using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class OrderDetailView
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public int id { get; set; }
    }
}
