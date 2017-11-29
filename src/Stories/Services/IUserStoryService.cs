using Stories.Models.Stories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IUserStoryService
    {
        Task<List<StoryModel>> GetRecent(Guid forUserId, Guid callingUserId, int page, int pageSize, bool includeDeleted = false);
    }
}
