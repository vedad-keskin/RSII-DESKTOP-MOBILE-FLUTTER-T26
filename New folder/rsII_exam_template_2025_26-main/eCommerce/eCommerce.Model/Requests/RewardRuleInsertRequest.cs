using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class RewardRuleInsertRequest
    {
        [Required]
        public int ActivityId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int NumberOfPoints { get; set; }
    }
}
