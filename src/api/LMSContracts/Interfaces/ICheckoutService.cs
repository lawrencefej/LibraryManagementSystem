using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;

namespace LMSContracts.Interfaces
{
    public interface ICheckoutService
    {
        Task<LmsResponseHandler<CheckoutForDetailedDto>> CheckoutAssets(Basket basketForCheckout);

        Task<LmsResponseHandler<CheckoutForDetailedDto>> CheckInAsset(CheckoutForCheckInDto checkoutForCheckIn);

        Task<LmsResponseHandler<CheckoutForDetailedDto>> GetCheckoutWithDetails(int checkoutId);

        Task<PagedList<CheckoutForListDto>> GetCurrentCheckoutsForAsset(int libraryAssetId, PaginationParams paginationParams);
        Task<PagedList<CheckoutForListDto>> GetCheckoutHistoryForAsset(int libraryAssetId, PaginationParams paginationParams);

        Task<PagedList<CheckoutForListDto>> GetCurrentCheckoutsForCard(int LibraryCardId, PaginationParams paginationParams);
        Task<PagedList<CheckoutForListDto>> GetCheckoutHistoryForCard(int LibraryCardId, PaginationParams paginationParams);

        Task<PagedList<CheckoutForListDto>> GetAllCurrentCheckouts(PaginationParams paginationParams);
        Task<PagedList<CheckoutForListDto>> GetCheckoutHistory(PaginationParams paginationParams);
    }
}
