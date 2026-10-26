using SocialNetworkApp.ViewModels.Messages;

namespace SocialNetworkApp.Core.Services.Contracts
{
    public interface IMessagesService 
    {
        Task<IEnumerable<MessageListEntryViewModel>> GetAllBetweenUsersAsync(string userId, string otherUserId);

        Task<bool> AddAsync(string senderUserId, string receiverUserId, string messageText); 
    }
}
