namespace Stories.Messaging.Constants
{
    public static class RabbitMQRoutingKeys
    {
        public const string Comment = "comment";
        public const string Story = "story";
    }

    public static class RabbitMQQueues
    {
        public const string Comments = "stories.queues.comments";
        public const string Stories = "stories.queues.stories";
    }

    public static class RabbitMQExchanges
    {
        public const string Ranking = "stories.ranking";
    }
}
