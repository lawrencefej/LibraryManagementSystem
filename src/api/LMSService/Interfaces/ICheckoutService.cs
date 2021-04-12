using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface ICheckoutService
    {
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