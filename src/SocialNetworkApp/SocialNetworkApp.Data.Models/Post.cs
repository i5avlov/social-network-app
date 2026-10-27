using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SocialNetworkApp.Common.ValidationConstants.Post; 

namespace SocialNetworkApp.Data.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TextMaxLength)] 
        public string Text { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        [Required] 
        public ApplicationUser User { get; set; } = null!;

        public IEnumerable<Like> UserLikes { get; set; } = new List<Like>(); 
    }
}
