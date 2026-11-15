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







    }
}
