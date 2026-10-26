using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data;
using SocialNetworkApp.Data.Models;
using SocialNetworkApp.ViewModels.Messages;

namespace SocialNetworkApp.Core.Services
{  
    public class MessagesService : IMessagesService 
    {
        private readonly ApplicationDbContext context;

        public MessagesService(ApplicationDbContext context)
        {
            this.context = context; 
        } 

        public async Task<bool> AddAsync(string senderUserId, string receiverUserId, string messageText)
        {
            if (senderUserId.IsNullOrEmpty() 
                || receiverUserId.IsNullOrEmpty() 
                || messageText.IsNullOrEmpty())
            {
                throw new ArgumentException(); 
            }

            Message entity = new Message()
            { 
                SenderUserId = senderUserId, 
                ReceiverUserId = receiverUserId, 
                Text = messageText 
            };

            await this.context.Messages.AddAsync(entity);
            return await this.context.SaveChangesAsync() == 1; 
        }

        public async Task<IEnumerable<MessageListEntryViewModel>> GetAllBetweenUsersAsync(string userId, string otherUserId)
        {
            if (userId.IsNullOrEmpty()
                || otherUserId.IsNullOrEmpty()) 
            {
                throw new ArgumentException();
            }

            return await this.context
                .Messages
                .Where(m => m.SenderUserId == userId && m.ReceiverUserId == otherUserId
                    || m.SenderUserId == otherUserId && m.ReceiverUserId == userId) 
                .OrderByDescending(m => m.TimeSent) 
                .Select(m => new MessageListEntryViewModel()
                {
                    SenderUserId = m.SenderUserId, 
                    Text = m.Text, 
                    TimeSent = m.TimeSent.ToShortTimeString() 
                }) 
                .ToListAsync(); 
        }
    }
}
