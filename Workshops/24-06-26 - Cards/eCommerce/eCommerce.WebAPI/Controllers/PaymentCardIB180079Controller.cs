using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services;

namespace eCommerce.WebAPI.Controllers;

public class PaymentCardIB180079Controller : BaseCRUDController<PaymentCardIB180079Response, PaymentCardIB180079Search, PaymentCardIB180079InsertRequest, PaymentCardIB180079UpdateRequest, IPaymentCardIB180079Service>
{
    public PaymentCardIB180079Controller(IPaymentCardIB180079Service paymentCardService) : base(paymentCardService)
    {
    }
}
