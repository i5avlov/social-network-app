using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data.Models;
using SocialNetworkApp.ViewModels;
using SocialNetworkApp.ViewModels.Posts;
using System.Security.Claims;

namespace SocialNetworkApp.Controllers
{
    [Authorize] 
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly IUsersService usersService; 

        public PostsController(
            IPostsService postsService, 
            IUsersService usersService)
        {
            this.postsService = postsService;
            this.usersService = usersService; 
        }

        [HttpGet] 
        public async Task<IActionResult> Details(int postId)
        {
            string? loggedInUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggedInUserId == null)
            {
                return Unauthorized();
            } 

            PostDetailsViewModel? model = await this.postsService.GetDetailsByIdAsync(postId);
            if (model == null)
            {
                return NotFound(); 
            } 

            string postAuthorId = await this.postsService.GetAuthorIdAsync(postId); 

            model.IsLoggedInUser = postAuthorId == loggedInUserId;
            model.IsLikedByLoggedUser = await this.postsService.LikeExistsAsync(postId, loggedInUserId); 

            return View(model); 
        }

        [HttpGet]
        public IActionResult Add()
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(); 
            }
            CreatePostFormModel model = new CreatePostFormModel()
            { 
                UserId = userId 
            }; 
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreatePostFormModel model)
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null 
                || userId != model.UserId) 
            {
                return Unauthorized(); 
            }

            if (ModelState.IsValid == false)
            {
                return View(model); 
            }

            try
            {
                await this.postsService.AddAsync(model);
                return RedirectToAction("Profile", "Users", new { userId = userId }); 
            }
            catch 
            { 
                return View("Error"); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserPosts(string userId)
        {
            string? loggedInUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggedInUserId == null)
            {
                return Unauthorized();
            } 

            ApplicationUser? user = await this.usersService.GetById(userId);
            if (user == null)
            {
                return BadRequest(); 
            }

            UserPostsViewModel model = new UserPostsViewModel(); 

            model.Name = $"{user.FirstName} {user.LastName}";
            model.City = user.City; 
            model.Posts = await this.postsService.GetAllByUserIdAsync(userId);
            // Tell the view if logged user is the owner of listed posts 
            model.IsLoggedInUser = loggedInUserId == userId; 

            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Like(int postId, string likingUserId) 
        {
            string? loggedInUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggedInUserId == null 
                || likingUserId != loggedInUserId)
            {
                return Unauthorized(); 
            }

            try
            {
                await this.postsService.LikeAsync(postId, loggedInUserId); 
                return RedirectToAction(nameof(Details), new { postId = postId }); 
            }
            catch
            {
                return RedirectToAction("Error", "HomeController"); 
            }
        }
    } 
}
