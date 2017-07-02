using RabbitMQ.Client;

namespace Stories.Messaging.Providers
{
    public interface IRabbitMQConnectionProvider
    {
        IConnection CreateConnection();
    }
}
