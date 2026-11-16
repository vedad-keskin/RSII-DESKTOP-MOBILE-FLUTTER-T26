using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators;

public class CouponUsageInsertValidator : AbstractValidator<CouponUsageInsertRequest>
{
    public CouponUsageInsertValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Items).NotEmpty();
    }
}
