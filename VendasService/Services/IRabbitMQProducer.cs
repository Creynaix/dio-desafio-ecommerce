namespace VendasService.Services
{
    public interface IRabbitMQProducer
    {
        void SendMessage<T>(T message);
    }
}
