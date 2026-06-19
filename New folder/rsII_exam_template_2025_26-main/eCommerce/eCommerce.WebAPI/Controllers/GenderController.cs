using eCommerce.Services;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using Microsoft.AspNetCore.Mvc;
using eCommerce.Model.Requests;

namespace eCommerce.WebAPI.Controllers;

public class GenderController : BaseCRUDController<GenderResponse, GenderSearch, GenderInsertRequest, GenderUpdateRequest, IGenderService>
{
    public GenderController(IGenderService genderService) : base(genderService)
    {
    }
}
