using EventCore.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace EventCore.RabbitMQEventBus
{
    public class RabbitMQPublisher : IEventPublisher
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private string _queueName;

        public  RabbitMQPublisher(IRabbitMQPersistentConnection persistentConnection, int retryCount)
        {
            _persistentConnection = persistentConnection;
           
            
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var eventName = @event.GetType().Name;
            using (var channel = _persistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: eventName, type: "fanout");
               
                channel.QueueDeclare(queue: eventName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: eventName, routingKey:eventName , basicProperties: null, body: body);
            }
        }


       
    }
}
