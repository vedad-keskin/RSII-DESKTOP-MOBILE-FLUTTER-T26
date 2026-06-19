using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class UserActivityUpdateValidator : AbstractValidator<UserActivityUpdateRequest>
    {
        public UserActivityUpdateValidator()
        {
        }
    }
}
