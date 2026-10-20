using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<PostsListEntryViewModel>> GetAllByUserIdAsync(string userId)
        {
            return await this.context
                .Posts
                .Where(p => p.UserId == userId) 
                .OrderByDescending(p => p.Id)  
                .Select(p => new PostsListEntryViewModel()
                {
                    Id = p.Id,
                    Text = CutText(p.Text, 50)  
                })
                .ToListAsync(); 
        }

        public async Task<PostDetailsViewModel?> GetDetailsByIdAsync(int postId)
        {
            return await this.context
                .Posts
                .Where(p => p.Id == postId)
                .Select(p => new PostDetailsViewModel()
                {
                    Id = p.Id, 
                    Text = p.Text, 
                    AuthorName = $"{p.User.FirstName} {p.User.LastName}" 
                })
                .FirstOrDefaultAsync();
        }

        private static string CutText(string text, int length)
        {
            if (text.Length > length)
            {
                text = text.Substring(0, length) + "..."; 
            }
            return text; 
        }

        public async Task<string> GetAuthorIdAsync(int postId)
        {
            Post? entity = await this.context
                .Posts
                .FindAsync(postId);

            if (entity == null)
            {
                throw new ArgumentException();
            }

            return entity.UserId; 
        }

        public async Task LikeAsync(int postId, string userId)
        {
            Post? post = await this.context
                .Posts
                .FindAsync(postId);
            // No post  
            if (post == null)
            {
                throw new ArgumentException(); 
            } 

            Like? existingLikeEntity = await this.context
                .Likes
                .SingleOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            
            if (existingLikeEntity == null)
            {
                // Post liked by user 
                Like newLikeEntity = new Like()
                {
                    PostId = postId,
                    UserId = userId
                };

                await this.context
                    .Likes
                    .AddAsync(newLikeEntity);
            } 
            else if (existingLikeEntity.IsDeleted == false)
            {
                // Post unliked by user 
                existingLikeEntity.IsDeleted = true;
            } 
            else if (existingLikeEntity.IsDeleted == true)
            {
                // Unliked post again liked by user 
                existingLikeEntity.IsDeleted = false;
            } 

            await this.context.SaveChangesAsync(); 
        }

        public async Task<bool> LikeExistsAsync(int postId, string userId)
        { 
            Like? like = await this.context
                .Likes
                .SingleOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            return like != null && like.IsDeleted == false; 
        }
    }
}
