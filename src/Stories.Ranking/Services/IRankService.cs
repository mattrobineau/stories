using Stories.Data.Entities;
using System.Threading.Tasks;

namespace Stories.Ranking.Services
{
    public interface IRankService
    {
        Task<bool> CalculateCommentScore(int id);
        void CalculateCommentScore(Comment comment);
        Task<bool> CalculateStoryScore(int id);
        void CalculateStoryScore(Story story);
    }
}
