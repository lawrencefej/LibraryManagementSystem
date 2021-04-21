using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSRepository.Data;
using LMSService.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly DataContext _context;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(DataContext context, ILogger<PaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task PayFees(int libraryCardID)
        {
            var card = await _context.LibraryCards
                .FirstOrDefaultAsync(p => p.Id == libraryCardID);

            if (card == null)
            {
                _logger.LogWarning($"Library {libraryCardID} does not exist");
                throw new NoValuesFoundException($"LibraryCard {libraryCardID} was not found");
            }

            card.Fees = 0;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Fees were paid for Library Card {libraryCardID}");
        }
    }
}
