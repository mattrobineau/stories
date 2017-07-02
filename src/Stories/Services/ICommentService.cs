using Stories.Models.ViewModels;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface ICommentService
    {
        Task<CommentViewModel> Add(AddCommentViewModel model);
        Task<CommentViewModel> Get(string hashId);
    }
}
