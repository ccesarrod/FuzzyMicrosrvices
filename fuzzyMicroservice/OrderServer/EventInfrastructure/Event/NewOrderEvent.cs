using EventCore;
using OrderService.Models;

namespace OrderService.Events
{
    public class NewOrderStartedEvent : IntegrationEvent
    {
        public int OrderId { get; set; }

        public OrderDetailView[] Order_Detail { get; set; }
    }
}
