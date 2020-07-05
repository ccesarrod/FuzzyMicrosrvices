using EventCore;

namespace OrderService.Events
{
    public class OrderFulfilledEvent : IntegrationEvent
    {
        public int OrderId { get; set; }
        public bool isFulFill { get; set; }
    }
}
