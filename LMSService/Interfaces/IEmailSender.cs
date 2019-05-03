using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string toAddress, string subject, string message);
    }
}