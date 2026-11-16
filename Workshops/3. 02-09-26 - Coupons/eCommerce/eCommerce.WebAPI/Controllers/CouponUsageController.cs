using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services;
using Microsoft.AspNetCore.Authorization;

namespace eCommerce.WebAPI.Controllers;

[Authorize]
public class CouponUsageController
    : BaseCRUDController<CouponUsageResponse, CouponUsageSearch, CouponUsageInsertRequest, CouponUsageUpdateRequest, ICouponUsageService>
{
    public CouponUsageController(ICouponUsageService couponUsageService) : base(couponUsageService)
    {
    }
}
