using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stories.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            // handle different codes or just return the default error view
            return View();
        }
    }
}