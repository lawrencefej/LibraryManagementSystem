using LMSLibrary.Dto;
using LMSService.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IReserveService
    {
        Task<IEnumerable<ReserveForReturnDto>> GetAllReserves();
        Task<ReserveForReturnDto> GetReserveForMember(int userId, int id);
        Task<IEnumerable<ReserveForReturnDto>> GetReservesForMember(int userId);
        Task<ResponseHandler> ReserveAsset(int userId, ReserveForCreationDto reserveforForCreationDto);
        Task<ResponseHandler> CancelReserve(int userId,int id);
        Task<ResponseHandler> ExpireReserveAsset(int id);
    }
}
