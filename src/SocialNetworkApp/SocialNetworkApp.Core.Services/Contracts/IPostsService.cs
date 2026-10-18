using SocialNetworkApp.ViewModels.Posts;

namespace SocialNetworkApp.Core.Services.Contracts
{
    public interface IPostsService
    {
        Task AddAsync(CreatePostFormModel model);

        Task<IEnumerable<PostsListEntryViewModel>> GetAllByUserIdAsync(string userId);

        Task<PostDetailsViewModel?> GetDetailsByIdAsync(int postId);

        Task<string> GetAuthorIdAsync(int postId); 
    }
}
