using System.Collections.Generic;

namespace Stories.Models.ViewModels
{
    public class StoriesViewModel
    {
        public List<StorySummaryViewModel> Stories { get; set; } = new List<StorySummaryViewModel>();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
