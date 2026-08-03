using eCommerce.Model.Requests;
using FluentValidation;

namespace eCommerce.Services.Validators
{
    public class ChatMessageIB180079InsertValidator : AbstractValidator<ChatMessageIB180079InsertRequest>
    {
        public ChatMessageIB180079InsertValidator()
        {
            RuleFor(x => x.ChatId)
                .GreaterThan(0).WithMessage("ChatId is required and must be greater than 0.");

            RuleFor(x => x.SenderUserId)
                .GreaterThan(0).WithMessage("SenderUserId is required and must be greater than 0.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Sadržaj poruke je obavezan.")
                .Must(c => !string.IsNullOrWhiteSpace(c)).WithMessage("Sadržaj poruke ne smije sadržavati samo prazne znakove.");
        }
    }
}
