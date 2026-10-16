using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data;
using SocialNetworkApp.Data.Models;
using SocialNetworkApp.ViewModels.Posts;

namespace SocialNetworkApp.Core.Services
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext context;

        public PostsService(ApplicationDbContext context)
        {
            this.context = context;
        } 

        public async Task AddAsync(CreatePostFormModel model) 
        { 
            // Fill in model data in new Post entity 
            Post entity = new Post()
            {
                Text = model.Text,
                UserId = model.UserId
            }; 
            // Add and save entity 
            await this.context
                .Posts
                .AddAsync(entity);
            await this.context.SaveChangesAsync(); 
        }
    }
}
