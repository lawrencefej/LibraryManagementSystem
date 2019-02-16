using LMSLibrary.Dto;
using LMSLibrary.Models;
using LMSService.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IReserveService
    {
        Task<ResponseHandler> CancelReserve(int userId, int id);

        Task<ResponseHandler> ExpireReserveAsset(int id);

        Task<IEnumerable<ReserveForReturnDto>> GetAllReserves();

        Task<IEnumerable<CheckoutForReturnDto>> GetCurrentCheckoutsForMember(int userId);

        Task<ReserveForReturnDto> GetReserveForMember(int userId, int id);

        Task<IEnumerable<ReserveForReturnDto>> GetReservesForMember(int userId);

        Task<ResponseHandler> ReserveAsset(int userId, int assetId);

        Task<IEnumerable<ReserveAsset>> Test();
    }
}