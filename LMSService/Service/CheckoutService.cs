using AutoMapper;
using LMSLibrary.DataAccess;
using LMSLibrary.Dto;
using LMSLibrary.Models;
using LMSRepository.Helpers;
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
        protected readonly ICheckoutRepository _checkoutRepo;
        protected readonly ILibraryRepository _libraryRepo;
        protected readonly ILibraryCardRepository _cardRepo;
        protected readonly ILibraryAssetRepository _assetRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutService> _logger;
        private readonly IReserveRepository _reserveRepo;
        private List<string> errors = new List<string>();

        public CheckoutService(ICheckoutRepository checkoutRepo, ILibraryRepository libraryRepo,
            ILibraryCardRepository CardRepo, ILibraryAssetRepository AssetRepo, IMapper mapper,
            ILogger<CheckoutService> logger, IReserveRepository reserveRepo)
        {
            _checkoutRepo = checkoutRepo;
            _libraryRepo = libraryRepo;
            _cardRepo = CardRepo;
            _assetRepo = AssetRepo;
            _mapper = mapper;
            _logger = logger;
            _reserveRepo = reserveRepo;
        }

        public CheckoutService()
        {
        }

        public async Task<ResponseHandler> CheckoutReservedAsset(int id)
        {
            ReserveAsset reserve = await GetCurrentReserve(id);

            reserve.StatusId = (int)EnumStatus.Checkedout;
            reserve.DateCheckedOut = DateTime.Now;

            var checkout = new CheckoutForCreationDto()
            {
                LibraryAssetId = reserve.LibraryAssetId,
                LibraryCardId = reserve.LibraryCardId
            };

            var checkoutForCreation = _mapper.Map<Checkout>(checkout);

            _libraryRepo.Add(checkout);

            if (await _libraryRepo.SaveAll())
            {
                ResponseHandler response = new ResponseHandler();
                response.Valid = true;
                return response;
            }

            throw new Exception("Failed to Checkout the item");
        }

        public async Task<ReserveAsset> GetCurrentReserve(int id)
        {
            var reserve = await _reserveRepo.GetReserve(id);

            if (reserve == null)
            {
                throw new NoValuesFoundException("Reserve was not found");
            }

            if (reserve.StatusId == (int)EnumStatus.Checkedout)
            {
                throw new LMSValidationException($"{reserve.Id} has already been checked out");
            }

            if (reserve.StatusId == (int)EnumStatus.Expired)
            {
                throw new LMSValidationException($"{reserve.Id} has expired");
            }

            if (reserve.StatusId == (int)EnumStatus.Canceled)
            {
                throw new LMSValidationException($"{reserve.Id} has been canceled");
            }

            return reserve;
        }

        public async Task<ResponseHandler> CheckoutAsset(CheckoutForCreationDto checkoutForCreationDto)
        {
            var validate = new CheckoutValidation();
            var libraryCard = await GetMemberLibraryCard(checkoutForCreationDto.LibraryCardId);
            var libraryAsset = await GetLibraryAsset(checkoutForCreationDto.LibraryAssetId);

            checkoutForCreationDto.AssetStatus = libraryAsset.Status.Name;
            checkoutForCreationDto.Fees = libraryCard.Fees;
            checkoutForCreationDto.CurrentCheckoutCount = await _checkoutRepo.GetMemberCurrentCheckoutAmount(libraryCard.Id);

            var result = await validate.ValidateAsync(checkoutForCreationDto);

            ResponseHandler response = new ResponseHandler(checkoutForCreationDto, errors);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors.Add($"{failure.PropertyName}: {failure.ErrorMessage}");
                }
                return response;
            }

            ReduceAssetCopiesAvailable(libraryAsset);

            var checkout = _mapper.Map<Checkout>(checkoutForCreationDto);
            checkout.StatusId = (int)EnumStatus.Checkedout;

            _libraryRepo.Add(checkout);

            if (await _libraryRepo.SaveAll())
            {
                var checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);
                response.Id = checkoutToReturn.Id;
                response.Valid = true;
                response.Result = checkoutToReturn;

                return response;
            }

            throw new Exception("Failed to Checkout the asset on save");
        }

        public async Task<IEnumerable<CheckoutForReturnDto>> GetAllCheckouts()
        {
            var checkouts = await _checkoutRepo.GetAllCheckouts();

            if (checkouts == null)
            {
                throw new NoValuesFoundException("There are no checkouts available");
            }

            return _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);
        }

        public async Task<CheckoutForReturnDto> GetCheckout(int id)
        {
            var checkout = await _checkoutRepo.GetCheckout(id);

            if (checkout == null)
            {
                throw new NoValuesFoundException("This checkout does not exist");
            }

            return _mapper.Map<CheckoutForReturnDto>(checkout);
        }

        public async Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForMember(int id)
        {
            var checkouts = await _checkoutRepo.GetCheckoutsForMember(id);

            if (checkouts == null)
            {
                throw new NoValuesFoundException("There are no checkouts available");
            }

            return _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);
        }

        public async Task<CheckoutForReturnDto> CheckInAsset(int id)
        {
            var checkout = await ValidateCheckin(id);

            checkout.StatusId = (int)EnumStatus.Returned;

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

            if (checkout.StatusId == (int)EnumStatus.Returned || checkout.DateReturned != null)
            {
                _logger.LogError("Checkout has already been returned");
                throw new LMSValidationException("Checkout has already been returned");
            }

            return checkout;
        }

        public async Task<LibraryAsset> GetLibraryAsset(int id)
        {
            var asset = await _assetRepo.GetAsset(id);

            if (asset == null)
            {
                _logger.LogError("Library asset was null");
                throw new NoValuesFoundException("LibraryAsset does not exist");
            }

            return asset;
        }

        public async Task<LibraryCard> GetMemberLibraryCard(int id)
        {
            var card = await _cardRepo.GetMemberCard(id);

            if (card == null)
            {
                _logger.LogError("Member has no library Card");
                throw new NoValuesFoundException("LibraryCard does not exist");
            }

            return card;
        }

        public void ReduceAssetCopiesAvailable(LibraryAsset asset)
        {
            asset.CopiesAvailable--;

            if (asset.CopiesAvailable == 0)
            {
                asset.StatusId = (int)EnumStatus.Unavailable;
            }
        }
    }
}