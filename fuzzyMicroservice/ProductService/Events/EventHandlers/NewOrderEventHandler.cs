using DataCore;
using EventCore;
using EventCore.RabbitMQEventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServicesAPI.ProductAPI;
using System;
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
        private readonly string _queueName= "NewOrderStartedEvent";
        private readonly string _username;
        private readonly string _password;
        private  IProductServiceAPI _productService;
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public NewOrderCreatedEventHandler(IRabbitMQPersistentConnection persistentConnection, IServiceScopeFactory serviceScopeFactory)
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

           var x = this.GetType().ToString();
            _channel = _persistentConnection.CreateModel();
            _channel.ExchangeDeclare(exchange: _queueName,type: ExchangeType.Fanout, durable: false, autoDelete: false);
            _channel.QueueDeclare( queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: _queueName, exchange: _queueName, routingKey: "");
        }

       

        protected override  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received +=  async (ch, ea) =>
            {
              var content = Encoding.UTF8.GetString(ea.Body.ToArray());
               var newOrderDetails = JsonConvert.DeserializeObject<OrderStartedIntegrationEvent>(content);

                try
                {
                    await Consumer_Received(ch, ea);
                }

                catch  (OutStockException ex)
                {
                    Console.WriteLine(ex.Message + " Write an exeception for low inventory order incomplete");
                }
                finally
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;
       
             _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
           
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var newOrderDetails = JsonConvert.DeserializeObject<OrderStartedIntegrationEvent>(content);
                HandleMessage(newOrderDetails);           
           
        }

        private void HandleMessage(OrderStartedIntegrationEvent newOrderModel)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
              
                _productService = scope.ServiceProvider.GetRequiredService<IProductServiceAPI>();

                Boolean result = false;
                foreach (var product in newOrderModel.Order_Detail)
                {
                    var temp = _productService.UpdateQuantity(product.id, product.quantity);
                    result = !result ? temp || result : temp && result;
                }

                //publish order status
            }
           
        }

        private async void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {

        }

        private async Task OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private async Task OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private async Task OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private async Task OnConsumerShutdown(object sender, ShutdownEventArgs e)
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
