using System;
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Services.Database
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public RewardRule? RewardRule { get; set; }
    }
}
