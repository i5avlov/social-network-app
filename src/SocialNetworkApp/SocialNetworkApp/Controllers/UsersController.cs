using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.ViewModels.Users;

namespace SocialNetworkApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService; 
        }

        [HttpGet] 
        public async Task<IActionResult> Index()
        {
            IEnumerable<UsersIndexUserViewModel> model = await this.usersService.GetAll(); 
            return View(model); 
        }
    }
}
