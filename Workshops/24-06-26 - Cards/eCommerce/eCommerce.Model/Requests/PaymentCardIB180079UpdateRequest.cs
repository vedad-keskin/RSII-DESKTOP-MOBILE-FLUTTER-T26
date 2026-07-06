using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Model.Requests
{
    public class PaymentCardIB180079UpdateRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string CVC { get; set; } = string.Empty;
        public DateTime ExiprationDate { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
