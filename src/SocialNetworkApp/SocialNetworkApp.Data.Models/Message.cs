using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SocialNetworkApp.Common.ValidationConstants.Message;
using static SocialNetworkApp.Common.ErrorMessages;  

namespace SocialNetworkApp.Data.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TextMaxLength, 
            ErrorMessage = MaxLengthExceededMessage)] 
        public string Text { get; set; } = null!;

        public DateTime TimeSent { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false; 

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
