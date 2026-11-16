using System.Data;
using eCommerce.Model.Exceptions;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Services;

public class CouponUsageService
    : BaseCRUDService<CouponUsageIB180079, CouponUsageResponse, CouponUsageSearch, CouponUsageInsertRequest, CouponUsageUpdateRequest>,
      ICouponUsageService
{
    private readonly IAuthenticatedUserAccessor _userAccessor;

    public CouponUsageService(ECommerceDbContext dbContext, IMapper mapper,
        IValidator<CouponUsageInsertRequest> insertValidator,
        IValidator<CouponUsageUpdateRequest> updateValidator,
        IAuthenticatedUserAccessor userAccessor)
        : base(dbContext, mapper, insertValidator, updateValidator)
    {
        _userAccessor = userAccessor;
    }

    public override Task<PageResult<CouponUsageResponse>> GetAllAsync(CouponUsageSearch? search = null)
    {
        return base.GetAllAsync(search ?? new CouponUsageSearch());
    }

    protected override IEnumerable<CouponUsageIB180079> ApplyFilters(
        IEnumerable<CouponUsageIB180079> query, CouponUsageSearch? search)
    {
        var userId = _userAccessor.GetUserId();
        query = query.Where(x => x.UserId == userId);

        if (search?.CouponId.HasValue == true)
        {
            query = query.Where(x => x.CouponId == search.CouponId.Value);
        }

        return query;
    }

    public override async Task<CouponUsageResponse> InsertAsync(CouponUsageInsertRequest request)
    {
        await _insertValidator.ValidateAndThrowAsync(request);

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

        return new CouponUsageResponse
        {
            Id = usage.Id,
            CouponId = usage.CouponId,
            UserId = usage.UserId,
            UsedAt = usage.UsedAt,
            OriginalAmount = originalAmount,
            DiscountAmount = discountAmount,
            FinalAmount = originalAmount - discountAmount,
        };
    }

    public override async Task<CouponUsageResponse> GetByIdAsync(int id)
    {
        var userId = _userAccessor.GetUserId();
        var usage = await _dbContext.CouponUsages
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (usage == null)
        {
            throw new KeyNotFoundException("Coupon usage not found.");
        }

        return _mapper.Map<CouponUsageResponse>(usage);
    }

    public override Task<CouponUsageResponse> UpdateAsync(int id, CouponUsageUpdateRequest request)
    {
        throw new ClinetException("Coupon usage history cannot be edited.");
    }

    public override Task DeleteAsync(int id)
    {
        throw new ClinetException("Coupon usage history cannot be deleted.");
    }
}
