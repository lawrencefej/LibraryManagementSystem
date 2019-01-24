using LMSLibrary.Dto;
using LMSLibrary.Models;
using LMSService.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface ICheckoutService
    {
        Task<IEnumerable<Checkout>> GetAllCheckouts();
        Task<CheckoutForReturnDto> GetCheckout(int id);
        Task<ResponseHandler> CreateCheckout(CheckoutForCreationDto checkoutForCreation);
        Task<CheckoutForReturnDto> CheckInAsset(int id);
        Task<Checkout> CheckoutReservedAsset(ReserveAsset reserve);
        Task<IEnumerable<Checkout>> GetCheckoutsForMember(int id);
        Task<IReadOnlyList<string>> ValidateCheckout(CheckoutForCreationDto checkout);
        Task<ResponseHandler> CheckoutAsset(CheckoutForCreationDto checkoutForCreationDto);
    }
}
