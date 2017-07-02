namespace Stories.Messaging.Configuration
{
    public class AMQPOptions
    {
        ///amqp://user:pass@hostName:port/vhost
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string Exchange { get; set; }
    }
}
