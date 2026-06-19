using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Services.Database
{
    public class UserActivity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        public int ActivityId { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public Activity Activity { get; set; } = null!;

        [Required]
        public DateTime DateAssigned { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = string.Empty;

        public string? Note { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string? RewardTitle { get; set; }

        public DateTime? RewardedAt { get; set; }
    }
}
