using System;

namespace eCommerce.Model.Responses
{
    public class PaymentCardIB180079Response
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string Cvc { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public decimal InitialBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
