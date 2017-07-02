using System;

namespace Stories.Data.Entities
{
    public interface ITimestamp
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}
