using System.Threading.Tasks;

namespace EventCore.Interfaces
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> 
       where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    //public interface IIntegrationEventHandler
    //{
    //}
}
