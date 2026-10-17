namespace SocialNetworkApp.ViewModels.Posts
{
    public class UserPostsViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string City { get; set; } = null!;

        public IEnumerable<PostsListEntryViewModel> Posts { get; set; } = new List<PostsListEntryViewModel>();

        public bool IsLoggedInUser { get; set; } 
    }
}
