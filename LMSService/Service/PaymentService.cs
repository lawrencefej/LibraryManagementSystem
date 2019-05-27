using LMSRepository.Interfaces;
using LMSService.Exceptions;
using LMSService.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly ILibraryCardRepository _libraryCardRepository;
        private readonly ILogger<PaymentService> _logger;
        private readonly ILibraryRepository _libraryRepository;

        public PaymentService(ILibraryCardRepository libraryCardRepository, ILogger<PaymentService> logger,
                                ILibraryRepository libraryRepository)
        {
            _libraryCardRepository = libraryCardRepository;
            _logger = logger;
            _libraryRepository = libraryRepository;
        }

        public async Task PayFees(int libraryCardID)
        {
            var card = await _libraryCardRepository.GetCard(libraryCardID);

            if (card == null)
            {
                _logger.LogWarning($"Library {libraryCardID} does not exist");
                throw new NoValuesFoundException($"LibraryCard {libraryCardID} was not found");
            }

            card.Fees = 0;

            if (await _libraryRepository.SaveAll())
            {
                _logger.LogInformation($"Fees were paid for Library Card {libraryCardID}");
                return;
            }

            throw new Exception($"Unsuccessful payment for card {libraryCardID} failed on save");
        }
    }
}