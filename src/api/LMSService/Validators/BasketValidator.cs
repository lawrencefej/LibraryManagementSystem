using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using LMSEntities.DataTransferObjects;

namespace LMSService.Validators
{
    public class BasketValidator : AbstractValidator<BasketForCheckoutDto>
    {
        public BasketValidator()
        {
            RuleFor(r => r.LibraryCardId)
                .GreaterThan(0)
                .WithMessage("Please select a valid Library Card");

            RuleFor(r => r.Assets)
                .NotNull()
                .WithMessage("Please select at least one item to checkout")
                .Must(AreIdsUnique)
                .WithMessage("The same item cannot be checked out twice");

            RuleForEach(r => r.Assets)
                .ChildRules(s =>
                {
                    s.RuleFor(t => t.LibraryAssetId)
                        .GreaterThan(0)
                        .WithMessage("Please select a valid item to checkout");
                });
        }


        private static bool AreIdsUnique(IList<LibraryAssetForBasket> items)
        {
            return items.Select(o => o.LibraryAssetId).Distinct().Count() == items.Count;
        }
    }
}
