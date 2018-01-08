using System.Collections.Generic;

namespace Stories.Models.ViewModels.User
{
    public class CommentHistoryViewModel
    {
        public IList<CommentViewModel> Comments { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }
}
