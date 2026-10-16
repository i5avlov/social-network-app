using System.ComponentModel.DataAnnotations;
using static SocialNetworkApp.Common.ValidationConstants.Post;
using static SocialNetworkApp.Common.ErrorMessages; 

namespace SocialNetworkApp.ViewModels.Posts
{
    public class CreatePostFormModel
    {
        [Required] 
        public string UserId { get; set; } = null!;

        [Required]
        [StringLength(TextMaxLength, MinimumLength = TextMinLength, 
            ErrorMessage = LengthOutOfRangeMessage)] 
        public string Text { get; set; } = string.Empty; 
    }
}
