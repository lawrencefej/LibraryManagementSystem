using LMSLibrary.Dto;
using LMSService.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IReserveService
    {
        Task<IEnumerable<ReserveForReturnDto>> GetAllReserves();
        Task<ReserveForReturnDto> GetReserve(int id);
        Task<IEnumerable<ReserveForReturnDto>> GetReservesForMember(int id);
        Task<ResponseHandler> ReserveAsset(ReserveForCreationDto reserveforForCreationDto);
        Task<ResponseHandler> CancelReserve(int id);
        Task<ResponseHandler> ExpireReserveAsset(int id);
    }
}
