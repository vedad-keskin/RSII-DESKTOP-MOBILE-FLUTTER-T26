using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.WebAPI.Controllers;

public class ActivityController : BaseCRUDController<ActivityResponse, ActivitySearch, ActivityInsertRequest, ActivityUpdateRequest, IActivityService>
{
    public ActivityController(IActivityService service) : base(service)
    {
    }
}
