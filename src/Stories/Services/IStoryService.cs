using Stories.Models.Story;
using Stories.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IStoryService
    {
        Task<StorySummaryViewModel> Create(CreateStoryModel model);
        Task<StoryViewModel> Get(string hashId, Guid? userId);
        Task<StoriesViewModel> GetNew(int page, int pageSize, Guid? userId);
        Task<StoriesViewModel> GetTop(int page, int pageSize, Guid? userId);
        Task<bool> Delete(string hashId);
    }
}