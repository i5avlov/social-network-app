using SocialNetworkApp.ViewModels.Posts;

namespace SocialNetworkApp.Core.Services.Contracts
{
    public interface IPostsService
    {
        Task AddPostAsync(CreatePostFormModel model); 
    }
}
