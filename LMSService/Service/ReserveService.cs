using AutoMapper;
using LMSLibrary.Data;
using LMSLibrary.Dto;
using LMSLibrary.Models;
using LMSRepository.Interfaces;
using LMSService.Helpers;
using LMSService.Interfaces;
using LMSService.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class ReserveService : CheckoutService ,IReserveService
    {
        private const string reserved = "Reserved";
        //private readonly ILibraryRepository _libraryRepo;
        //private readonly ILibraryCardRepository _cardRepo;
        //private readonly ILibraryAssetRepository _assetRepo;
        private readonly IMapper _mapper;
        private readonly IReserveRepository _reserveRepo;
        private readonly ILogger<ReserveService> _logger;

        public ReserveService(IReserveRepository reserveRepo, ILibraryRepository libraryRepo, ILibraryCardRepository cardRepo, 
            ILibraryAssetRepository assetRepo, IMapper mapper, ILogger<ReserveService> logger)
        {
            //_libraryRepo = libraryRepo;
            //_cardRepo = cardRepo;
            //_assetRepo = assetRepo;
            _mapper = mapper;
            _reserveRepo = reserveRepo;
            _logger = logger;
        }

        public async Task<ResponseHandler> CancelReserve(int id)
        {
            var reserve = await GetCurrentReserve(id);

            reserve.Status = await  _libraryRepo.GetStatus(canceled);

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
            var reserve = await GetCurrentReserve(id);

            reserve.Status = await _libraryRepo.GetStatus(expired);

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

            //if (reserves == null)
            //{
            //    throw new NoValuesFoundException("There are no reserves available");
            //}

            var reserveToReturn = _mapper.Map<IEnumerable<ReserveForReturnDto>>(reserves);

            return reserveToReturn;
        }

        public async Task<ReserveForReturnDto> GetReserve(int id)
        {
            var reserve = await _reserveRepo.GetReserve(id);

            //if (reserve == null)
            //{
            //    throw new NoValuesFoundException("This checkout does not exist");
            //}

            var reserveToReturn = _mapper.Map<ReserveForReturnDto>(reserve);

            return reserveToReturn;
        }

        public async Task<IEnumerable<ReserveForReturnDto>> GetReservesForMember(int id)
        {
            var reserves = await _reserveRepo.GetReservesForMember(id);

            //if (reserves == null)
            //{
            //    throw new NoValuesFoundException("There are no checkouts available");
            //}

            return _mapper.Map<IEnumerable<ReserveForReturnDto>>(reserves);
        }

        public async Task<ResponseHandler> ReserveAsset(ReserveForCreationDto reserveforForCreationDto)
        {
            var validate = new ReserveValidation();

            var libraryCard = await GetLibraryCard(reserveforForCreationDto.LibraryCardId);
            var libraryAsset = await GetLibraryAsset(reserveforForCreationDto.LibraryAssetId);


            reserveforForCreationDto.AssetStatus = libraryAsset.Status.Name;
            reserveforForCreationDto.Fees = libraryCard.Fees;
            reserveforForCreationDto.CurrentReserveCount = await _reserveRepo.GetMemberCurrentReserveAmount(libraryCard.Id);

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

            ReduceAssetCopiesAvailable(libraryAsset);

            var reserve = _mapper.Map<ReserveAsset>(reserveforForCreationDto);
            reserve.Status = await _libraryRepo.GetStatus(reserved);

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
