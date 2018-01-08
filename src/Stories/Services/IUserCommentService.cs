using Stories.Models.Comment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IUserCommentService
    {
        Task<IList<CommentModel>> GetRecent(Guid forUserId, Guid callingUserId, int page, int pageSize, bool includeDeleted = false);
        Task<int> GetRecentCommentPageCount(Guid forUserId, bool includeDeleted = false);
    }
}
