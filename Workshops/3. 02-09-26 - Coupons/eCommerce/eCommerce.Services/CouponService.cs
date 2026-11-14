using System.Data;
using eCommerce.Model.Exceptions;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Services
{
    public class CouponService : BaseCRUDService<CouponIB180079, CouponResponse, CouponSearch, CouponInsertRequest, CouponUpdateRequest>, ICouponService
    {
        public CouponService(ECommerceDbContext dbContext, MapsterMapper.IMapper mapper, IValidator<CouponInsertRequest> insertValidator, IValidator<CouponUpdateRequest> updateValidator)
            : base(dbContext, mapper, insertValidator, updateValidator)
        {
        }

        protected override IEnumerable<CouponIB180079> ApplyFilters(IEnumerable<CouponIB180079> query, CouponSearch? search)
        {
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Code))
                {
                    query = query.Where(c => c.Code.Contains(search.Code, StringComparison.OrdinalIgnoreCase));
                }

                if (search.IsActive.HasValue)
                {
                    query = query.Where(c => c.IsActive == search.IsActive.Value);
                }
            }

            return query;
        }

        protected override async Task<IQueryable<CouponIB180079>> IncludeRelatedEntitiesAsync(
            CouponSearch? search, IQueryable<CouponIB180079> query = null!)
        {
            return await Task.FromResult(query.Include(c => c.CouponUsages));
        }

        //public override async Task<CouponResponse> GetByIdAsync(int id)
        //{
        //    var coupon = await _dbContext.Coupons
        //        .AsNoTracking()
        //        .Include(c => c.CouponUsages)
        //        .FirstOrDefaultAsync(c => c.Id == id);

        //    if (coupon == null)
        //    {
        //        throw new KeyNotFoundException($"Coupon with id {id} not found.");
        //    }

        //    return _mapper.Map<CouponResponse>(coupon);
        //}

        //public override async Task<CouponResponse> InsertAsync(CouponInsertRequest request)
        //{
        //    await using var tx = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        //    if (await _dbContext.Coupons.AnyAsync(c => c.Code == request.Code))
        //    {
        //        throw new ClinetException("Coupon code already exists.");
        //    }

        //    var response = await base.InsertAsync(request);
        //    await tx.CommitAsync();
        //    return response;
        //}

        //public override async Task<CouponResponse> UpdateAsync(int id, CouponUpdateRequest request)
        //{
        //    if (request.Id != id)
        //    {
        //        throw new ClinetException("Coupon id does not match the URL.");
        //    }

        //    await using var tx = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        //    if (!await _dbContext.Coupons.AnyAsync(c => c.Id == id))
        //    {
        //        throw new ClinetException("Coupon not found.");
        //    }

        //    if (await _dbContext.Coupons.AnyAsync(c => c.Code == request.Code && c.Id != id))
        //    {
        //        throw new ClinetException("Coupon code already exists.");
        //    }

        //    await base.UpdateAsync(id, request);
        //    var response = await GetByIdAsync(id);
        //    await tx.CommitAsync();
        //    return response;
        //}

        //public override async Task DeleteAsync(int id)
        //{
        //    await using var tx = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        //    if (!await _dbContext.Coupons.AnyAsync(c => c.Id == id))
        //    {
        //        throw new ClinetException("Coupon not found.");
        //    }

        //    if (await _dbContext.CouponUsages.AnyAsync(u => u.CouponId == id))
        //    {
        //        throw new ClinetException("A coupon that has already been used cannot be deleted.");
        //    }

        //    await base.DeleteAsync(id);
        //    await tx.CommitAsync();
        //}
    }
}
