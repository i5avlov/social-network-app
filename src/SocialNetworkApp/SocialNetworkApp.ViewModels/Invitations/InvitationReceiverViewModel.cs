using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.ViewModels.Invitations
{
    public class InvitationReceiverViewModel
    {
        [Required] 
        public string Id { get; set; } = null!;

        public string? Name { get; set; } 

        public string? City { get; set; } 
    }
}
