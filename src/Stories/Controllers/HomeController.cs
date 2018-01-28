using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stories.Services;
using System;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class HomeController : BaseController
    {
        private IStoryService StoryService;
        private ILogger<HomeController> Logger { get; }

        public HomeController(IStoryService storyService, ILogger<HomeController> logger)
        {
            StoryService = storyService;
            Logger = logger;
        }

        public async Task<IActionResult> Index(int page)
        {
            if (CurrentUser.Identity.IsAuthenticated && Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return View(await StoryService.GetTop(page, 20, userId));
            }

            Logger.LogInformation("Test");
            return View(await StoryService.GetTop(page, 20, null));
        }

        public async Task<IActionResult> New(int page)
        {
            if (CurrentUser.Identity.IsAuthenticated && Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return View(await StoryService.GetNew(page, 20, userId));
            }
            return View(await StoryService.GetNew(page, 20, null));
        }

        public IActionResult Guidelines()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
