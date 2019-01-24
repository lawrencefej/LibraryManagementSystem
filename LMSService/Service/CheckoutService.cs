using AutoMapper;
using LMSLibrary.Data;
using LMSLibrary.Dto;
using LMSLibrary.Models;
using LMSRepository.Interfaces;
using LMSService.Exceptions;
using LMSService.Helpers;
using LMSService.Interfaces;
using LMSService.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly string checkedout = "Checkedout";
        private readonly string unavailable = "Unavailable";
        private readonly string returned = "Returned";
        private readonly ICheckoutRepository _checkoutRepo;
        private readonly ILibraryRepository _libraryRepo;
        private readonly ILibraryCardRepository _cardRepo;
        private readonly ILibraryAssetRepository _assetRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutService> _logger;
        private List<string> errors = new List<string>();

        public CheckoutService(ICheckoutRepository checkoutRepo, ILibraryRepository libraryRepo, 
            ILibraryCardRepository CardRepo, ILibraryAssetRepository AssetRepo, IMapper mapper, ILogger<CheckoutService> logger)
        {
            _checkoutRepo = checkoutRepo;
            _libraryRepo = libraryRepo;
            _cardRepo = CardRepo;
            _assetRepo = AssetRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<Checkout> CheckoutReservedAsset(ReserveAsset reserve)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseHandler> CheckoutAsset(CheckoutForCreationDto checkoutForCreationDto)
        {
            var validate = new CheckoutValidation();
            var libraryCard = await GetLibraryCard(checkoutForCreationDto.LibraryCardId);
            var libraryAsset = await GetLibraryAsset(checkoutForCreationDto.LibraryAssetId);

            checkoutForCreationDto.AssetStatus = libraryAsset.Status.Name;
            checkoutForCreationDto.Fees = libraryCard.Fees;
            checkoutForCreationDto.CurrentCheckoutCount = await _checkoutRepo.GetMemberCurrentCheckoutAmount(libraryCard.Id);

            var result = await validate.ValidateAsync(checkoutForCreationDto);

            ResponseHandler t = new ResponseHandler(checkoutForCreationDto, errors);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors.Add($"{failure.PropertyName}: {failure.ErrorMessage}");
                }
                return t;
            }

            ReduceAssetCopiesAvailable(libraryAsset);

            var checkout = _mapper.Map<Checkout>(checkoutForCreationDto);
            checkout.Status = await _libraryRepo.GetStatus(checkedout);

            _libraryRepo.Add(checkout);

            if (await _libraryRepo.SaveAll())
            {
                var checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);
                t.Id = checkoutToReturn.Id;
                t.Valid = true;
                t.Result = checkoutToReturn;

                return t;
            }

            throw new Exception("Failed to Checkout the item");
        }

        public async Task<IReadOnlyList<string>> ValidateCheckout(CheckoutForCreationDto checkout)
        {
            var validate = new CheckoutValidation();
            var libraryCard = await GetLibraryCard(checkout.LibraryCardId);
            var libraryAsset = await GetLibraryAsset(checkout.LibraryAssetId);
            

            checkout.AssetStatus = libraryAsset.Status.Name;
            checkout.Fees = libraryCard.Fees;
            checkout.CurrentCheckoutCount = await _checkoutRepo.GetMemberCurrentCheckoutAmount(libraryCard.Id);

            var result = await validate.ValidateAsync(checkout);

            var result2 = validate.Validate(checkout);

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

        public async Task<CheckoutForReturnDto> CheckInAsset(int id)
        {
            var checkout = await ValidateCheckin(id);

            checkout.Status = await _libraryRepo.GetStatus(returned);

            checkout.DateReturned = DateTime.Now;

            var libraryAsset = await GetLibraryAsset(checkout.LibraryAssetId);

            libraryAsset.CopiesAvailable++;

            if (await _libraryRepo.SaveAll())
            {
                return _mapper.Map<CheckoutForReturnDto>(checkout);
            }

            throw new Exception($"returning {checkout.Id} failed on save");
        }

        private async Task<Checkout> ValidateCheckin(int id)
        {
            var checkout = await _checkoutRepo.GetCheckout(id);

            if (checkout == null)
            {
                _logger.LogError("Checkout was null");
                throw new NoValuesFoundException("Checkout does not exist");
            }

            if (checkout.Status.Name == "Returned" || checkout.DateReturned != null)
            {
                _logger.LogError("Checkout has already been returned");
                throw new LMSValidationException("Checkout has already been returned");
            }

            return checkout;
        }

        private async Task<LibraryAsset> GetLibraryAsset(int id)
        {
            var asset = await _assetRepo.GetAsset(id);

            if (asset == null)
            {
                _logger.LogError("Library asset was null");
                throw new NoValuesFoundException("LibraryAsset does not exist");
            }

            return asset;
        }

        private async Task<LibraryCard> GetLibraryCard(int id)
        {
            var card = await _cardRepo.GetCard(id);

            if (card == null)
            {
                _logger.LogError("Library Card was null");
                throw new NoValuesFoundException("LibraryCard does not exist");
            }

            return card;
        }

        private async void ReduceAssetCopiesAvailable(LibraryAsset asset)
        {
            asset.CopiesAvailable--;

            if (asset.CopiesAvailable == 0)
            {
                asset.Status = await _libraryRepo.GetStatus(unavailable);
            }
        }

        internal void IsAssetAvailable(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseHandler> CreateCheckout(CheckoutForCreationDto checkoutForCreation)
        {
            throw new NotImplementedException();
        }
    }
}
