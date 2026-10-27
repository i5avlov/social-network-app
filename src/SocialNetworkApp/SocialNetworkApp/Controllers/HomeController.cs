using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SocialNetworkApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (this.User.Identity?.IsAuthenticated ?? false)
            {
                string? userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return RedirectToAction("Profile", "Users", new { userId = userId }); 

            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
