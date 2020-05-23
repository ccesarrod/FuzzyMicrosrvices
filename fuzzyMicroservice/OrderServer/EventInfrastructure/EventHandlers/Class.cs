using EventCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Events.EventHandlers
{
    public class OrderStartedIntegrationEventHandler 
    {
       

        //public OrderStartedIntegrationEventHandler(
        //    IBasketRepository repository,
        //    ILogger<OrderStartedIntegrationEventHandler> logger)
        //{
        //    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //}

        //public async Task Handle(OrderStartedIntegrationEvent @event)
        //{
        //    using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
        //    {
        //        _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

        //        await _repository.DeleteBasketAsync(@event.UserId.ToString());
        //    }
        //}
    }
}
