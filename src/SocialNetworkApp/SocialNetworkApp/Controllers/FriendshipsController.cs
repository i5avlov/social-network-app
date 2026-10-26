using Microsoft.AspNetCore.Mvc;

namespace SocialNetworkApp.Controllers
{
    public class FriendshipsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
