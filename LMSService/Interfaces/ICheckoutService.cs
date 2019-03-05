using LMSRepository.Interfaces.Dto;
using LMSRepository.Interfaces.Models;
using LMSService.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface ICheckoutService
    {
        Task<CheckoutForReturnDto> CheckInAsset(int id);

        Task<ResponseHandler> CheckoutAsset(CheckoutForCreationDto checkoutForCreation);

        Task<ResponseHandler> CheckoutReservedAsset(int id);

        Task<IEnumerable<CheckoutForReturnDto>> GetAllCheckouts();

        Task<CheckoutForReturnDto> GetCheckout(int id);

        Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForAsset(int libraryAssetId);

        Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForMember(int userId);

        Task<ReserveAsset> GetCurrentReserve(int id);

        Task<LibraryAsset> GetLibraryAsset(int id);

        Task<LibraryCard> GetMemberLibraryCard(int userId);

        void ReduceAssetCopiesAvailable(LibraryAsset asset);

        Task<IEnumerable<CheckoutForReturnDto>> SearchCheckouts(string searchString);
    }
}