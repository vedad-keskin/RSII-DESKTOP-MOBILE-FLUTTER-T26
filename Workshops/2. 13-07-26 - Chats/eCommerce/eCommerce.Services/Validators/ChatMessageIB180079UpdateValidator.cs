using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class ChatMessageIB180079UpdateValidator : AbstractValidator<ChatMessageIB180079UpdateRequest>
    {
        public ChatMessageIB180079UpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required and must be greater than 0.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Sadržaj poruke je obavezan.")
                .Must(c => !string.IsNullOrWhiteSpace(c)).WithMessage("Sadržaj poruke ne smije sadržavati samo prazne znakove.");
        }
    }
}
