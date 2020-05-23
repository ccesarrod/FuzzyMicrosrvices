using EventCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServicesAPI.ProductAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Events.EventHandlers
{
    public class NewOrderCreatedEventHandler : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
       
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly IProductServiceAPI _productService;

        public NewOrderCreatedEventHandler(IProductServiceAPI productService)
        {
            _productService = productService;
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _queueName,type: ExchangeType.Direct, durable: false, autoDelete: false);
            _channel.QueueDeclare( queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
           
        }

       

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var newOrderDetails = JsonConvert.DeserializeObject<OrderStartedIntegrationEvent>(content);

                HandleMessage(newOrderDetails);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(OrderStartedIntegrationEvent newOrderModel)
        {
            Boolean result= false;
          foreach(var product in newOrderModel.Details)
            {
               var temp = _productService.UpdateQuantity(product.id, product.quantity);
                result= !result? temp || result: temp && result;
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {

        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
