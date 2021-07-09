using FluentValidation;
using LMSEntities.DataTransferObjects;
using LMSService.Validators.services;

namespace LMSService.Validators
{
    public class LibraryAssetUpdateValidator : AbstractValidator<LibraryAssetForUpdateDto>
    {
        private readonly IValidatorService _validatorService;
        public LibraryAssetUpdateValidator(IValidatorService validatorService)
        {
            _validatorService = validatorService;

            RuleFor(r => r.AssetType)
                .IsInEnum()
                .WithMessage("Please select a valid type");

            RuleFor(x => x.AssetAuthors)
                .NotNull()
                .WithMessage("Please select at least one Author")
                .ForEach(t =>
                {
                    t.Must(o => _validatorService.DoesAuthorExist(o.AuthorId))
                        .WithMessage("Please select a valid Author");
                });
            RuleFor(x => x.AssetCategories)
                .NotNull()
                .WithMessage("Please select at least one Category")
                .ForEach(t =>
                {
                    t.Must(o => _validatorService.DoesCategoryExist(o.CategoryId))
                        .WithMessage("Please select a valid Category");
                });
            RuleFor(r => r.NumberOfCopies)
                .GreaterThan(0);
            When(p => p.AssetType == LibraryAssetTypeDto.Book, () =>
            {
                RuleFor(r => r.ISBN)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage($"ISBN is required when the selected type is {LibraryAssetTypeDto.Book}")
                .Length(10, 13);
                // .Must(_validatorService.DoesIsbnExist)
                // .WithMessage("ISBN: {PropertyValue} already exists");
            });
            RuleFor(r => r.Title)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(1, 50)
                .WithMessage("{PropertyName} must not be more than 50 characters");

            RuleFor(r => r.Description)
                .Length(1, 250)
                .WithMessage("{PropertyName} must not be more than 250 characters");

            RuleFor(r => r.Year)
                .NotEmpty()
                .Must(_validatorService.IsValidYear)
                .WithMessage("{PropertyValue} is not a valid year");
        }
    }
}
