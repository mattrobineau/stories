using Microsoft.Extensions.Logging;
using Stories.Messaging.Constants;
using Stories.Messaging.Services;
using System;

namespace Stories.Services
{
    public class VoteQueueService : IVoteQueueService
    {
        private readonly IMessageService MessageService;
        private readonly ILogger<VoteQueueService> Logger;

        public VoteQueueService(IMessageService messageService, ILogger<VoteQueueService> logger)
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
                Logger.LogCritical("Message service story publishing failed.", e);
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
                Logger.LogCritical("Message service comment publishing failed.", e);
            }
        }
    }
}
