using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;

namespace LMSContracts.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorDto> AddAuthor(AuthorDto authorDto);

        Task<LmsResponseHandler<AuthorDto>> DeleteAuthor(int authorId);

        Task<LmsResponseHandler<AuthorDto>> EditAuthor(AuthorDto authorDto);

        Task<LmsResponseHandler<AuthorDto>> GetAuthorForController(int authorId);

        Task<PagedList<AuthorDto>> GetPaginatedAuthors(PaginationParams paginationParams);
    }
}
