using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)] 
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(80)]
        public string City { get; set; } = null!;

        [MaxLength(100)]
        public string? Occupation { get; set; }

        [Column(TypeName = "DATE")]
        public DateOnly? Birthdate { get; set; }
    }
}
