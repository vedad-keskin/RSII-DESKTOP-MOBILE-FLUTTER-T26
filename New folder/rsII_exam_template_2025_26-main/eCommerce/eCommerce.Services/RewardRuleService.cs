using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Services
{
    public class RewardRuleService
        : BaseCRUDService<RewardRule, RewardRuleResponse, RewardRuleSearch, RewardRuleInsertRequest, RewardRuleUpdateRequest>,
            IRewardRuleService
    {
        public RewardRuleService(
            ECommerceDbContext dbContext,
            IMapper mapper,
            IValidator<RewardRuleInsertRequest> insertValidator,
            IValidator<RewardRuleUpdateRequest> updateValidator)
            : base(dbContext, mapper, insertValidator, updateValidator)
        {
        }

        protected override IEnumerable<RewardRule> ApplyFilters(IEnumerable<RewardRule> query, RewardRuleSearch? search)
        {
            if (!string.IsNullOrWhiteSpace(search?.RewardTitle))
            {
                query = query.Where(r => r.RewardTitle.Contains(search.RewardTitle, StringComparison.OrdinalIgnoreCase));
            }

            return query;
        }

        public override async Task<RewardRuleResponse> InsertAsync(RewardRuleInsertRequest request)
        {
            var validationResult = await _insertValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => _mapper.Map<ValidationFailure>(e));
                throw new FluentValidation.ValidationException(errors);
            }

            if (await _dbContext.RewardRules.AnyAsync(r => r.ActivityId == request.ActivityId))
            {
                throw new InvalidOperationException("A reward for that activity already exists.");
            }

            var activity = await _dbContext.Activities.FirstOrDefaultAsync(a => a.Id == request.ActivityId);
            if (activity == null)
            {
                throw new InvalidOperationException("Activity not found.");
            }

            var entity = new RewardRule
            {
                ActivityId = request.ActivityId,
                NumberOfPoints = request.NumberOfPoints,
                RewardTitle = $"{activity.Name} - {request.NumberOfPoints} points",
                MaxDaysToComplete = (int)(activity.DueDate.Date - DateTime.Now.Date).TotalDays,
            };

            _dbContext.RewardRules.Add(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RewardRuleResponse>(entity);
        }
    }
}
