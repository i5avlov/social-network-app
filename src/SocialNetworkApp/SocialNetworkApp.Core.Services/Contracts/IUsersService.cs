using SocialNetworkApp.Data.Models;
using SocialNetworkApp.ViewModels.Users;

namespace SocialNetworkApp.Core.Services.Contracts
{
    public interface IUsersService
    {
        Task<IEnumerable<UsersIndexUserViewModel>> GetAll();

        Task<ApplicationUser?> GetById(string userId); 

        Task<UserProfileViewModel?> GetProfileById(string userId); 
    }
}
