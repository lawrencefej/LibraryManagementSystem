using LMSLibrary.Dto;
using LMSLibrary.Models;
using LMSService.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface ICheckoutService
    {
        Task<IEnumerable<CheckoutForReturnDto>> GetAllCheckouts();

        Task<CheckoutForReturnDto> GetCheckout(int id);

        Task<CheckoutForReturnDto> CheckInAsset(int id);

        Task<ResponseHandler> CheckoutReservedAsset(int id);

        Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForMember(int id);

        Task<ResponseHandler> CheckoutAsset(CheckoutForCreationDto checkoutForCreationDto);

        Task<LibraryCard> GetMemberLibraryCard(int id);

        Task<LibraryAsset> GetLibraryAsset(int id);

        Task<ReserveAsset> GetCurrentReserve(int id);

        void ReduceAssetCopiesAvailable(LibraryAsset asset);
    }
}