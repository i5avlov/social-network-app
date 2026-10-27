using SocialNetworkApp.Data.Models;
using SocialNetworkApp.ViewModels.Messages;
using SocialNetworkApp.ViewModels.Users;

namespace SocialNetworkApp.Core.Services.Contracts
{
    public interface IUsersService
    {
        Task<IEnumerable<UsersListEntryViewModel>> GetAll(string? removeUserWithId); 

        Task<ApplicationUser?> GetById(string userId); 

        Task<bool> ExistsById(string userId); 

        Task<UserProfileViewModel?> GetProfileById(string userId);

        Task<MessageSenderViewModel> GetMessageSenderUserById(string userId); 
    }
}
