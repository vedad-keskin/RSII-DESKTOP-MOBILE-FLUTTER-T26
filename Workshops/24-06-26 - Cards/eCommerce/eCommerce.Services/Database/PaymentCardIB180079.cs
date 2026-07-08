using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Services.Database
{
    public class PaymentCardIB180079
    {
        [Key]
        public int Id { get; set; }
        
        // Foreign key for User
        public int UserId { get; set; }
        
        // Navigation property for Product
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;


        [Required]
        [MaxLength(12)]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(3)]
        public string CVC { get; set; } = string.Empty;

        public DateTime ExiprationDate { get; set; } 

        public decimal InitialBalance { get; set; }
        
        
        
    }
} 