using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators;

public class CouponUsageUpdateValidator : AbstractValidator<CouponUsageUpdateRequest>
{
    public CouponUsageUpdateValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
