using FluentValidation;
using LMSEntities.DataTransferObjects;
using LMSService.Validators.services;

namespace LMSService.Validators
{
    public class LibraryCardValidator : AbstractValidator<LibraryCardForCreationDto>
    {
        private readonly IValidatorService _validatorService;
        public LibraryCardValidator(IValidatorService validatorService)
        {
            _validatorService = validatorService;

            RuleFor(r => r.FirstName)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(25)
                .WithMessage("{PropertyName} must not be more than 25 characters");

            RuleFor(r => r.LastName)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(25)
                .WithMessage("{PropertyName} must not be more than 25 characters");

            RuleFor(r => r.Email).NotEmpty()
                .EmailAddress();

            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(r => r.Gender)
                .IsInEnum()
                .WithMessage("{PropertyName} is required");

            RuleFor(r => r.DateOfBirth)
                .NotNull()
                .WithMessage("{PropertyName} is required");

            RuleFor(r => r.Address)
                .NotNull()
                .WithMessage("{PropertyName} is required");

            RuleFor(r => r.Address.Street)
                .NotNull()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not be more than 50 characters");

            RuleFor(r => r.Address.City)
                .NotNull()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(25)
                .WithMessage("{PropertyName} must not be more than 25 characters");

            // TODO Switch to custom validator
            RuleFor(r => r.Address.Zipcode)
                .NotNull()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(5)
                .WithMessage("{PropertyName} must not be more than 5 characters");

            RuleFor(r => r.Address.StateId)
                .GreaterThan(0)
                .WithMessage("State is required")
                .Must(_validatorService.IsValidState)
                .WithMessage("Please select a valid state");
        }
    }
}
