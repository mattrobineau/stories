using Stories.Messaging.Constants;
using Stories.Messaging.Services;

namespace Stories.Services
{
    public class VoteQueueService : IVoteQueueService
    {
        private readonly IMessageService MessageService;

        public VoteQueueService(IMessageService messageService)
        {
            MessageService = messageService;
        }

        public void QueueStoryVote(int storyId)
        {
            MessageService.Publish(RabbitMQExchanges.Ranking, RabbitMQRoutingKeys.Story, storyId.ToString());
        }
        public void QueueCommentVote(int commentId)
        {
            MessageService.Publish(RabbitMQExchanges.Ranking, RabbitMQRoutingKeys.Comment, commentId.ToString());
        }
    }
}
