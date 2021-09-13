using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;

namespace LMSContracts.Interfaces
{
    public interface ICheckoutService
    {
        Task<LmsResponseHandler<CheckoutForDetailedDto>> CheckoutAssets(BasketForCheckoutDto basketForCheckout);

        Task<LmsResponseHandler<CheckoutForDetailedDto>> CheckInAsset(CheckoutForCheckInDto checkoutForCheckIn);

        Task<LmsResponseHandler<CheckoutForDetailedDto>> GetCheckoutWithDetails(int checkoutId);

        Task<PagedList<CheckoutForListDto>> GetCheckoutsForAsset(int libraryAssetId, PaginationParams paginationParams);

        Task<PagedList<CheckoutForListDto>> GetCheckoutsForCard(int LibraryCardId, PaginationParams paginationParams);

        Task<PagedList<CheckoutForListDto>> GetCheckouts(PaginationParams paginationParams);
    }
}
