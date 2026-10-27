using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.ViewModels.Users;
using System.Security.Claims;

namespace SocialNetworkApp.Controllers
{
    [Authorize] 
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
        [AllowAnonymous] 
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

            string? loggedInUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Whether profile belongs to logged in user 
            model.IsLoggedInUser = loggedInUserId == userId; 
            // Whether logged in user and profile owner are friends 
            model.IsFriendsWithUser = await this.friendshipsService.FriendshipExists(loggedInUserId, userId); 

            return View(model); 
        }
    }
}
