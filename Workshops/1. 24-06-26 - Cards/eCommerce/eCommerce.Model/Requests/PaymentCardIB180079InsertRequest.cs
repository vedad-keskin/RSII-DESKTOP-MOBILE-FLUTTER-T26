using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Model.Requests
{
    public class PaymentCardIB180079InsertRequest
    {
        public int UserId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string CVC { get; set; } = string.Empty;
        public DateTime ExiprationDate { get; set; } = DateTime.Now.AddDays(3);
        public decimal InitialBalance { get; set; }
    }
}
