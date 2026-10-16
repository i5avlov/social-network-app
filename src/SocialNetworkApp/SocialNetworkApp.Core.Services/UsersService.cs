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
            // TODO: Exclude current user 
            // Get all users and map to index view model
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

        public async Task<UserProfileViewModel?> GetProfileById(string userId)
        { 
            // Filter target user and map to profile view model 
            // formatting user's birth date 
            return await this.context
                .Users
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileViewModel()
                {
                    Id = userId, 
                    FirstName = u.FirstName, 
                    LastName = u.LastName, 
                    City = u.City, 
                    Birthdate = FormatBirthdate(u.Birthdate)
                })
                .FirstOrDefaultAsync(); 
        }

        private static string FormatBirthdate(DateOnly birthdate)
        {
            return birthdate.ToString("MMMM dd, yyyy"); 
        }
    }
}
