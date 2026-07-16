using BookFace.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookFace.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Global Query Filters (Soft Delete)
            builder.Entity<Post>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Comment>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Message>().HasQueryFilter(x => !x.IsDeleted);

            // Indexes for Performance
            builder.Entity<Post>().HasIndex(x => x.CreatorId);
            builder.Entity<Comment>().HasIndex(x => x.PostId);
            builder.Entity<Message>().HasIndex(x => x.ChatId);
            builder.Entity<Like>().HasIndex(x => x.PostId);

            // Relationships
            builder
                .Entity<ApplicationUser>()
                .HasMany(x => x.Posts)
                .WithOne(x => x.Creator)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Friendship>()
                .HasOne(x => x.Requester)
                .WithMany(x => x.Friendships)
                .HasForeignKey(x => x.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Friendship>()
                .HasOne(x => x.Addressee)
                .WithMany()
                .HasForeignKey(x => x.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ChatUser>()
                .HasKey(x => new { x.ChatId, x.UserId });

            builder
                .Entity<ChatUser>()
                .HasOne(x => x.Chat)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<ChatUser>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }

}
