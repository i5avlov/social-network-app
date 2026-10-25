using SocialNetworkApp.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkApp.Data.Models
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; } 

        [MaxLength(200)]
        public string? Text { get; set; }

        [Required] 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required] 
        public InvitationStatus Status { get; set; } = InvitationStatus.Waiting;

        public DateTime? StatusChangedAt { get; set; }

        [Required] 
        public string SenderUserId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(SenderUserId))]
        public ApplicationUser SenderUser { get; set; } = null!;

        [Required] 
        public string ReceiverUserId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(ReceiverUserId))]
        public ApplicationUser ReceiverUser { get; set; } = null!;
    }
}
