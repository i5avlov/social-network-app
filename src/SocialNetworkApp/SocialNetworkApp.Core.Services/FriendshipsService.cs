using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data;
using SocialNetworkApp.Data.Models;
using SocialNetworkApp.ViewModels.Users;

namespace SocialNetworkApp.Core.Services
{
    public class FriendshipsService : IFriendshipsService
    {
        private readonly ApplicationDbContext context;

        public FriendshipsService(ApplicationDbContext context)
        {
            this.context = context;
        } 

        public async Task<bool> AddAsync(string sourceUserId, string targetUserId)
        {
            if (sourceUserId.IsNullOrEmpty() 
                || targetUserId.IsNullOrEmpty())
            {
                throw new ArgumentException(); 
            }

            Friendship entity = new Friendship()
            { 
                SourceUserId = sourceUserId, 
                TargetUserId = targetUserId 
            };
            await this.context
                .Friendships
                .AddAsync(entity);
            return await this.context.SaveChangesAsync() == 1; 
        }

        public async Task<bool> FriendshipExists(string? userId, string? otherUserId)
        {
            if (userId.IsNullOrEmpty() 
                || otherUserId.IsNullOrEmpty())
            {
                return false; 
            } 

            return await this.context 
                .Friendships
                .AnyAsync(f => f.SourceUserId == userId && f.TargetUserId == otherUserId
                    || f.SourceUserId == otherUserId && f.TargetUserId == userId); 
        }

        public async Task<IEnumerable<UsersListEntryViewModel>> GetAllAsync(string userId)
        {
            var sourceUserFriends = await this.context
                .Friendships
                .Where(f => f.SourceUserId == userId)
                .Select(f => new UsersListEntryViewModel()
                { 
                    Id = f.TargetUser.Id, 
                    Name = f.TargetUser.FirstName + " " + f.TargetUser.LastName, 
                    City = f.TargetUser.City 
                })
                .ToListAsync(); 
            var targetUserFriends = await this.context
                .Friendships
                .Where(f => f.TargetUserId == userId)
                .Select(f => new UsersListEntryViewModel()
                {
                    Id = f.SourceUser.Id,
                    Name = f.SourceUser.FirstName + " " + f.SourceUser.LastName,
                    City = f.SourceUser.City
                })
                .ToListAsync(); 

            return sourceUserFriends.Concat(targetUserFriends); 
        }
    }
}
