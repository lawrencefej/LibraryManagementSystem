using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LMSLibrary.Data;
using LMSLibrary.Dto;
using LMSLibrary.Models;
using LMSLibrary.Validators;

namespace LMSLibrary.Services
{
    public class CheckoutService2 : ICheckoutService2
    {
        private readonly ICheckoutRepository2 _checkoutRepo;
        private readonly ILibraryRepository _libraryRepo;
        private readonly ILibraryCardRepository _CardRepo;
        private readonly ILibraryAssetRepository _assetRepo;
        private readonly IMapper _mapper;
        //private readonly IValidator<CheckoutForCreationDto> _validator;
        private readonly string checkedout = "Checkedout";
        private readonly string unavailable = "Unavailable";
        private readonly string returned = "Returned";
        private List<string> errors = new List<string>();


        public CheckoutService2(ICheckoutRepository2 checkoutRepo, ILibraryRepository libraryRepo, 
            ILibraryCardRepository CardRepo, ILibraryAssetRepository AssetRepo, IMapper mapper/*, IValidator<CheckoutForCreationDto> validator*/)
        {
            _checkoutRepo = checkoutRepo;
            _libraryRepo = libraryRepo;
            _CardRepo = CardRepo;
            _assetRepo = AssetRepo;
            _mapper = mapper;
            //_validator = validator;
        }

        public Task<Checkout> CheckoutReservedAsset(ReserveAsset reserve)
        {
            throw new NotImplementedException();
        }

        public async Task<CheckoutForReturnDto> CreateCheckout(CheckoutForCreationDto checkoutForCreation)
        {
            var libraryCard = await _CardRepo.GetCard(checkoutForCreation.LibraryCardId);
            var libraryAsset = await _assetRepo.GetAsset(checkoutForCreation.LibraryAssetId);

            checkoutForCreation.Fees = libraryCard.Fees;

            var validator = new CheckoutForCreationDtoValidator();
            //await _validator.ValidateAndThrowAsync(checkoutForCreation);

            var results = validator.Validate(checkoutForCreation);

            //bool success = results.IsValid;
            //IList<ValidationFailure> failures = results.Errors;

            //if (!results.IsValid)
            //{
            //    Validator.ValidateAndThrow(checkoutForCreation);
            //}

            var checkout = _mapper.Map<Checkout>(checkoutForCreation);

            checkout.Status = await _libraryRepo.GetStatus(checkedout);

            _libraryRepo.Add(checkout);

            ReduceAssetCopiesAvailable(libraryAsset);

            if (await _libraryRepo.SaveAll())
            {
                //throw new Exception("Failed to Checkout the item");
                var checkoutToReturn =  _mapper.Map<CheckoutForReturnDto>(checkout);

                return checkoutToReturn;
                //return checkout;
            }

            //return checkout;
            throw new Exception("Failed to Checkout the item");
        }

        public async Task<IReadOnlyList<string>> ValidateCheckout(CheckoutForCreationDto checkout)
        {
            //errors.Clear();
            var test = new CheckoutForCreationDtoValidator();
            var libraryCard = await _CardRepo.GetCard(checkout.LibraryCardId);
            var libraryAsset = await _assetRepo.GetAsset(checkout.LibraryAssetId);

            //TODO fix null exception

            //checkout.AssetStatus = libraryAsset.Status.Name;
            //checkout.Fees = libraryCard.Fees;

            var result = test.Validate(checkout);

            IsAssetAvailable(libraryAsset);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors.Add($"{failure.PropertyName}: {failure.ErrorMessage}");
                }
            }

            return errors;
        }

        public async Task<IEnumerable<Checkout>> GetAllCheckouts()
        {
            var checkouts = await _checkoutRepo.GetAllCheckouts();

            return checkouts;
        }

        public async Task<CheckoutForReturnDto> GetCheckout(int id)
        {
            var checkout = await _checkoutRepo.GetCheckout(id);

            return _mapper.Map<CheckoutForReturnDto>(checkout);
        }

        public Task<IEnumerable<Checkout>> GetCheckoutsForMember(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Checkout> ReturnCheckedOutAsset(Checkout checkout)
        {
            throw new NotImplementedException();
        }

        private async void ReduceAssetCopiesAvailable(LibraryAsset libraryAsset)
        {
            libraryAsset.CopiesAvailable--;

            if (libraryAsset.CopiesAvailable == 0)
            {
                libraryAsset.Status = await _libraryRepo.GetStatus(unavailable);
            }
        }

        internal void IsAssetAvailable(LibraryAsset asset)
        {
            if (asset.Status.Name == unavailable)
            {
                errors.Add($"{asset.Title} is not available");
            }
        }
    }
}
