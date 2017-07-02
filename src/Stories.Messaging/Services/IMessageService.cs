using RabbitMQ.Client;

namespace Stories.Messaging.Services
{
    public interface IMessageService
    {
        IModel GetModel();
        void Publish(string exchange, string routingKey, string message);
    }
}
