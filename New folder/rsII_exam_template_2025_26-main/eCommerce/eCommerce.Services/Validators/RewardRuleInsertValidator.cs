using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class RewardRuleInsertValidator : AbstractValidator<RewardRuleInsertRequest>
    {
        public RewardRuleInsertValidator()
        {
            RuleFor(x => x.ActivityId)
                .GreaterThan(0);

            RuleFor(x => x.NumberOfPoints)
                .GreaterThan(0);
        }
    }
}
