using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;

namespace LMSContracts.Interfaces
{
    public interface ICheckoutService
    {
        Task<LmsResponseHandler<CheckoutForReturnDto>> CheckoutItems(LibraryCard card, CheckoutForCreationDto checkoutDto);
        Task<LibraryCard> GetMemberLibraryCard(string CardNumber);
        Task CheckInAsset(int checkoutId);

        Task<CheckoutForReturnDto> CheckoutAsset(CheckoutForCreationDto checkoutForCreation);

        Task CheckoutAsset(IEnumerable<CheckoutForCreationDto> checkoutsForCreation);

        Task<Checkout> GetCheckout(int checkoutId);

        Task<IEnumerable<Checkout>> GetCheckoutsForAsset(int libraryAssetId);

        Task<IEnumerable<Checkout>> GetCheckoutsForMember(int userId);

        Task<LibraryAsset> GetLibraryAsset(int id);

        Task<IEnumerable<Checkout>> SearchCheckouts(string searchString);

        Task<PagedList<Checkout>> GetAllCurrentCheckouts(PaginationParams paginationParams);
    }
}
