using System.Threading.Tasks;

namespace LMSContracts.Interfaces
{
    public interface IPaymentService
    {
        Task PayFees(int libraryCardID);
    }
}
