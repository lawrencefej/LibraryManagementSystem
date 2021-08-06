using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class AuthorService : IAuthorService
    {
        // TODO fix to use base api
        private readonly DataContext _context;
        private readonly ILogger<AuthorService> _logger;
        private readonly IMapper _mapper;

        public AuthorService(DataContext context, IMapper mapper, ILogger<AuthorService> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<AuthorDto> AddAuthor(AuthorDto authorDto)
        {
            Author author = _mapper.Map<Author>(authorDto);

            _context.Add(author);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"");

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<LmsResponseHandler<AuthorDto>> DeleteAuthor(int authorId)
        {
            Author author = await GetAuthor(authorId);

            if (author != null)
            {
                _context.Remove(author);
                await _context.SaveChangesAsync();

                return LmsResponseHandler<AuthorDto>.Successful();
            }
            return LmsResponseHandler<AuthorDto>.Failed("");
        }

        public async Task<LmsResponseHandler<AuthorDto>> EditAuthor(AuthorDto authorDto)
        {
            Author author = await GetAuthor(authorDto.Id);

            if (author != null)
            {
                _context.Update(author);
                await _context.SaveChangesAsync();

                return LmsResponseHandler<AuthorDto>.Successful(_mapper.Map<AuthorDto>(author));
            }

            return LmsResponseHandler<AuthorDto>.Failed("");
        }

        public async Task<LmsResponseHandler<AuthorDto>> GetAuthorForController(int authorId)
        {
            Author author = await GetAuthor(authorId);

            if (author != null)
            {
                AuthorDto authorForReturn = _mapper.Map<AuthorDto>(author);

                return LmsResponseHandler<AuthorDto>.Successful(authorForReturn);
            }

            return LmsResponseHandler<AuthorDto>.Failed("");
        }

        public async Task<PagedList<AuthorDto>> GetPaginatedAuthors(PaginationParams paginationParams)
        {
            IQueryable<Author> authors = _context.Authors.AsNoTracking().AsQueryable();

            authors = FilterAuthors(paginationParams, authors);

            PagedList<Author> authorsToReturn = await PagedList<Author>.CreateAsync(authors, paginationParams.PageNumber, paginationParams.PageSize);

            return _mapper.Map<PagedList<AuthorDto>>(authorsToReturn);
        }

        private async Task<Author> GetAuthor(int authorId)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
        }

        private static IQueryable<Author> FilterAuthors(PaginationParams paginationParams, IQueryable<Author> authors)
        {
            if (!string.IsNullOrEmpty(paginationParams.SearchString))
            {
                authors = authors
                    .Where(x => x.FullName.Contains(paginationParams.SearchString));
            }

            authors = paginationParams.SortDirection == "desc" ? authors.OrderByDescending(x => x.FullName) : authors.OrderBy(x => x.FullName);

            return authors;
        }
    }
}
