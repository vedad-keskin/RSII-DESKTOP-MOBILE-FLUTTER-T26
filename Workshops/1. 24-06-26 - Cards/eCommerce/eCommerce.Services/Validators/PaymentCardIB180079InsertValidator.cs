using eCommerce.Model.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services.Validators
{
    public class PaymentCardIB180079InsertValidator : AbstractValidator<PaymentCardIB180079InsertRequest>
    {
        public PaymentCardIB180079InsertValidator()
        {

            // FK
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .GreaterThan(0).WithMessage("UserId is required and must be greater than 0.");


            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card Number is required.")
                .Length(12).WithMessage("Card Number must be exactly 12 digits.")
                //.Matches("[0-9]{12}").WithMessage("Card Number must contain only digits.")
                .Matches(@"\d{12}").WithMessage("Card Number must contain only digits.");

            RuleFor(x => x.CVC)
                .NotEmpty().WithMessage("CVC is required.")
                .Length(3).WithMessage("CVC must be exactly 3 digits.")
                //.Matches("[0-9]{12}").WithMessage("CVC must contain only digits.")
                .Matches(@"\d{3}").WithMessage("CVC must contain only digits.");

            RuleFor(x => x.ExiprationDate)
                .Must(x => x.Date >= DateTime.Today).WithMessage("Exipration Date must be in the future.");


            RuleFor(x => x.InitialBalance)
               .NotEmpty().WithMessage("Initial Balance is required.")
               .GreaterThanOrEqualTo(0).WithMessage("Initial Balance must be greater than or equal to 0.");

        }
    }
}
