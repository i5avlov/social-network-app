using SocialNetworkApp.ViewModels.Posts;

namespace SocialNetworkApp.Core.Services.Contracts
{
    public interface IPostsService
    {
        Task AddAsync(CreatePostFormModel model);

        Task<CreatePostFormModel?> GetToEditAsync(int postId);

        Task<bool> EditAsync(int postId, CreatePostFormModel model); 

        Task<IEnumerable<PostsListEntryViewModel>> GetAllByUserIdAsync(string userId);

        Task<PostDetailsViewModel?> GetDetailsByIdAsync(int postId);

        Task<string> GetAuthorIdAsync(int postId);

        Task LikeAsync(int postId, string userId);

        Task<bool> LikeExistsAsync(int postId, string userId); 
    }
}
