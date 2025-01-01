using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Core.Entities;
using Application.Event;
using Application.Services;

namespace Application.ListenerRabbitMQ
{
    public class OrderCreatedListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string Queue = "creating-orders-service/order-created";
        private const string RoutingKeySubscribe = "order-created";
        private readonly IServiceProvider _serviceProvider;
        private const string TrackingsExchange = "orders-service";

        public OrderCreatedListener(IServiceProvider serviceProvider)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = connectionFactory.CreateConnection("order-created-consumer");

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: TrackingsExchange,
                type: "direct",
                durable: true 
            );

            _channel.QueueDeclare(
                queue: Queue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _channel.QueueBind(Queue, TrackingsExchange, RoutingKeySubscribe);

            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                await ProcessMessage(eventArgs);
            };

            _channel.BasicConsume(Queue, false, consumer);

            return Task.CompletedTask;
        }

        private async Task ProcessMessage(BasicDeliverEventArgs eventArgs)
        {
            try
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var @event = JsonConvert.DeserializeObject<OrderCreatedEvent>(contentString);

                var eventJson = JsonConvert.SerializeObject(@event, Formatting.Indented);
                Console.WriteLine($"Order received:\n{eventJson}");

                Complete(@event).Wait();

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                _channel.BasicNack(eventArgs.DeliveryTag, false, requeue: false);
            }
        }

        public async Task Complete(OrderCreatedEvent @event)
        {
            using var scope = _serviceProvider.CreateScope();

            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

            var order = await orderService.AddAsync(@event);
        }
    }
}
