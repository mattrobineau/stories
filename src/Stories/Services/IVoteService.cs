using System;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IVoteService
    {
        Task<bool> ToggleStoryVote(string hashId, Guid userId);
        Task<bool> ToggleCommentVote(string hashId, Guid userId);
    }
}
