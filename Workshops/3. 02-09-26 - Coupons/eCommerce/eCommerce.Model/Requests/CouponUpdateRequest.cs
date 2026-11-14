using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class CouponUpdateRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public decimal DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int MaxUses { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
