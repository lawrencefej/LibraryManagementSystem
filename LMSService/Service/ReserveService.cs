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
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class ReserveService : IReserveService
    {
        private readonly ILibraryRepository _libraryRepo;
        private readonly ILibraryCardRepository _cardRepo;
        private readonly ILibraryAssetRepository _assetRepo;
        private readonly IMapper _mapper;
        private readonly IReserveRepository _reserveRepo;
        private readonly ILogger<ReserveService> _logger;
        private readonly IUserRepository _userRepo;
        private readonly ICheckoutService _checkoutService;

        public ReserveService(IReserveRepository reserveRepo, ILibraryRepository libraryRepo, ILibraryCardRepository cardRepo,
            ILibraryAssetRepository assetRepo, IMapper mapper, ILogger<ReserveService> logger, IUserRepository userRepo, ICheckoutService checkoutService)
        {
            _libraryRepo = libraryRepo;
            _cardRepo = cardRepo;
            _assetRepo = assetRepo;
            _mapper = mapper;
            _reserveRepo = reserveRepo;
            _logger = logger;
            _userRepo = userRepo;
            _checkoutService = checkoutService;
        }

        public async Task<ResponseHandler> CancelReserve(int userId, int id)
        {
            var reserve = await GetReservedAsset(userId, id);

            reserve.StatusId = (int)EnumStatus.Canceled;

            if (await _libraryRepo.SaveAll())
            {
                ResponseHandler response = new ResponseHandler();
                response.Valid = true;
                return response;
            }

            throw new Exception("Failed to cancel the reserve");
        }

        public async Task<ResponseHandler> ExpireReserveAsset(int id)
        {
            var reserve = await _checkoutService.GetCurrentReserve(id);

            reserve.StatusId = (int)EnumStatus.Expired;

            if (await _libraryRepo.SaveAll())
            {
                ResponseHandler response = new ResponseHandler();
                response.Valid = true;
                return response;
            }

            throw new Exception("Failed to cancel the reserve");
        }

        //public async Task<ResponseHandler> AutomatedExpireReserveAsset()
        //{
        //    var reserve = await _checkoutService.GetAllCheckouts();

        //    reserve.StatusId = (int)StatusEnum.Expired;

        //    if (await _libraryRepo.SaveAll())
        //    {
        //        ResponseHandler response = new ResponseHandler();
        //        response.Valid = true;
        //        return response;
        //    }

        //    throw new Exception("Failed to cancel the reserve");
        //}

        public async Task<IEnumerable<ReserveForReturnDto>> GetAllReserves()
        {
            var reserves = await _reserveRepo.GetAllReserves();

            var reserveToReturn = _mapper.Map<IEnumerable<ReserveForReturnDto>>(reserves);

            return reserveToReturn;
        }

        public async Task<ReserveForReturnDto> GetReserveForMember(int userId, int id)
        {
            var reserve = await GetReservedAsset(userId, id);

            var reserveToReturn = _mapper.Map<ReserveForReturnDto>(reserve);

            return reserveToReturn;
        }

        private async Task<ReserveAsset> GetReservedAsset(int userId, int id)
        {
            var card = await _checkoutService.GetMemberLibraryCard(userId);

            if (!card.ReservedAssets.Any(p => p.Id == id))
            {
                throw new LMSUnauthorizedException();
            }

            var reserve = await _reserveRepo.GetReserve(id);

            return reserve;
        }

        public async Task<IEnumerable<ReserveForReturnDto>> GetReservesForMember(int userId)
        {
            var card = await _checkoutService.GetMemberLibraryCard(userId);
            var reserves = await _reserveRepo.GetReservesForMember(card.Id);

            var reservesToReturn = _mapper.Map<IEnumerable<ReserveForReturnDto>>(reserves);

            return reservesToReturn;
        }

        public async Task<ResponseHandler> ReserveAsset(int userId, int assetId)
        {
            // TODO refine this method and the validation
            var libraryCard = await _checkoutService.GetMemberLibraryCard(userId);
            var libraryAsset = await _checkoutService.GetLibraryAsset(assetId);

            var reserves = await _reserveRepo.GetReservesForMember(libraryCard.Id);

            if (reserves.Any(a => a.LibraryAssetId == libraryAsset.Id))
            {
                throw new LMSValidationException($"you have already reserved {libraryAsset.Title}");
            }

            var reserve = new ReserveForCreationDto
            {
                LibraryAssetId = libraryAsset.Id,
                AssetStatus = libraryAsset.Status.Name,
                Fees = libraryCard.Fees,
                LibraryCardId = libraryCard.Id,
                CurrentReserveCount = await _reserveRepo.GetMemberCurrentReserveAmount(libraryCard.Id)
            };

            var validate = new ReserveValidation();
            var result = await validate.ValidateAsync(reserve);

            var errors = new List<string>();
            ResponseHandler response = new ResponseHandler(reserve, errors);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors.Add($"{failure.PropertyName}: {failure.ErrorMessage}");
                }
                return response;
            }

            _checkoutService.ReduceAssetCopiesAvailable(libraryAsset);

            var reserveAsset = _mapper.Map<ReserveAsset>(reserve);

            reserveAsset.StatusId = (int)EnumStatus.Reserved;

            _libraryRepo.Add(reserveAsset);

            if (await _libraryRepo.SaveAll())
            {
                response.Valid = true;
                return response;
            }

            throw new Exception("Failed to reserve the asset on save");
        }

        public async Task<IEnumerable<ReserveAsset>> Test()
        {
            var assets = await _reserveRepo.GetExpiringReserves();

            return assets;
        }

        public async Task<IEnumerable<CheckoutForReturnDto>> GetCurrentCheckoutsForMember(int userId)
        {
            var checkouts = await _checkoutService.GetCheckoutsForMember(userId);

            return checkouts;
        }
    }
}