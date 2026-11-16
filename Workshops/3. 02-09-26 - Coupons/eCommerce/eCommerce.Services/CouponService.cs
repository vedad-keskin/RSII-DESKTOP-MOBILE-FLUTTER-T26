using eCommerce.Model.Exceptions;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.Services
{
    public class CouponService : BaseCRUDService<CouponIB180079, CouponResponse, CouponSearch, CouponInsertRequest, CouponUpdateRequest>, ICouponService
    {
        private readonly IAuthenticatedUserAccessor _userAccessor;

        public CouponService(ECommerceDbContext dbContext, MapsterMapper.IMapper mapper, IValidator<CouponInsertRequest> insertValidator, IValidator<CouponUpdateRequest> updateValidator, IAuthenticatedUserAccessor userAccessor) : base(dbContext, mapper, insertValidator, updateValidator)
        {
            _userAccessor = userAccessor;
        }

        public async Task<CouponApplyResponse> ApplyAsync(CouponApplyRequest request)
        {
            var userId = _userAccessor.GetUserId()
                ?? throw new ClinetException("Please log in to apply a coupon.");

            if (request.Items.Any(x => x == null || x.ProductId <= 0 || x.Quantity <= 0))
            {
                throw new ClinetException("Cart items must have a product and a positive quantity.");
            }

            await using var tx = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            var coupon = await _dbContext.Coupons.Include(x => x.CouponUsages)
                .FirstOrDefaultAsync(x => x.Code == request.Code);

            if (coupon == null)
            {
                throw new ClinetException("Coupon not found.");
            }

            var now = DateTime.UtcNow;
            if (!coupon.IsActive || now < coupon.ValidFrom || now > coupon.ValidTo)
            {
                throw new ClinetException("Coupon is inactive or outside its validity period.");
            }

            if (coupon.CouponUsages.Any(x => x.UserId == userId))
            {
                throw new ClinetException("You have already used this coupon.");
            }

            if (coupon.CouponUsages.Count >= coupon.MaxUses)
            {
                throw new ClinetException("Coupon has reached its maximum number of uses.");
            }

            decimal originalAmount = 0;
            foreach (var line in request.Items)
            {
                var product = await _dbContext.Products.FindAsync(line.ProductId);
                if (product == null)
                {
                    throw new ClinetException($"Product {line.ProductId} was not found.");
                }

                originalAmount += product.Price * line.Quantity;
            }

            var discountAmount = Math.Round(originalAmount * coupon.DiscountPercentage / 100, 2);
            var usage = new CouponUsageIB180079
            {
                CouponId = coupon.Id,
                UserId = userId,
                UsedAt = now,
                DiscountAmount = discountAmount,
            };

            _dbContext.CouponUsages.Add(usage);
            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();

            return new CouponApplyResponse
            {
                OriginalAmount = originalAmount,
                DiscountAmount = discountAmount,
                FinalAmount = originalAmount - discountAmount,
            };
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

            if (await _dbContext.Coupons.AnyAsync(u => u.Code == request.Code && u.Id != id))
            {
                throw new ClinetException($"Code '{request.Code}' is already in use.");
            }

            return await base.UpdateAsync(id, request);
        }



    }
}
