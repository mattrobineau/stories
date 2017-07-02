using RabbitMQ.Client;
using Stories.Messaging.Configuration;

namespace Stories.Messaging.Providers
{
    public sealed class RabbitMQConnectionProvider : IRabbitMQConnectionProvider
    {
        private readonly AMQPOptions Options;

        public RabbitMQConnectionProvider(AMQPOptions options)
        {
            Options = options;
        }

        public IConnection CreateConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            // "guest"/"guest" by default, limited to localhost connections
            factory.UserName = Options.UserName;
            factory.Password = Options.Password;
            factory.HostName = Options.HostName;
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;// Options.Port;

            return factory.CreateConnection();
        }
    }
}
