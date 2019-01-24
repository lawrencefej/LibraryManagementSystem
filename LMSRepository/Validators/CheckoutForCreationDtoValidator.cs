using FluentValidation;
using LMSLibrary.Dto;

namespace LMSLibrary.Validators
{

    public class CheckoutForCreationDtoValidator : AbstractValidator<CheckoutForCreationDto>
    {
        private readonly string status = "Available";
        private readonly int maxCheckoutCount = 50;

        public CheckoutForCreationDtoValidator()
        {
            RuleFor(c => c.LibraryAssetId).GreaterThan(0);
            RuleFor(c => c.LibraryCardId).GreaterThan(0);
            RuleFor(c => c.Fees).Equal(0).WithMessage("This member still has fees to pay");
            RuleFor(c => c.Since);
            RuleFor(c => c.Until);
            RuleFor(c => c.AssetStatus).Equal(status).WithMessage("This asset is unavailable");
            RuleFor(c => c.CurrentCheckoutCount).GreaterThan(maxCheckoutCount).WithMessage("This member has reached the max amount of current checkouts");

        }
    }
}
