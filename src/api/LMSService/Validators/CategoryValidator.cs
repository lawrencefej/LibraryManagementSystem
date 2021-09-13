using FluentValidation;
using LMSEntities.DataTransferObjects;

namespace LMSService.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Category {PropertyName} is required")
                .MaximumLength(100)
                .WithMessage("Category {PropertyName} must not be more than 100 characters");

            RuleFor(r => r.Description)
                .MaximumLength(250)
                .WithMessage("Category {PropertyName} must not be more than 250 characters");

        }
    }
}
