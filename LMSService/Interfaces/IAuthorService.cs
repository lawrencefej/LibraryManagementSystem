using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Dto
{
    public interface IAuthorService
    {
        Task<AuthorDto> AddAuthor(AuthorDto authorDto);

        Task DeleteAuthor(int authorId);

        Task EditAuthor(AuthorDto authorDto);

        Task<IEnumerable<AuthorDto>> GetAuthors();

        Task<AuthorDto> GetAuthor(int authorId);

        Task<IEnumerable<AuthorDto>> SearchAuthors(string searchString);

        Task<PagedList<Author>> GetAllAsync(PaginationParams paginationParams);
    }
}