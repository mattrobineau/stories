using System.Collections.Generic;

namespace Stories.Models.ViewModels
{
    public class StoryViewModel
    {        
        public StorySummaryViewModel Summary { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
