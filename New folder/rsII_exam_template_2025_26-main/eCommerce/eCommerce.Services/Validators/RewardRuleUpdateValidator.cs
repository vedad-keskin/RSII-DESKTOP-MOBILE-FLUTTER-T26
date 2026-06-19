using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class RewardRuleUpdateValidator : AbstractValidator<RewardRuleUpdateRequest>
    {
        public RewardRuleUpdateValidator()
        {
            RuleFor(x => x.NumberOfPoints)
                .GreaterThan(0);
        }
    }
}
