using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class CouponUpdateValidator : AbstractValidator<CouponUpdateRequest>
    {
        public CouponUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(100)
                .Matches("^[A-Z]+$").WithMessage("Code must contain uppercase letters only, without spaces.");

            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.DiscountPercentage).GreaterThan(0).LessThanOrEqualTo(100).PrecisionScale(5, 2, true);
            RuleFor(x => x.ValidFrom).NotEmpty();
            RuleFor(x => x.ValidTo).NotEmpty().GreaterThan(x => x.ValidFrom);
            RuleFor(x => x.MaxUses).GreaterThan(0);
        }
    }
}
