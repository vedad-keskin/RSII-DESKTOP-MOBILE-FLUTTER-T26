using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class ChatIB180079UpdateValidator : AbstractValidator<ChatIB180079UpdateRequest>
    {
        public ChatIB180079UpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required and must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Naziv razgovora je obavezan.")
                .Must(n => !string.IsNullOrWhiteSpace(n)).WithMessage("Naziv razgovora ne smije sadržavati samo prazne znakove.")
                .MaximumLength(100).WithMessage("Naziv razgovora ne može biti duži od 100 karaktera.");
        }
    }
}
