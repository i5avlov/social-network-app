using SocialNetworkApp.ViewModels.Users;

namespace SocialNetworkApp.ViewModels.Invitations
{
    public class InvitationsListEntryViewModel
    {
        public int Id { get; set; } 

        public string? Text { get; set; } 

        public UserInvitationViewModel User { get; set; } = null!; 

        public string TimeSent { get; set; } = null!; 
    }
}
