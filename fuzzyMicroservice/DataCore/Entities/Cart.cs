using System;
using System.Collections.Generic;
using System.Text;

namespace DataCore.Entities
{
    public class Cart
    {
        public int ProductId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int Id { get; set; }
    }
}
