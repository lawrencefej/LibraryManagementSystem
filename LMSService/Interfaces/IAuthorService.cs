using LMSRepository.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorDto> AddAuthor(AuthorDto authorDto);

        Task DeleteAuthor(int authorId);

        Task EditAuthor(AuthorDto authorDto);

        Task<IEnumerable<AuthorDto>> GetAuthors();

        Task<AuthorDto> GetAuthor(int authorId);

        Task<IEnumerable<AuthorDto>> SearchAuthors(string searchString);
    }
}