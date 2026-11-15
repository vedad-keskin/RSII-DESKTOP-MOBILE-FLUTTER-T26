using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;

namespace eCommerce.Services
{
    public interface ICouponService : IBaseCRUDService<CouponResponse, CouponSearch, CouponInsertRequest, CouponUpdateRequest>
    {

    }
}
