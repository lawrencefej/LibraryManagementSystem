using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IPaymentService
    {
        Task PayFees(int libraryCardID);
    }
}