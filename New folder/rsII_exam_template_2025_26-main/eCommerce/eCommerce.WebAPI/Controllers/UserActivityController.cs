using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.WebAPI.Controllers;

public class UserActivityController : BaseCRUDController<UserActivityResponse, UserActivitySearch, UserActivityInsertRequest, UserActivityUpdateRequest, IUserActivityService>
{
    private readonly IUserActivityService _userActivityService;

    public UserActivityController(IUserActivityService service) : base(service)
    {
        _userActivityService = service;
    }

    [HttpPost("change-status")]
    public async Task<ActionResult<UserActivityResponse>> ChangeStatusAsync(int id, string status)
    {
        var result = await _userActivityService.ChangeStatusAsync(id, status);
        return Ok(result);
    }
}
