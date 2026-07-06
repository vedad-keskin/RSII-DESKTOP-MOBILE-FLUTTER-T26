using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Services.Database
{
    public class PaymentCardIB180079
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(12)]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(3)]
        public string Cvc { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; }

        public decimal InitialBalance { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
