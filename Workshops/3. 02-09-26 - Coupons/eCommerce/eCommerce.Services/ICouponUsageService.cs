using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;

namespace eCommerce.Services;

public interface ICouponUsageService
    : IBaseCRUDService<CouponUsageResponse, CouponUsageSearch, CouponUsageInsertRequest, CouponUsageUpdateRequest>
{
}
