using System;
using System.Collections.Generic;

namespace Stories.Data.Entities
{
    public class Story : ITimestamp
    {
        public Story() {}

        public int Id { get; set; }

        public string Description { get; set; }
        public string DescriptionMarkdown { get; set; }
        public string Title { get; set; }
        public int Upvotes { get; set; }
        public Guid UserId { get; set; }
        public bool UserIsAuthor { get; set; }
        public string Url { get; set; }        

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        public virtual StoryScore Score { get; set; }
        public virtual User User { get; set; }
        public virtual List<Vote> Votes { get; set; } = new List<Vote>();

        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
    }    
}
