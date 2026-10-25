using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SocialNetworkApp.Common.ValidationConstants.ApplicationUser; 
using static SocialNetworkApp.Common.ErrorMessages; 


namespace SocialNetworkApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(FirstNameMaxLength, 
            ErrorMessage = MaxLengthExceededMessage)] 
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength,
            ErrorMessage = MaxLengthExceededMessage)] 
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(CityMaxLength,
            ErrorMessage = MaxLengthExceededMessage)]
        public string City { get; set; } = null!;

        [MaxLength(OccupationMaxLength,
            ErrorMessage = MaxLengthExceededMessage)] 
        public string? Occupation { get; set; }

        [Required] 
        [Column(TypeName = BirthdateColumnTypeName)]
        public DateOnly Birthdate { get; set; }

        public IEnumerable<Post> Posts { get; set; } = new List<Post>();

        public IEnumerable<Like> PostLikes { get; set; } = new List<Like>();

        [InverseProperty("SenderUser")]
        public IEnumerable<Invitation> SentInvitations { get; set; } = new List<Invitation>();

        [InverseProperty("ReceiverUser")]
        public IEnumerable<Invitation> ReceivedInvitations { get; set; } = new List<Invitation>();

        [InverseProperty("SenderUser")]
        public IEnumerable<Message> SentMessages { get; set; } = new List<Message>();

        [InverseProperty("ReceiverUser")]
        public IEnumerable<Message> ReceivedMessages { get; set; } = new List<Message>(); 
    }
}
