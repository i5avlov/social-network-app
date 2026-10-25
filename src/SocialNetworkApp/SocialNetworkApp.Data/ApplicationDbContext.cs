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

        public DbSet<Invitation> Invitations { get; set; } 

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

            // Invitation entity configuration 
            // Error with the default delete behavior 'cascade'
            // due to two foreign keys referencing the same table 
            builder.Entity<Invitation>(entity =>
            {
                entity.HasOne(inv => inv.SenderUser)
                    .WithMany(u => u.SentInvitations)
                    .HasForeignKey(inv => inv.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(inv => inv.ReceiverUser)
                    .WithMany(u => u.ReceivedInvitations)
                    .HasForeignKey(inv => inv.ReceiverUserId)
                    .OnDelete(DeleteBehavior.Restrict); 
            }); 
        }
    }
}
