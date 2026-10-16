using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.ViewModels;
using SocialNetworkApp.ViewModels.Posts;
using System.Security.Claims;

namespace SocialNetworkApp.Controllers
{
    [Authorize] 
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService; 
        } 

        public IActionResult Index()
        {
            return View();
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
    }
}
