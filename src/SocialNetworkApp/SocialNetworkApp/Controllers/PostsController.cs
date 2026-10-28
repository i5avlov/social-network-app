using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data.Models;
using SocialNetworkApp.ViewModels.Posts;
using System.Security.Claims;

namespace SocialNetworkApp.Controllers
{
    [Authorize] 
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly IUsersService usersService;
        private readonly IFriendshipsService friendshipsService; 
        public PostsController(
            IPostsService postsService, 
            IUsersService usersService,
            IFriendshipsService friendshipsService)
        {
            this.postsService = postsService;
            this.usersService = usersService;
            this.friendshipsService = friendshipsService; 
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
            model.UserId = postAuthorId; 
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
                int postId = await this.postsService.AddAsync(model);
                return RedirectToAction(nameof(Details), new { postId = postId }); 
            }
            catch 
            { 
                return View("Error"); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int postId)
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            CreatePostFormModel? model = await this.postsService.GetToEditAsync(postId);
            if (model == null 
                || model.UserId != userId)
            {
                return BadRequest(); 
            }
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int postId, CreatePostFormModel model) 
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            if (model.UserId != userId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid == false)
            {
                return View(model); 
            }
            await this.postsService.EditAsync(postId, model);
            return RedirectToAction(nameof(Details), new { postId = postId }); 
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
            string? loggedUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggedUserId == null 
                || likingUserId != loggedUserId)
            {
                return Unauthorized(); 
            }

            bool isUserOwnPost = await this.postsService.GetAuthorIdAsync(postId) == likingUserId; 
            if (isUserOwnPost)
            {
                TempData["OwnPostLikeNotAllowed"] = "Yoou cannot like your own post."; 
                return RedirectToAction(nameof(Details), new { postId = postId }); 
            }

            bool areFriendsUsers = await this.friendshipsService.FriendshipExists(loggedUserId, likingUserId);
            // Users are not friends 
            if (areFriendsUsers == false)
            {
                TempData["PostLikeOnlyToFriendAllowed"] = "You cannot like a post of a user that is not your friend.";
                return RedirectToAction(nameof(Details), new { postId = postId });
            }

            try
            {
                await this.postsService.LikeAsync(postId, loggedUserId); 
                return RedirectToAction(nameof(Details), new { postId = postId }); 
            }
            catch
            {
                return RedirectToAction("Error", "HomeController"); 
            }
        }
    } 
}
