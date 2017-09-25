using System;

namespace Stories.Models.Votes
{
    public class ToggleVoteModel
    {
        public string HashId { get; set; }
        public Guid UserId { get; set; }
    }
}
