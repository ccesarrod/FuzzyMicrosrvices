using System;
using System.Collections.Generic;
using System.Text;

namespace DataCore.Entities
{
    public partial class Order 
    {
        public Order()
        {
            this.Order_Details = new List<OrderDetail>();
        }

        public int OrderID { get; set; }
        public string CustomerID { get; set; }

        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> Order_Details { get; set; }

    }
}
