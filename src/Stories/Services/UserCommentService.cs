using Stories.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class UserCommentService
    {
        public Task<IList<CommentModel>> GetRecent(Guid forUserId, Guid callingUserId, int page, int pageSize, bool includeDeleted = false)
        {
            return null;
        }
    }
}
