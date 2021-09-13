using System.Threading.Tasks;

namespace LMSContracts.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string toAddress, string subject, string message);
    }
}
