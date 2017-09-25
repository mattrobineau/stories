using System;

namespace Stories.Validation.BusinessRules
{
    public interface IStoryRules
    {
        bool DeletionThresholdReached(int storyId);
        bool DeletionThresholdReached(DateTime createdDate);
        bool StoryExists(int storyId);
        bool UserIsSubmitter(int storyId, Guid userId);
    }
}
