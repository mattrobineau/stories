using System.Security.Claims;

namespace Stories.Identity
{
    public class AppUser : ClaimsPrincipal        
    {
        public AppUser(ClaimsPrincipal principal) : base(principal)
        {}

        public string Name
        {
            get { return FindFirst(ClaimTypes.Name).Value; }
        }

        public string NameIdentifier
        {
            get { return FindFirst(ClaimTypes.NameIdentifier).Value; }
        }
    }
}
