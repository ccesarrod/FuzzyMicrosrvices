using RabbitMQ.Client;
using System;

namespace EventCore.RabbitMQEventBus
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
       
    }
}
