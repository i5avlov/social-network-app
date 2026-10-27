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
        private readonly IFriendshipsService friendshipsService; 

        public UsersController(
            IUsersService usersService, 
            IPostsService postsService, 
            IInvitationsService invitationsService, 
            IFriendshipsService friendshipsService) 
        {
            this.usersService = usersService; 
            this.postsService = postsService; 
            this.invitationsService = invitationsService;
            this.friendshipsService = friendshipsService; 
        }

        [HttpGet] 
        public async Task<IActionResult> Index()
        {
            string? loggedInUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); 
            // All users when no user is logged in
            // or all except the logged in user 
            IEnumerable<UsersListEntryViewModel> model = await this.usersService.GetAll(loggedInUserId);
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
            // Attach friends 
            model.Friends = await this.friendshipsService.GetAllAsync(userId); 

            // Whether profile belongs to logged in user
            string? loggedInUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);  
            model.IsLoggedInUser = loggedInUserId == userId; 

            return View(model); 
        }
    }
}
