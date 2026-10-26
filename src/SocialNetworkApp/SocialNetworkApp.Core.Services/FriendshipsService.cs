using Microsoft.IdentityModel.Tokens;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data;
using SocialNetworkApp.Data.Models;

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
    }
}
