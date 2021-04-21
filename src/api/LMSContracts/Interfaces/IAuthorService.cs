using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.Helpers;
using LMSEntities.Models;

namespace LMSContracts.Interfaces
{
    public interface IAuthorService
    {
        Task<Author> AddAuthor(Author author);

        Task DeleteAuthor(Author author);

        Task EditAuthor(Author author);

        Task<Author> GetAuthor(int authorId);

        Task<IEnumerable<Author>> SearchAuthors(string searchString);

        Task<PagedList<Author>> GetPaginatedAuthors(PaginationParams paginationParams);
    }
}
