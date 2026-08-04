using eCommerce.Model.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services.Validators
{
    public class ChatInsertValidator : AbstractValidator<ChatInsertRequest>
    {
        public ChatInsertValidator()
        {
            RuleFor(x => x.User1Id)
                .GreaterThan(0).WithMessage("User1Id is required and must be greater than 0.");

            RuleFor(x => x.User2Id)
                 .GreaterThan(0).WithMessage("User2Id is required and must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        }
    }
}
