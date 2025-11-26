using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace EstoqueService.Services
{
    public class RabbitMQConsumer
    {
        public void Start()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "vendasQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Mensagem recebida: {message}");

                // Atualizar estoque (simulação)
                // Aqui você pode implementar a lógica para atualizar o estoque
            };

            channel.BasicConsume(queue: "vendasQueue",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("Consumidor iniciado. Pressione [enter] para sair.");
            Console.ReadLine();
        }
    }
}