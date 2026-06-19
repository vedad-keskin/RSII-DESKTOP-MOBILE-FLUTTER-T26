using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Services;

public class UserActivityService
    : BaseCRUDService<UserActivity, UserActivityResponse, UserActivitySearch, UserActivityInsertRequest, UserActivityUpdateRequest>,
        IUserActivityService
{
    public UserActivityService(
        ECommerceDbContext dbContext,
        IMapper mapper,
        IValidator<UserActivityInsertRequest> insertValidator,
        IValidator<UserActivityUpdateRequest> updateValidator)
        : base(dbContext, mapper, insertValidator, updateValidator)
    {
    }

    protected override IEnumerable<UserActivity> ApplyFilters(IEnumerable<UserActivity> query, UserActivitySearch? search)
    {
        if (!string.IsNullOrWhiteSpace(search?.Status))
        {
            query = query.Where(ua => ua.Status.Contains(search.Status, StringComparison.OrdinalIgnoreCase));
        }

        return query;
    }

    protected override async Task<IQueryable<UserActivity>> IncludeRelatedEntitiesAsync(
        UserActivitySearch? search,
        IQueryable<UserActivity> query = null!)
    {
        return await Task.FromResult(
            query
                .Include(ua => ua.User)
                .Include(ua => ua.Activity)
                    .ThenInclude(a => a.RewardRule));
    }

    protected override UserActivity MapInsertRequestToEntity(UserActivityInsertRequest request)
    {
        var entity = base.MapInsertRequestToEntity(request);
        entity.Status = "Assigned";
        entity.DateAssigned = DateTime.Now;
        return entity;
    }

    public override async Task<UserActivityResponse> InsertAsync(UserActivityInsertRequest request)
    {
        var response = await base.InsertAsync(request);
        return await GetByIdAsync(response.Id);
    }














    public override async Task<UserActivityResponse> GetByIdAsync(int id)
    {
        var entity = await _dbContext.UserActivities
            //.AsNoTracking()
            .Include(ua => ua.User)
            .Include(ua => ua.Activity)
                .ThenInclude(a => a.RewardRule)
            .FirstOrDefaultAsync(ua => ua.Id == id);

        if (entity == null)
        {
            throw new KeyNotFoundException($"{nameof(UserActivity)} with id {id} not found.");
        }

        return _mapper.Map<UserActivityResponse>(entity);
    }

    public override async Task<PageResult<UserActivityResponse>> GetAllAsync(UserActivitySearch? search = null)
    {
        search ??= new UserActivitySearch();

        var query = _dbContext.UserActivities.AsQueryable();
        query = await IncludeRelatedEntitiesAsync(search, query);
        query = ApplyFilters(query, search).AsQueryable();

        int? totalCount = null;
        if (search.IncludeTotalCount ?? false)
        {
            totalCount = query.Count();
        }

        if (!string.IsNullOrWhiteSpace(search.SortBy))
        {
            query = query.OrderBy(search.SortBy);
        }

        if (search.Page.HasValue && search.PageSize.HasValue)
        {
            query = query.Skip((search.Page.Value - 1) * search.PageSize.Value);
        }

        if (search.PageSize.HasValue)
        {
            query = query.Take(search.PageSize.Value);
        }

        var entities = await query.AsNoTracking().ToListAsync();
        var items = entities.Select(entity => _mapper.Map<UserActivityResponse>(entity)).ToList();

        return new PageResult<UserActivityResponse>
        {
            Items = items,
            TotalCount = totalCount,
        };
    }












    public override async Task<UserActivityResponse> UpdateAsync(int id, UserActivityUpdateRequest request)
    {
        await base.UpdateAsync(id, request);
        return await GetByIdAsync(id);
    }

    public async Task<UserActivityResponse> ChangeStatusAsync(int id, string status)
    {
        var userActivity = await _dbContext.UserActivities
            .Include(ua => ua.User)
            .Include(ua => ua.Activity)
                .ThenInclude(a => a.RewardRule)
            .FirstOrDefaultAsync(ua => ua.Id == id);

        if (userActivity == null)
        {
            throw new InvalidOperationException("UserActivity not found.");
        }

        var rewardRule = await _dbContext.RewardRules
            .FirstOrDefaultAsync(r => r.ActivityId == userActivity.ActivityId);

        var currentStatus = userActivity.Status;

        if (currentStatus == "Assigned" && (status == "InProgress" || status == "Cancelled"))
        {
            if (status == "InProgress" && DateTime.Now > userActivity.Activity.DueDate)
            {
                throw new InvalidOperationException("Activity due date has passed.");
            }

            userActivity.Status = status;
        }
        else if (currentStatus == "InProgress" && (status == "Completed" || status == "Cancelled"))
        {
            userActivity.Status = status;

            if (status == "Completed")
            {
                userActivity.CompletedAt = DateTime.Now;
                userActivity.RewardedAt = DateTime.Now;
                userActivity.RewardTitle = rewardRule?.RewardTitle;
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid status transition.");
        }

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserActivityResponse>(userActivity);
    }
}
