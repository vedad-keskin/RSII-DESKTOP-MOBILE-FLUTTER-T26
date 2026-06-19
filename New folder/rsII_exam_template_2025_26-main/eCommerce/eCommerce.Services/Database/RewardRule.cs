using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Services.Database
{
    public class RewardRule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ActivityId { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public Activity Activity { get; set; } = null!;

        public string RewardTitle { get; set; } = string.Empty;

        public int MaxDaysToComplete { get; set; }

        public int NumberOfPoints { get; set; }
    }
}
