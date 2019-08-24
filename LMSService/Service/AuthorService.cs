using LMSRepository.Data;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly DataContext _context;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(DataContext context, ILogger<AuthorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Author> AddAuthor(Author author)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"added {author.FullName}");

            return author;
        }

        public async Task DeleteAuthor(Author author)
        {
            //TODO add logs
            _context.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task EditAuthor(Author author)
        {
            _context.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task<Author> GetAuthor(int authorId)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);

            return author;
        }

        public async Task<IEnumerable<Author>> SearchAuthors(string searchString)
        {
            var authors = _context.Authors.AsQueryable();

            authors = authors.Where(s => s.FirstName.Contains(searchString)
                    || s.LastName.Contains(searchString));

            return await authors.ToListAsync();
        }

        public async Task<PagedList<Author>> GetAllAsync(PaginationParams paginationParams)
        {
            var authors = _context.Authors.AsQueryable();

            return await PagedList<Author>.CreateAsync(authors, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}