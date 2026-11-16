using eCommerce.Services;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using Microsoft.AspNetCore.Mvc;
using eCommerce.Model.Requests;

namespace eCommerce.WebAPI.Controllers;

public class CouponController : BaseCRUDController<CouponResponse, CouponSearch, CouponInsertRequest, CouponUpdateRequest, ICouponService>
{
    public CouponController(ICouponService couponService) : base(couponService)
    {
    }


    [HttpPost("Apply")]
    public async Task<ActionResult<CouponApplyResponse>> Apply([FromBody] CouponApplyRequest request)
    {
        var result = await _service.CouponApplyAsync(request);
        return Ok(result);
    }


}
