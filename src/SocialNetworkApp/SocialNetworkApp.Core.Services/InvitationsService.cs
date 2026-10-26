using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data;
using SocialNetworkApp.Data.Models;
using SocialNetworkApp.Data.Models.Enums;
using SocialNetworkApp.ViewModels.Invitations;
using SocialNetworkApp.ViewModels.Users;

namespace SocialNetworkApp.Core.Services
{
    public class InvitationsService : IInvitationsService
    {
        private readonly ApplicationDbContext context;

        public InvitationsService(ApplicationDbContext context)
        {
            this.context = context; 
        }

        public async Task CreateAsync(string senderUserId, string receiverUserId, string? text)
        {
            ApplicationUser? senderUser = await this.context
                .Users
                .FindAsync(senderUserId);
            ApplicationUser? receiverUser = await this.context
                .Users
                .FindAsync(receiverUserId); 
            // No one/both users 
            if (senderUser == null 
                || receiverUser == null)
            {
                // TODO: Exception message 
                throw new ArgumentException(); 
            }

            Invitation entity = new Invitation()
            {
                Text = text, 
                SenderUserId = senderUserId, 
                ReceiverUserId = receiverUserId 
            };
            await this.context.Invitations.AddAsync(entity);
            await this.context.SaveChangesAsync(); 
        }

        public async Task<InvitationReceiverViewModel> GetReceiverUserAsync(string receiverUserId)
        {
            ApplicationUser? receiverUser = await this.context
                .Users
                .FindAsync(receiverUserId);
            // No user 
            if (receiverUser == null)
            {
                // TODO: Exception message 
                throw new ArgumentException(); 
            }

            return new InvitationReceiverViewModel()
            {
                Id = receiverUser.Id,
                Name = $"{receiverUser.FirstName} {receiverUser.LastName}",
                City = receiverUser.City
            }; 
        }

        public async Task<IEnumerable<InvitationsListEntryViewModel>> GetAllWaitingByUserIdAsync(string userId)
        { 
            return await this.context
                .Users 
                .AsNoTracking() 
                .Where(u => u.Id == userId)
                .SelectMany(u => u.ReceivedInvitations)
                .Where(inv => inv.Status == InvitationStatus.Waiting)
                .Select(inv => new InvitationsListEntryViewModel()
                { 
                    Id = inv.Id, 
                    Text = inv.Text,
                    TimeSent = inv.CreatedAt.ToShortTimeString(),
                    User = new UserInvitationViewModel()
                    {
                        Name = $"{inv.SenderUser.FirstName} {inv.SenderUser.LastName}",
                        City = inv.SenderUser.City
                    }
                })
                .ToListAsync(); 
        }

        public async Task<bool> AcceptAsync(int invitationId)
        {
            Invitation? invitation = await this.context
                .Invitations
                .FindAsync(invitationId);
            if (invitation == null)
            {
                throw new ArgumentException();
            }
            invitation.Status = InvitationStatus.Accepted;
            invitation.StatusChangedAt = DateTime.Now;
            return await this.context.SaveChangesAsync() > 0;
        } 

        public async Task<bool> RejectAsync(int invitationId)
        {
            Invitation? invitation = await this.context
                .Invitations
                .FindAsync(invitationId);
            if (invitation == null)
            {
                throw new ArgumentException();
            }
            invitation.Status = InvitationStatus.Rejected;
            invitation.StatusChangedAt = DateTime.Now;
            return await this.context.SaveChangesAsync() > 0;
        } 

        public async Task<Invitation?> GetBySenderIdReceiverId(string senderUserId, string receiverUserId)
        {
            return await this.context
                .Invitations
                .FirstOrDefaultAsync(inv => inv.SenderUserId == senderUserId && inv.ReceiverUserId == receiverUserId
                    || inv.SenderUserId == receiverUserId && inv.ReceiverUserId == senderUserId); 
        }

        public bool IsRejectedByUserIdAsync(Invitation? invitation, string userId)
        {
            return invitation != null 
                && invitation.Status == InvitationStatus.Rejected 
                && invitation.ReceiverUserId == userId; 
        }

        public bool IsSentByUserIdAsync(Invitation? invitation, string userId)
        {
            return invitation != null 
                && invitation.SenderUserId == userId; 
        }

        public async Task<InvitationDetailsViewModel> GetDetailsById(int invitationId)
        {
            return await this.context
                .Invitations
                .Where(inv => inv.Id == invitationId)
                .Select(inv => new InvitationDetailsViewModel()
                {
                    SenderUserId = inv.SenderUserId,
                    ReceiverUserId = inv.ReceiverUserId
                })
                .FirstAsync(); 

        }
    }
}
