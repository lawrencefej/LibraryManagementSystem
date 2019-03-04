using FluentValidation;
using LMSRepository.Interfaces.Dto;

namespace LMSService.Validators
{
    public class CheckoutValidation : AbstractValidator<CheckoutForCreationDto>
    {
        private readonly string status = "Available";
        private readonly int maxCheckoutCount = 50;

        public CheckoutValidation()
        {
            RuleFor(c => c.Fees).Equal(0).WithMessage("This member still has fees to pay");
            RuleFor(c => c.AssetStatus).Equal(status).WithMessage("This asset is unavailable");
            RuleFor(c => c.CurrentCheckoutCount).LessThanOrEqualTo(maxCheckoutCount).WithMessage("This member has reached the max amount of current checkouts");
        }
    }
}