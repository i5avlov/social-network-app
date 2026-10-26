using System.ComponentModel.DataAnnotations;
using static SocialNetworkApp.Common.ValidationConstants; 
using static SocialNetworkApp.Common.ErrorMessages; 

namespace SocialNetworkApp.ViewModels.Messages
{
    public class ChatViewModel
    {
        public IEnumerable<MessageListEntryViewModel> Messages { get; set; } = new List<MessageListEntryViewModel>();

        [Required]
        public MessageSenderViewModel OtherUser { get; set; } = null!;

        [Required] 
        public MessageSenderViewModel LoggedUser { get; set; } = null!;

        [StringLength(Message.TextMaxLength, 
            MinimumLength = Message.TextMinLength, 
            ErrorMessage = LengthOutOfRangeMessage)] 
        public string? MessageText { get; set; } 
    }
}
