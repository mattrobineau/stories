using Stories.Models.Votes;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IVoteService
    {
        Task<bool> ToggleStoryVote(ToggleVoteModel model);
        Task<bool> ToggleCommentVote(ToggleVoteModel model);
    }
}
