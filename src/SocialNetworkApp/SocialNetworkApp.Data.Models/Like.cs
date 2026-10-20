using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkApp.Data.Models
{
    public class Like
    {
        [Required]
        public int PostId { get; set; }

        [Required] 
        public string UserId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!; 

        [Required]
        [ForeignKey(nameof(UserId))] 
        public ApplicationUser User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now; 

        public bool IsDeleted { get; set; }
    }
}
