namespace SocialNetworkApp.ViewModels.Messages
{
    public class MessageListEntryViewModel
    {
        public string Text { get; set; } = null!;

        public string SenderUserId { get; set; } = null!; 

        public string TimeSent { get; set; } = null!;
    }
}
