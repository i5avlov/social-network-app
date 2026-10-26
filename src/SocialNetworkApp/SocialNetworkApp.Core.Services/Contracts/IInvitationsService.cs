using SocialNetworkApp.Data.Models;
using SocialNetworkApp.ViewModels.Invitations;

namespace SocialNetworkApp.Core.Services.Contracts
{
    public interface IInvitationsService
    {
        Task<InvitationReceiverViewModel> GetReceiverUserAsync(string receiverUserId);

        Task CreateAsync(string senderUserId, string receiverUserId, string? text);

        Task<IEnumerable<InvitationsListEntryViewModel>> GetAllWaitingByUserIdAsync(string userId);

        Task<InvitationDetailsViewModel> GetDetailsById(int invitationId); 

        Task<bool> AcceptAsync(int invitationId); 

        Task<bool> RejectAsync(int invitationId);

        Task<Invitation?> GetBySenderIdReceiverId(string senderUserId, string receiverUserId); 

        bool IsRejectedByUserIdAsync(Invitation? invitation, string userId); 

        bool IsSentByUserIdAsync(Invitation? invitation, string userId); 
    } 
}
