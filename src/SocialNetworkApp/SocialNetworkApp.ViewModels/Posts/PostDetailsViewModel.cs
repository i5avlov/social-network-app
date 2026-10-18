namespace SocialNetworkApp.ViewModels.Posts
{
    public class PostDetailsViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;

        public string AuthorName { get; set; } = null!; 

        public bool IsLoggedInUser { get; set; } 
    }
}
