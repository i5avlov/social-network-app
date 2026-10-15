using SocialNetworkApp.ViewModels.Users;

namespace SocialNetworkApp.Core.Services.Contracts
{
    public interface IUsersService
    {
        Task<IEnumerable<UsersIndexUserViewModel>> GetAll(); 
    }
}
