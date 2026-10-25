using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.ViewModels.Invitations
{
    public class CreateInvitationFormModel
    {
        [MaxLength(200)]
        public string? Text { get; set; }

        [Required] 
        public string SenderUserId { get; set; } = null!;

        [Required] 
        public InvitationReceiverViewModel ReceiverUser { get; set; } = null!; 
    }
}
