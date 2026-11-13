using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Services.Database
{
    public class CouponUsageIB180079
    {
        [Key]
        public int Id { get; set; }

        public int CouponId { get; set; }

        [ForeignKey("CouponId")]
        public CouponIB180079 Coupon { get; set; } = null!;

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public DateTime UsedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }
    }
}
