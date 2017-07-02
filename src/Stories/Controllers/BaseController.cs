using Microsoft.AspNetCore.Mvc;
using Stories.Attributes;
using Stories.Constants;
using Stories.Identity;
using System.Security.Claims;

namespace Stories.Controllers
{
    [Roles(Roles = Roles.User)]
    public abstract class BaseController : Controller
    {
        public AppUser CurrentUser
        {
            get { return new AppUser(this.User as ClaimsPrincipal); }
        }
    }
}
