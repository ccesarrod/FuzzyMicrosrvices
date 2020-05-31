using EventCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Events
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
      public  int OrderId { get; set; }
      public  OrderDetail[] Order_Detail { get; set; }
    }

    public class OrderDetail
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public int id { get; set; }
    }

}
