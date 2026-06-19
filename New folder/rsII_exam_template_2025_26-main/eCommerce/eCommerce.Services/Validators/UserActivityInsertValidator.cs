using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class UserActivityInsertValidator : AbstractValidator<UserActivityInsertRequest>
    {
        public UserActivityInsertValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.ActivityId)
                .GreaterThan(0);
        }
    }
}
