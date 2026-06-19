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
    public class ActivityService
        : BaseCRUDService<Activity, ActivityResponse, ActivitySearch, ActivityInsertRequest, ActivityUpdateRequest>,
            IActivityService
    {
        public ActivityService(
            ECommerceDbContext dbContext,
            IMapper mapper,
            IValidator<ActivityInsertRequest> insertValidator,
            IValidator<ActivityUpdateRequest> updateValidator)
            : base(dbContext, mapper, insertValidator, updateValidator)
        {
        }

        protected override IEnumerable<Activity> ApplyFilters(IEnumerable<Activity> query, ActivitySearch? search)
        {
            if (!string.IsNullOrWhiteSpace(search?.Name))
            {
                query = query.Where(a => a.Name.Contains(search.Name, StringComparison.OrdinalIgnoreCase));
            }

            return query;
        }

        public override async Task<ActivityResponse> InsertAsync(ActivityInsertRequest request)
        {
            var validationResult = await _insertValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => _mapper.Map<ValidationFailure>(e));
                throw new FluentValidation.ValidationException(errors);
            }

            if (await _dbContext.Activities.AnyAsync(a => a.Name == request.Name))
            {
                throw new InvalidOperationException("An activity with that name already exists.");
            }

            return await base.InsertAsync(request);
        }
    }
}
