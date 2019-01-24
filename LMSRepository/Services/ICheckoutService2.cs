using FluentValidation.Results;
using LMSLibrary.Dto;
using LMSLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSLibrary.Services
{
    public interface ICheckoutService2
    {
        Task<IEnumerable<Checkout>> GetAllCheckouts();
        Task<CheckoutForReturnDto> GetCheckout(int id);
        Task<CheckoutForReturnDto> CreateCheckout(CheckoutForCreationDto checkoutForCreation);
        Task<Checkout> ReturnCheckedOutAsset(Checkout checkout);
        Task<Checkout> CheckoutReservedAsset(ReserveAsset reserve);
        Task<IEnumerable<Checkout>> GetCheckoutsForMember(int id);
        Task<IReadOnlyList<string>> ValidateCheckout(CheckoutForCreationDto checkout);
    }
}
