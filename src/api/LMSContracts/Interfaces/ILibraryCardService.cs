using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;

namespace LMSContracts.Interfaces
{
    public interface ILibraryCardService
    {
        Task<LmsResponseHandler<LibraryCardForDetailedDto>> AddLibraryCard(LibraryCardForCreationDto addCardDto);

        Task<PagedList<LibrarycardForListDto>> AdvancedLibraryCardSearch(LibraryCardForAdvancedSearch card, PaginationParams paginationParams);

        Task<LmsResponseHandler<LibraryCardForDetailedDto>> GetLibraryCardByNumber(string cardNumber);

        Task<LmsResponseHandler<LibraryCardForDetailedDto>> GetLibraryCardById(int id);

        Task<PagedList<LibrarycardForListDto>> GetAllLibraryCard(PaginationParams paginationParams);

        Task<LmsResponseHandler<LibraryCardForDetailedDto>> DeleteLibraryCard(int cardId);

        Task<LmsResponseHandler<LibraryCardForDetailedDto>> ReactivateLibraryCard(int cardId);

        Task<LmsResponseHandler<LibraryCardForDetailedDto>> UpdateLibraryCard(LibraryCardForUpdate cardForUpdate);
    }
}
