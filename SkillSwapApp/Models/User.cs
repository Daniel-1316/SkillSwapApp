using System;
using System.ComponentModel.DataAnnotations;

namespace SkillSwapApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        public double Rating { get; set; } = 0.0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User"; // Guest, User, Admin
    }
}