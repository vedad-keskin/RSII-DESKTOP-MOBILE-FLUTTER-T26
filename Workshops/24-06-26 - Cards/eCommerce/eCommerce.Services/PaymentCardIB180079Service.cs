using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using MapsterMapper;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.Services
{
    public class PaymentCardIB180079Service : BaseCRUDService<PaymentCardIB180079, PaymentCardIB180079Response, PaymentCardIB180079Search, PaymentCardIB180079InsertRequest, PaymentCardIB180079UpdateRequest>, IPaymentCardIB180079Service
    {
        public PaymentCardIB180079Service(ECommerceDbContext dbContext, IMapper mapper, IValidator<PaymentCardIB180079InsertRequest> insertValidator, IValidator<PaymentCardIB180079UpdateRequest> updateValidator)
            : base(dbContext, mapper, insertValidator, updateValidator)
        {
        }

        protected override IEnumerable<PaymentCardIB180079> ApplyFilters(IEnumerable<PaymentCardIB180079> query, PaymentCardIB180079Search? search)
        {
            if (search != null && search.UserId.HasValue)
            {
                query = query.Where(c => c.UserId == search.UserId.Value);
            }

            return query;
        }
    }
}
