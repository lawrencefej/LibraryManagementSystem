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

        Task<PagedList<CheckoutForListDto>> GetCheckoutsForAsset(int libraryAssetId, PaginationParams paginationParams);

        Task<PagedList<CheckoutForListDto>> GetCheckoutsForCard(int LibraryCardId, PaginationParams paginationParams);

        Task<PagedList<CheckoutForListDto>> GetCheckouts(PaginationParams paginationParams);
    }
}
