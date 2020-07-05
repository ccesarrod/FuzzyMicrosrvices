using DataCore.Entities;
using EventCore.RabbitMQEventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServicesAPI.OrderAPI;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Events.EventHandlers
{
    public class OrderFulfilledEventHandler : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;

        private readonly string _hostname;
        private readonly string _queueName = "OrderFulfilledEvent";
        //
        private readonly string _username;
        private readonly string _password;
      
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public OrderFulfilledEventHandler(IRabbitMQPersistentConnection persistentConnection,
            IServiceScopeFactory serviceScopeFactory)
        {
            _persistentConnection = persistentConnection;
            _serviceScopeFactory = serviceScopeFactory;
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
           
         
            _channel = _persistentConnection.CreateModel();
            _channel.ExchangeDeclare(exchange: _queueName, type: ExchangeType.Fanout, durable: false, autoDelete: false);
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: _queueName, exchange: _queueName, routingKey: "");

        }
        //Get status
        //if status is good sedn to shipping
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var newOrderDetails = JsonConvert.DeserializeObject<OrderFulfilledEvent>(content);

                try
                {
                    await ConsumerReceived(ch, ea);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " Write an exeception for low inventory order incomplete");
                }
                finally
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }

            };
           

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs ea)
        {

            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            var newOrderDetails = JsonConvert.DeserializeObject<OrderFulfilledEvent>(content);
            HandleMessage(newOrderDetails);

        }

        private void HandleMessage(OrderFulfilledEvent newOrderModel)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    if (newOrderModel.isFulFill)
                    {
                        var orderServiceAPI = scope.ServiceProvider.GetRequiredService<IOrderAPI>();
                        var order = orderServiceAPI.GetById(newOrderModel.OrderId);
                        order.OrderDate = DateTime.Now;
                        orderServiceAPI.Update(order);
                    }
                  
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Procesing queue Order fulfillEvent" + ex.Message);
                }

            }

        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

}
