using eCommerce.Services;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using Microsoft.AspNetCore.Mvc;
using eCommerce.Model.Requests;

namespace eCommerce.WebAPI.Controllers;

public class PaymentCardIB180079Controller : BaseCRUDController<PaymentCardIB180079Response, PaymentCardIB180079Search, PaymentCardIB180079InsertRequest, PaymentCardIB180079UpdateRequest, IPaymentCardIB180079Service>
{
    public PaymentCardIB180079Controller(IPaymentCardIB180079Service paymentCardIB180079Service) : base(paymentCardIB180079Service)
    {
    }
}
