using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Services.Database
{
    public class CouponIB180079
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercentage { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int MaxUses { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<CouponUsageIB180079> Usages { get; set; } = new List<CouponUsageIB180079>();
    }
}
