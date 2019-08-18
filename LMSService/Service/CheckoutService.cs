using AutoMapper;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using LMSService.Interfacees;
using LMSService.Exceptions;
using LMSService.Helpers;
using LMSService.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class CheckoutService : ICheckoutService
    {
        protected readonly ILibraryAssetRepository _assetRepo;
        protected readonly ILibraryCardRepository _cardRepo;
        protected readonly ICheckoutRepository _checkoutRepo;
        protected readonly ILibraryRepository _libraryRepo;
        private readonly ILogger<CheckoutService> _logger;
        private readonly IMapper _mapper;
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

        public async Task<CheckoutForReturnDto> CheckInAsset(int id)
        {
            var checkout = await ValidateCheckin(id);

            checkout.StatusId = (int)EnumStatus.Returned;

            checkout.DateReturned = DateTime.Today;

            var libraryAsset = await GetLibraryAsset(checkout.LibraryAssetId);

            libraryAsset.CopiesAvailable++;

            if (await _libraryRepo.SaveAll())
            {
                var checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);

                return checkoutToReturn;
            }

            throw new Exception($"returning {checkout.Id} failed on save");
        }

        public async Task<ResponseHandler> CheckoutAsset(CheckoutForCreationDto checkoutForCreation)
        {
            var libraryCard = await GetMemberLibraryCard(checkoutForCreation.userId);

            await IsAssetCurrentlyCheckedOutByMember(checkoutForCreation.LibraryAssetId, libraryCard.Id);

            var libraryAsset = await GetLibraryAsset(checkoutForCreation.LibraryAssetId);

            checkoutForCreation.AssetStatus = libraryAsset.Status.Name;
            checkoutForCreation.LibraryCardId = libraryCard.Id;
            checkoutForCreation.Fees = libraryCard.Fees;
            checkoutForCreation.CurrentCheckoutCount = await _checkoutRepo.GetMemberCurrentCheckoutAmount(libraryCard.Id);

            var validate = new CheckoutValidation();
            var result = await validate.ValidateAsync(checkoutForCreation);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors.Add($"{failure.ErrorMessage}");
                }
                return new ResponseHandler(errors);
            }

            ReduceAssetCopiesAvailable(libraryAsset);

            var checkout = _mapper.Map<Checkout>(checkoutForCreation);
            checkout.StatusId = (int)EnumStatus.Checkedout;

            _libraryRepo.Add(checkout);

            if (await _libraryRepo.SaveAll())
            {
                var checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);
                checkoutToReturn.Status = nameof(EnumStatus.Checkedout);
                return new ResponseHandler(checkoutToReturn, checkoutToReturn.Id);
            }

            throw new Exception("Failed to Checkout the asset on save");
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
                response.IsSuccessful = true;
                return response;
            }

            throw new Exception("Failed to Checkout the item");
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

        public async Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForAsset(int libraryAssetId)
        {
            var checkouts = await _checkoutRepo.GetCheckoutsForAsset(libraryAssetId);

            return _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);
        }

        public async Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForMember(int userId)
        {
            var card = await GetMemberLibraryCard(userId);

            var checkouts = await _checkoutRepo.GetMemberCurrentCheckouts(card.Id);

            return _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);
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

        public async Task<LibraryCard> GetMemberLibraryCard(int userId)
        {
            var card = await _cardRepo.GetMemberCard(userId);

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

        public async Task<IEnumerable<CheckoutForReturnDto>> SearchCheckouts(string searchString)
        {
            var checkouts = await _checkoutRepo.SearchCheckouts(searchString);

            var checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            return checkoutsToReturn;
        }

        private async Task IsAssetCurrentlyCheckedOutByMember(int assetId, int cardId)
        {
            if (await _checkoutRepo.IsAssetCurrentlyCheckedOutByMember(assetId, cardId))
            {
                throw new LMSValidationException("This asset is currently checked out by this member");
            }
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

        public async Task<PagedList<Checkout>> GetAllAsync(PaginationParams paginationParams)
        {
            var checkouts = _checkoutRepo.GetAll();

            checkouts = checkouts.OrderByDescending(a => a.Since);

            return await PagedList<Checkout>.CreateAsync(checkouts, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}