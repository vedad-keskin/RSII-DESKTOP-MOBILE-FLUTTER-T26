using eCommerce.Model.Requests;
using FluentValidation;
using System;

namespace eCommerce.Services.Validators
{
    public class PaymentCardIB180079InsertValidator : AbstractValidator<PaymentCardIB180079InsertRequest>
    {
        public PaymentCardIB180079InsertValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId is required and must be greater than 0.");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .Length(12).WithMessage("Card number must be exactly 12 digits.")
                .Matches(@"^\d{12}$").WithMessage("Card number must contain only digits.");

            RuleFor(x => x.Cvc)
                .NotEmpty().WithMessage("CVC is required.")
                .Length(3).WithMessage("CVC must be exactly 3 digits.")
                .Matches(@"^\d{3}$").WithMessage("CVC must contain only digits.");

            RuleFor(x => x.ExpirationDate)
                .Must(d => d.Date >= DateTime.Today)
                .WithMessage("Expiration date must not be in the past.");

            RuleFor(x => x.InitialBalance)
                .GreaterThanOrEqualTo(0).WithMessage("Initial balance must be greater than or equal to 0.");
        }
    }
}
