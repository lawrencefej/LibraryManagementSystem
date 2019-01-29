using AutoMapper;
using LMSLibrary.Data;
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

            reserve.StatusId = (int)StatusEnum.Canceled;
            
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

            reserve.StatusId = (int)StatusEnum.Expired;

            if (await _libraryRepo.SaveAll())
            {
                ResponseHandler response = new ResponseHandler();
                response.Valid = true;
                return response;
            }

            throw new Exception("Failed to cancel the reserve");
        }

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

        public async Task<ResponseHandler> ReserveAsset(int userId, ReserveForCreationDto reserveforForCreationDto)
        {
            var libraryCard = await _checkoutService.GetMemberLibraryCard(userId);
            var libraryAsset = await _checkoutService.GetLibraryAsset(reserveforForCreationDto.LibraryAssetId);


            reserveforForCreationDto.AssetStatus = libraryAsset.Status.Name;
            reserveforForCreationDto.Fees = libraryCard.Fees;
            reserveforForCreationDto.LibraryCardId = libraryCard.Id;
            reserveforForCreationDto.CurrentReserveCount = await _reserveRepo.GetMemberCurrentReserveAmount(libraryCard.Id);


            var validate = new ReserveValidation();
            var result = await validate.ValidateAsync(reserveforForCreationDto);

            var errors = new List<string>();
            ResponseHandler response = new ResponseHandler(reserveforForCreationDto, errors);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors.Add($"{failure.PropertyName}: {failure.ErrorMessage}");
                }
                return response;
            }

            _checkoutService.ReduceAssetCopiesAvailable(libraryAsset);

            var reserve = _mapper.Map<ReserveAsset>(reserveforForCreationDto);

            reserve.StatusId = (int)StatusEnum.Reserved;

            _libraryRepo.Add(reserve);

            if (await _libraryRepo.SaveAll())
            {
                response.Valid = true;
                return response;
            }

            throw new Exception("Failed to reserve the asset on save");
        }
    }
}
