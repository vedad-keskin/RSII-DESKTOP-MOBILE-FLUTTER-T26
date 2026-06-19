using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class RewardRuleUpdateRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int NumberOfPoints { get; set; }
    }
}
