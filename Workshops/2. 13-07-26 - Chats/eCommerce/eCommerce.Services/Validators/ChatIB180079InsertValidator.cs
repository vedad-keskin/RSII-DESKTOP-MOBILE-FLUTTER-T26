using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class ChatIB180079InsertValidator : AbstractValidator<ChatIB180079InsertRequest>
    {
        public ChatIB180079InsertValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Naziv razgovora je obavezan.")
                .Must(n => !string.IsNullOrWhiteSpace(n)).WithMessage("Naziv razgovora ne smije sadržavati samo prazne znakove.")
                .MaximumLength(100).WithMessage("Naziv razgovora ne može biti duži od 100 karaktera.");

            RuleFor(x => x.User1Id)
                .GreaterThan(0).WithMessage("User1Id is required and must be greater than 0.");

            RuleFor(x => x.User2Id)
                .GreaterThan(0).WithMessage("User2Id is required and must be greater than 0.");
        }
    }
}
