using System.Threading.Tasks;
using EmailService;
using EmailService.Configuration;
using EmailService.Model;
using LMSContracts.Interfaces;

namespace LMSService.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly ISmtpConfiguration _smtpConfiguration;

        public EmailSender(IEmailService emailService, ISmtpConfiguration smtpConfiguration)
        {
            _emailService = emailService;
            _smtpConfiguration = smtpConfiguration;
        }

        public async Task SendEmail(string toAddress, string subject, string message)
        {
            var settings = new EmailSettings
            {
                Host = _smtpConfiguration.Host,
                Port = _smtpConfiguration.Port,
                Username = _smtpConfiguration.Username,
                Password = _smtpConfiguration.Password,
                ApiKey = _smtpConfiguration.ApiKey
            };

            var email = new Email
            {
                FromAddress = _smtpConfiguration.Email,
                ToAddress = toAddress,
                FromName = _smtpConfiguration.Name,
                Subject = subject,
                Content = message
            };

            await _emailService.SendEmail(settings, email);
        }
    }
}
