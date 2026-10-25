using SocialNetworkApp.ViewModels.Invitations;
using SocialNetworkApp.ViewModels.Posts;

namespace SocialNetworkApp.ViewModels.Users
{
    public class UserProfileViewModel 
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!; 

        public string City { get; set; } = null!;

        public string? Birthdate { get; set; }

        public IEnumerable<PostsListEntryViewModel> Posts { get; set; } = new List<PostsListEntryViewModel>();

        public IEnumerable<InvitationsListEntryViewModel> WaitingInvitations { get; set; } = new List<InvitationsListEntryViewModel>(); 

        public bool IsLoggedInUser { get; set; } 
    }
}
