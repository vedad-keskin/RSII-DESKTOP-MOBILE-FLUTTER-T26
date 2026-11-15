using eCommerce.Model.Exceptions;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.Services
{
    public class CouponService : BaseCRUDService<CouponIB180079, CouponResponse, CouponSearch, CouponInsertRequest, CouponUpdateRequest>, ICouponService
    {
        public CouponService(ECommerceDbContext dbContext, MapsterMapper.IMapper mapper, IValidator<CouponInsertRequest> insertValidator, IValidator<CouponUpdateRequest> updateValidator) : base(dbContext, mapper, insertValidator, updateValidator)
        {
        }

        protected override IEnumerable<CouponIB180079> ApplyFilters(IEnumerable<CouponIB180079> query, CouponSearch? search)
        {
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Code))
                {
                    query = query.Where(pt => pt.Code.Contains(search.Code, StringComparison.OrdinalIgnoreCase));
                }

            }

            return query;
        }

        protected override async Task<IQueryable<CouponIB180079>> IncludeRelatedEntitiesAsync(
    CouponSearch? search,
    IQueryable<CouponIB180079> query = null!)
        {
            return await Task.FromResult(query.Include(r => r.CouponUsages));
        }


        public override async Task DeleteAsync(int id)
        {

            if (await _dbContext.CouponUsages.AnyAsync(x => x.CouponId == id))
            {

                throw new ClinetException("A Coupon that has already been used cannot be deleted.");

            }

            await base.DeleteAsync(id);
        }


        public override async Task<CouponResponse> InsertAsync(CouponInsertRequest request)
        {

            if (await _dbContext.Coupons.AnyAsync(u => u.Code == request.Code))
            {
                throw new ClinetException($"Code '{request.Code}' is already in use.");
            }

            return await base.InsertAsync(request);
        }

        public override async Task<CouponResponse> UpdateAsync(int id, CouponUpdateRequest request)
        {

            if (await _dbContext.Coupons.AnyAsync(u => u.Code == request.Code))
            {
                throw new ClinetException($"Code '{request.Code}' is already in use.");
            }

            return await base.UpdateAsync(id, request);
        }



    }
}
