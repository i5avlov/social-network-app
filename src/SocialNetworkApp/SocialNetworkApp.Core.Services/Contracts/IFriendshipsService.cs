using SocialNetworkApp.ViewModels.Users;

namespace SocialNetworkApp.Core.Services.Contracts
{
    public interface IFriendshipsService
    {
        Task<bool> AddAsync(string sourceUserId, string targetUserId);

        Task<IEnumerable<UsersListEntryViewModel>> GetAllAsync(string userId); 
    }
}
