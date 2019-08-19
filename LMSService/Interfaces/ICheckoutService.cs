using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface ICheckoutService
    {
        Task CheckInAsset(int checkoutId);

        Task<ResponseHandler> CheckoutAsset(CheckoutForCreationDto checkoutForCreation);

        //Task<IEnumerable<CheckoutForReturnDto>> GetAllCheckouts();

        Task<CheckoutForReturnDto> GetCheckout(int checkoutId);

        Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForAsset(int libraryAssetId);

        Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForMember(int userId);

        Task<LibraryAsset> GetLibraryAsset(int id);

        Task<LibraryCard> GetMemberLibraryCard(int userId);

        void ReduceAssetCopiesAvailable(LibraryAsset asset);

        Task<IEnumerable<CheckoutForReturnDto>> SearchCheckouts(string searchString);

        Task<PagedList<Checkout>> GetAllCheckouts(PaginationParams paginationParams);
    }
}