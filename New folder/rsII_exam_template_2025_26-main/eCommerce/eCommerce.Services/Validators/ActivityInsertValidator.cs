using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class ActivityInsertValidator : AbstractValidator<ActivityInsertRequest>
    {
        public ActivityInsertValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .MaximumLength(200);

            RuleFor(x => x.DueDate)
                .NotEmpty();
        }
    }
}
