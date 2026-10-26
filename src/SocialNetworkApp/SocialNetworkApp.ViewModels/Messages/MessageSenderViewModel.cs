using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.ViewModels.Messages
{
    public class MessageSenderViewModel
    {
        [Required]
        public string Id { get; set; } = null!; 

        public string? Name { get; set; } 

        public string? City { get; set; } 
    }
}
