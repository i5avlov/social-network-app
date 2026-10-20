using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Data.Models;

namespace SocialNetworkApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Like> Likes { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // Like entity configuration 
            builder.Entity<Like>(entity =>
            {
                entity.HasKey(l => new { l.PostId, l.UserId });

                entity.HasOne(l => l.Post)
                    .WithMany(l => l.UserLikes) 
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(l => l.User) 
                    .WithMany(l => l.PostLikes) 
                    .OnDelete(DeleteBehavior.Restrict); 
            }); 
        }
    }
}
