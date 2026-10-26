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

        public DbSet<Message> Messages { get; set; }

        public DbSet<Friendship> Friendships { get; set; } 

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

            // Message entity configuration 
            // Error with the default delete behavior 'cascade'
            // due to two foreign keys referencing the same table 
            builder.Entity<Message>(entity =>
            {
                entity.HasOne(m => m.SenderUser)
                    .WithMany(u => u.SentMessages)
                    .HasForeignKey(m => m.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.ReceiverUser)
                    .WithMany(u => u.ReceivedMessages)
                    .HasForeignKey(m => m.ReceiverUserId)
                    .OnDelete(DeleteBehavior.Restrict); 
            });

            // Friendship entity configuration 
            builder.Entity<Friendship>(entity => {
                entity.HasKey(f => new { f.SourceUserId, f.TargetUserId });

                entity.HasOne(f => f.SourceUser)
                    .WithMany(u => u.SourceUserFriendships)
                    .HasForeignKey(f => f.SourceUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.TargetUser)
                    .WithMany(u => u.TargetUserFriendships)
                    .HasForeignKey(f => f.TargetUserId)
                    .OnDelete(DeleteBehavior.Restrict); 
            });
        }
    }
}
