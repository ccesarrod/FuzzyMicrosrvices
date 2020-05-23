using System;
using System.Collections.Generic;
using System.Text;

namespace EventCore.Interfaces
{
    public interface IEventPublisher
    {
        void Publish(IntegrationEvent @event);
    }
}
