using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkApp.Data.Models
{
    public class Friendship
    {
        [Required]
        public string SourceUserId { get; set; } = null!; 

        [Required]
        [ForeignKey(nameof(SourceUserId))]
        public ApplicationUser SourceUser { get; set; } = null!;

        [Required]
        public string TargetUserId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(TargetUserId))]
        public ApplicationUser TargetUser { get; set; } = null!;

        public DateTime StartedAt { get; set; } = DateTime.Now; 

        public bool IsActive { get; set; } = true; 
    }
}
