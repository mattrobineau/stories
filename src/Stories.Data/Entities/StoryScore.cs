using System;

namespace Stories.Data.Entities
{
    public class StoryScore : ITimestamp
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public double Value { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Story Story { get; set; }
    }
}
