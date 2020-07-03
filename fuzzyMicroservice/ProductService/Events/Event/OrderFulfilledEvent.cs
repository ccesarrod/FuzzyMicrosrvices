using EventCore;
using System;

namespace ProductService.Events.Event
{
    public class OrderFulfilledEvent:IntegrationEvent
    {
       public  int OrderId { get; set; }
       public  bool isFulFill { get; set; }
    }
}
