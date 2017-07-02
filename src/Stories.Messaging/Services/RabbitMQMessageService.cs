using RabbitMQ.Client;
using Stories.Messaging.Providers;
using System.Text;

namespace Stories.Messaging.Services
{
    public sealed class RabbitMQMessageService : IMessageService
    {
        private readonly IRabbitMQConnectionProvider ConnectionProvider;

        public RabbitMQMessageService(IRabbitMQConnectionProvider connectionProvider)
        {
            ConnectionProvider = connectionProvider;
        }

        public IModel GetModel()
        {
            var connection = ConnectionProvider.CreateConnection();
            return connection.CreateModel();
        }

        public void Publish(string exchange, string routingKey, string message)
        {
            var connection = ConnectionProvider.CreateConnection();
            var messageModel = connection.CreateModel();

            var properties = messageModel.CreateBasicProperties();
            properties.Persistent = true;

            var messageBytes = Encoding.UTF8.GetBytes(message);

            messageModel.BasicPublish(exchange, routingKey, properties, messageBytes);
        }
    }
}
