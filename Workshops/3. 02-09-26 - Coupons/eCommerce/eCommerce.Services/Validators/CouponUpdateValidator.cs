using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class CouponUpdateValidator : AbstractValidator<CouponUpdateRequest>
    {
        public CouponUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .MinimumLength(2).WithMessage("Name must have at least 2 characters")
                .Matches("^[A-Z]+$").WithMessage("Code must contain uppercase letters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.DiscountPercentage)
                .GreaterThan(0).WithMessage("Discount Percentage must be higher than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("Discount Percentage must be lower or equal to 0.");

            RuleFor(x => x.ValidTo)
                .GreaterThan(x => x.ValidFrom).WithMessage("Valid To must be higher than Valid From.");

            RuleFor(x => x.MaxUses)
                .GreaterThan(0).WithMessage("MaxU ses must be higher than 0.");
        }
    }
}
