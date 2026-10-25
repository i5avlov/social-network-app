using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.ViewModels.Users;
using System.Security.Claims;

namespace SocialNetworkApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService; 
        private readonly IPostsService postsService; 
        private readonly IInvitationsService invitationsService; 

        public UsersController(
            IUsersService usersService, 
            IPostsService postsService, 
            IInvitationsService invitationsService) 
        {
            this.usersService = usersService; 
            this.postsService = postsService; 
            this.invitationsService = invitationsService; 
        }

        [HttpGet] 
        public async Task<IActionResult> Index()
        {
            IEnumerable<UsersIndexUserViewModel> model = await this.usersService
                .GetAll(); 
            return View(model); 
        }

        [HttpGet] 
        public async Task<IActionResult> Profile(string userId)
        { 
            // No user id 
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(); 
            } 

            UserProfileViewModel? model = await this.usersService
                .GetProfileById(userId); 

            // No user 
            if (model == null)
            {
                return NotFound(); 
            } 

            // Attach user posts to view model 
            model.Posts = await this.postsService.GetAllByUserIdAsync(userId);
            // Attach waiting received invitations 
            model.WaitingInvitations = await this.invitationsService.GetAllWaitingByUserIdAsync(userId); 

            // Whether profile belongs to logged in user
            string? loggedInUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);  
            model.IsLoggedInUser = loggedInUserId == userId; 

            return View(model); 
        }
    }
}
