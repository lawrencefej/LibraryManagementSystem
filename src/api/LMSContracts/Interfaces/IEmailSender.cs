using System.Threading.Tasks;

namespace LMSContracts.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string toAddress, string subject, string message);
    }
}
