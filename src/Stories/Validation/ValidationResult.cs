using System.Collections.Generic;

namespace Stories.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public IList<string> Messages { get; set; } = new List<string>();
    }
}
