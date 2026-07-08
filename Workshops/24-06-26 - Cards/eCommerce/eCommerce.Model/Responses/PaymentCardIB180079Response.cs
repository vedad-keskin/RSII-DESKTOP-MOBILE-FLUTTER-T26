using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Model.Responses
{
    public class PaymentCardIB180079Response
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string CardNumber { get; set; } = string.Empty;
        public string CVC { get; set; } = string.Empty;
        public DateTime ExiprationDate { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
