using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data;
using SocialNetworkApp.ViewModels.Users;

namespace SocialNetworkApp.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext context;

        public UsersService(ApplicationDbContext context)
        {
            this.context = context; 
        } 

        public async Task<IEnumerable<UsersIndexUserViewModel>> GetAll()
        {
            return await this.context
                .Users
                .Select(u => new UsersIndexUserViewModel
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    City = u.City
                })
                .ToListAsync(); 
        }
    }
}
