using FluentValidation;
using LMSEntities.DataTransferObjects;

namespace LMSService.Validators
{
    public class AuthorValidator : AbstractValidator<AuthorDto>
    {
        public AuthorValidator()
        {
            RuleFor(t => t.FullName)
                .NotEmpty()
                .Length(1, 50)
                .WithMessage("Author {PropertyName} must not be more than 50 characters");

            RuleFor(r => r.Description).MaximumLength(250)
                .WithMessage("Author {PropertyName} must not be more than 250 characters");
        }
    }
}
