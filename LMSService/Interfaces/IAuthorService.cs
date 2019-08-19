using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IAuthorService
    {
        Task<Author> AddAuthor(Author author);

        Task DeleteAuthor(Author author);

        Task EditAuthor(Author author);

        Task<Author> GetAuthor(int authorId);

        Task<IEnumerable<Author>> SearchAuthors(string searchString);

        Task<PagedList<Author>> GetAllAsync(PaginationParams paginationParams);
    }
}