using BookFace.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace BookFace.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {

        }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<ApplicationUser>()
                .HasMany(x => x.Likes)
                .WithMany(x => x.Likes);

            builder
                .Entity<ApplicationUser>()
                .HasMany(x => x.Posts)
                .WithOne(x => x.Creator);

            builder
                .Entity<Friendship>()
                .HasKey(x => new { x.SenderId, x.ReceiverId });

            builder
                .Entity<Friend>()
                .HasMany(x => x.Friendships)
                .WithOne(x => x.Receiver)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Friend>()
                .HasOne(x => x.User)
                .WithOne(x => x.Friend)
                .HasForeignKey<ApplicationUser>(x => x.FriendId);

            builder
                .Entity<ApplicationUser>()
                .HasMany(x => x.Friendships)
                .WithOne(x => x.Sender)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
