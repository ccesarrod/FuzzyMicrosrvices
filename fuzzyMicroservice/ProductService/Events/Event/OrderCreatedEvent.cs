using EventCore;
using System;

namespace ProductService.Events.Event
{
    public class OrderSuccedeEvent:IntegrationEvent
    {
       public  int OrderId { get; set; }
       public  DateTime OrderCreated { get; set; }
    }
}
