using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;

namespace LMSContracts.Interfaces
{
    public interface ILibraryCardService
    {
        Task<LibraryCardForDetailedDto> AddLibraryCard(LibraryCardForCreationDto addCardDto);
        Task<LibraryCard> GetLibraryCardByNumber(string cardNumber);
        Task<LibraryCard> GetLibraryCardById(int id);
        Task<bool> DoesLibraryCardExist(string cardNumber);
        Task<PagedList<LibraryCard>> GetAllLibraryCard(PaginationParams paginationParams);
        Task<IEnumerable<LibraryCard>> SearchLibraryCard(LibraryCard card);
        Task DeleteLibraryCard(LibraryCard card);
        Task<LibraryCard> UpdateLibraryCard(LibraryCard card);
        // Task<LibraryCard> AddLibraryCard(LibraryCard card);
    }
}
