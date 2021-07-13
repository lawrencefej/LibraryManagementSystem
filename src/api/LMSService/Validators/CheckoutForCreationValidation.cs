// using System.Collections.Generic;
// using System.Linq;
// using FluentValidation;
// using LMSEntities.DataTransferObjects;
// using LMSEntities.Models;
// using LMSService.Validators.services;

// namespace LMSService.Validators
// {
//     public class CheckoutForCreationValidation : AbstractValidator<CheckoutForCreationDto>
//     {
//         private readonly IValidatorService _validatorService;
//         public CheckoutForCreationValidation(IValidatorService validatorService)
//         {
//             _validatorService = validatorService;

//             RuleFor(r => r.LibraryCardId)
//                 .GreaterThan(0)
//                 .WithMessage("Please select a valid card");

//             RuleFor(r => r.Items)
//                 .NotNull()
//                 .WithMessage("Please select at least one item to checkout")
//                 .Must(AreIdsUnique)
//                 .WithMessage("The same item cannot be checked out twice");

//             RuleForEach(r => r.Items)
//                 .ChildRules(s =>
//                 {
//                     s.RuleFor(t => t.LibraryAssetId)
//                         .GreaterThan(0)
//                 .WithMessage("Please select a valid item to checkout")
//                 .Must(_validatorService.DoesAssetExist)
//                 .WithMessage("Item {CollectionIndex} does not exist");
//                 });
//         }

//         private static bool AreIdsUnique(ICollection<CheckoutItemForCreationDto> items)
//         {
//             return items.Select(o => o.LibraryAssetId).Distinct().Count() == items.Count;
//         }
//     }
// }
