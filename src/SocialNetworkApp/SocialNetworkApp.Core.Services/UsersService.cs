using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data;
using SocialNetworkApp.Data.Models;
using SocialNetworkApp.ViewModels.Messages;
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

        public async Task<IEnumerable<UsersListEntryViewModel>> GetAll(string? removeUserWithId = null) 
        {
            // Get all users and map to index view model
            var users = this.context
                .Users
                .Select(u => new UsersListEntryViewModel
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    City = u.City
                }); 

            // Id of user to remove from all is present 
            if (removeUserWithId != null)
            {
                users = users.Where(u => u.Id != removeUserWithId); 
            } 

            return await users.ToListAsync(); 
        }

        public async Task<ApplicationUser?> GetById(string userId)
        {
            return await this.context
                .Users
                .FindAsync(userId);
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

        public async Task<bool> ExistsById(string userId)
        {
            return await this.GetById(userId) != null; 
        }

        public async Task<MessageSenderViewModel> GetMessageSenderUserById(string userId)
        {
            return await this.context 
                .Users 
                .Where(u => u.Id == userId) 
                .Select(u => new MessageSenderViewModel()
                {
                    Id = u.Id, 
                    Name = u.FirstName + " " + u.LastName, 
                    City = u.City 
                })
                .FirstAsync(); 
        }
    }
}
