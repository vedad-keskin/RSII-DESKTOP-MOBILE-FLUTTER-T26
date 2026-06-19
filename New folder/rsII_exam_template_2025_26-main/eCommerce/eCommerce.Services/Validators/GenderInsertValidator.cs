using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class GenderInsertValidator : AbstractValidator<GenderInsertRequest>
    {
        public GenderInsertValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
                .MinimumLength(2).WithMessage("Name must have at least 2 characters.");

        }
    }
}
