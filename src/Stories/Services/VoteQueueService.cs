using Stories.Messaging.Constants;
using Stories.Messaging.Services;
using Stories.Logging;
using System;

namespace Stories.Services
{
    public class VoteQueueService : IVoteQueueService
    {
        private readonly IMessageService MessageService;
        private readonly ILogger Logger;

        public VoteQueueService(IMessageService messageService, ILogger logger)
        {
            MessageService = messageService;
            Logger = logger;

        }

        public void QueueStoryVote(int storyId)
        {
            try
            {
                MessageService.Publish(RabbitMQExchanges.Ranking, RabbitMQRoutingKeys.Story, storyId.ToString());
            }
            catch(Exception e)
            {
                Logger.Log("Message service story publishing failed.", e);
            }
        }
        public void QueueCommentVote(int commentId)
        {
            try
            {
                MessageService.Publish(RabbitMQExchanges.Ranking, RabbitMQRoutingKeys.Comment, commentId.ToString());
            }
            catch(Exception e)
            {
                Logger.Log("Message service comment publishing failed.", e);
            }
        }
    }
}
