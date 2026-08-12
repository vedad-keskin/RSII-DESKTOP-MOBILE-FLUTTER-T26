using eCommerce.Model.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services.Validators
{
    public class ChatMessageInsertValidator : AbstractValidator<ChatMessageInsertRequest>
    {
        public ChatMessageInsertValidator()
        {
            RuleFor(x => x.ChatId)
                .GreaterThan(0).WithMessage("ChatId is required and must be greater than 0.");

            RuleFor(x => x.SenderId)
                 .GreaterThan(0).WithMessage("SenderId is required and must be greater than 0.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Content cannot be empty text");

        }
    }
}
