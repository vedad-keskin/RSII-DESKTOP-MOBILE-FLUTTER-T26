using eCommerce.Model.Exceptions;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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




        public async Task<CouponApplyResponse> CouponApplyAsync(CouponApplyRequest request)
        {
            // user -> order service

            var userId = _userAccessor.GetUserId()
             ?? throw new InvalidOperationException("User id claim is missing.");


            if (request.Items == null || request.Items.Count == 0)
            {
                throw new ClinetException("Cart is empty.");
            }


            // request -> Items -> ProductId + quant , Code

            var coupon = await _dbContext.Coupons
                .Include(o => o.CouponUsages)
                .FirstOrDefaultAsync(o => o.Code == request.Code);


            if (coupon == null)
            {
                throw new ClinetException("Coupon not found.");
            }

            var duplicate = await _dbContext.CouponUsages.AnyAsync(r =>
                r.UserId == userId && r.CouponId == coupon.Id);

            if (duplicate)
            {
                throw new ClinetException("User has alredy used this coupon.");
            }

             bool IsCouponValid = coupon.IsActive && coupon.ValidFrom <= DateTime.UtcNow && coupon.ValidTo >= DateTime.UtcNow && coupon.CouponUsages.Count() < coupon.MaxUses;

            if (!IsCouponValid)
            {
                throw new ClinetException("Coupon is not valid.");

            }

            var merged = request.Items
                .Where(i => i.Quantity > 0)
                .GroupBy(i => i.ProductId)
                .Select(g => new { ProductId = g.Key, Quantity = g.Sum(x => x.Quantity) })
                .ToList();


            decimal total = 0;


            foreach (var line in merged)
            {
                var product = await _dbContext.Products.FindAsync(line.ProductId);


                if (product == null)
                {
                    throw new ClinetException($"Product {line.ProductId} was not found.");
                }

                if (!product.IsActive)
                {
                    throw new ClinetException($"Product '{product.Name}' is not available.");
                }

             
                var unitPrice = product.Price;

                total += unitPrice * line.Quantity;

            }

            var discountAmount = Math.Round( total * coupon.DiscountPercentage / 100 , 2);


            // dodati coupon usage
            // coupon Id , user Id , UsedAt , discounted amount

            var newCouponUsage = new CouponUsageIB180079()
            {
                UserId = userId,
                CouponId = coupon.Id,
                UsedAt = DateTime.UtcNow,
                DiscountAmount = discountAmount
            };

            _dbContext.CouponUsages.Add(newCouponUsage);
            await _dbContext.SaveChangesAsync();


            // response -> 3 amount


            return new CouponApplyResponse()
            {
                OriginalAmount = total,
                FinalAmount = total - discountAmount,
                DiscountAmount = discountAmount,
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
