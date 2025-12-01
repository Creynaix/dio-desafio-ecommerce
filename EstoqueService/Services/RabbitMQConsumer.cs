using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EstoqueService.Data;
using Microsoft.Extensions.Logging;

namespace EstoqueService.Services
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQConsumer> _logger;
        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMQConsumer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<RabbitMQConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var hostName = _configuration["RabbitMQ:HostName"] ?? "localhost";
            var queueName = _configuration["RabbitMQ:QueueName"] ?? "vendasQueue";

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Mensagem recebida: {message}");

                try
                {
                    var vendaMessage = JsonSerializer.Deserialize<VendaMessage>(message);
                    if (vendaMessage != null)
                    {
                        await AtualizarEstoque(vendaMessage);
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        _logger.LogInformation($"Estoque atualizado para pedido {vendaMessage.PedidoId}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem");
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            _channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            _logger.LogInformation("RabbitMQ Consumer iniciado");
            return Task.CompletedTask;
        }

        private async Task AtualizarEstoque(VendaMessage vendaMessage)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EstoqueContext>();

            foreach (var item in vendaMessage.Itens)
            {
                var produto = await context.Produtos.FindAsync(item.ProdutoId);
                if (produto != null)
                {
                    produto.Quantidade -= item.Quantidade;
                    _logger.LogInformation($"Produto {produto.Nome} (ID: {produto.Id}): {produto.Quantidade + item.Quantidade} -> {produto.Quantidade}");
                }
            }

            await context.SaveChangesAsync();
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }

    public class VendaMessage
    {
        public int PedidoId { get; set; }
        public List<ItemVendaMessage> Itens { get; set; } = new();
    }

    public class ItemVendaMessage
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}