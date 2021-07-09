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

        Task<LmsResponseHandler<LibraryCardForDetailedDto>> GetLibraryCardByNumber(string cardNumber);

        Task<LmsResponseHandler<LibraryCardForDetailedDto>> GetLibraryCardById(int id);

        Task<PagedList<LibrarycardForListDto>> GetAllLibraryCard(PaginationParams paginationParams);

        Task<LmsResponseHandler<LibraryCardForDetailedDto>> DeleteLibraryCard(LibraryCardForDetailedDto cardForDel);

        Task<LmsResponseHandler<LibraryCardForDetailedDto>> UpdateLibraryCard(LibraryCardForUpdate cardForUpdate);
    }
}
