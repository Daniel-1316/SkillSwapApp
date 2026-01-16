using System;
using System.ComponentModel.DataAnnotations;
using SkillSwapApp.Models;

namespace SkillSwapApp.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int ReviewerId { get; set; } // Кой оценява

        [Required]
        public int ReviewedUserId { get; set; } // Когото оценяват

        [Required]
        public int SkillId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User Reviewer { get; set; }
        public User ReviewedUser { get; set; }
        public Skill Skill { get; set; }
    }
}