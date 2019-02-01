using FluentValidation;
using LMSLibrary.Dto;

namespace LMSService.Validators
{
    public class ReserveValidation : AbstractValidator<ReserveForCreationDto>
    {
        private readonly string status = "Available";
        private readonly int maxCheckoutCount = 50;

        public ReserveValidation()
        {
            RuleFor(c => c.Fees).Equal(0).WithMessage("This member still has fees to pay");
            RuleFor(c => c.AssetStatus).Equal(status).WithMessage("This asset is unavailable");
            RuleFor(c => c.CurrentReserveCount).LessThanOrEqualTo(maxCheckoutCount).WithMessage("This member has reached the max amount of current reserves");
        }
    }
}