namespace Stories.Services
{
    public interface IVoteQueueService
    {
        void QueueStoryVote(int storyId);
        void QueueCommentVote(int commentId);
    }
}
