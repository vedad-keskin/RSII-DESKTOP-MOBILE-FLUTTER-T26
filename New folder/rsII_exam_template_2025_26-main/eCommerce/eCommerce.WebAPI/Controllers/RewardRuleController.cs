using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.WebAPI.Controllers;

public class RewardRuleController : BaseCRUDController<RewardRuleResponse, RewardRuleSearch, RewardRuleInsertRequest, RewardRuleUpdateRequest, IRewardRuleService>
{
    public RewardRuleController(IRewardRuleService service) : base(service)
    {
    }
}
