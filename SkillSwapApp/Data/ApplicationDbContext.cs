using System;
using Microsoft.EntityFrameworkCore;
using SkillSwapApp.Models;

namespace SkillSwapApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany()
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ReviewedUser)
                .WithMany()
                .HasForeignKey(r => r.ReviewedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed admin user
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@skillswap.com",
                PasswordHash = "admin123", // В реалност трябва да е хеширана
                Role = "Admin",
                Bio = "System Administrator",
                Rating = 5.0,
                CreatedAt = DateTime.Now

            },
        new User
        {
            Id = 4,
            Username = "mike_wilson",
            Email = "mike@example.com",
            PasswordHash = "password123",
            Role = "User",
            Bio = "Marketing specialist and content creator",
            Rating = 4.2,
            CreatedAt = new DateTime(2024, 4, 10, 9, 15, 0)
        }

            );
        }
    }
}