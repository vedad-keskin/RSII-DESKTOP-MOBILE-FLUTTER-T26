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

        [Required]
        [Column(TypeName = "decimal(5,2)")] // 100.00
        public decimal DiscountPercentage { get; set; }


        public DateTime ValidFrom { get; set; } 
        public DateTime ValidTo { get; set; } 
        public int MaxUses { get; set; }

        public bool IsActive { get; set; } = true;



        // Navigation property for products
        public ICollection<CouponUsageIB180079> CouponUsages { get; set; } = new List<CouponUsageIB180079>();
    }
} 