using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace VendasService.Services
{
    public class RabbitMQProducer : IRabbitMQProducer, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQProducer(IConfiguration configuration)
        {
            var hostName = configuration["RabbitMQ:HostName"] ?? "localhost";
            _queueName = configuration["RabbitMQ:QueueName"] ?? "vendasQueue";

            var factory = new ConnectionFactory
            {
                HostName = hostName
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar a fila
            _channel.QueueDeclare(queue: _queueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public void SendMessage<T>(T message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "",
                                  routingKey: _queueName,
                                  basicProperties: properties,
                                  body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}