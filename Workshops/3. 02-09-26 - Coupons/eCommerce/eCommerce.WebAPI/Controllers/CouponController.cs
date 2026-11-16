using eCommerce.Services;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using Microsoft.AspNetCore.Mvc;
using eCommerce.Model.Requests;
using Microsoft.AspNetCore.Authorization;

namespace eCommerce.WebAPI.Controllers;

public class CouponController : BaseCRUDController<CouponResponse, CouponSearch, CouponInsertRequest, CouponUpdateRequest, ICouponService>
{
    public CouponController(ICouponService couponService) : base(couponService)
    {
    }

    [Authorize]
    [HttpPost("Apply")]
    public async Task<ActionResult<CouponApplyResponse>> Apply([FromBody] CouponApplyRequest request)
    {
        var result = await _service.ApplyAsync(request);
        return Ok(result);
    }
}
