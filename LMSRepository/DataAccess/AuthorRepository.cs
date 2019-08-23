using LMSRepository.Data;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    public class AuthorRepository /*: IAuthorRepository*/
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<Author> GetAll()
        {
            var authors = _context.Authors.AsQueryable();

            return authors;
        }

        public async Task<Author> GetAuthor(int authorId)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);

            return author;
        }

        public async Task<IEnumerable<Author>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();

            return authors;
        }

        public async Task<IEnumerable<Author>> SearchAuthor(string searchString)
        {
            var authors = from author in _context.Authors
                          select author;

            if (!string.IsNullOrEmpty(searchString))
            {
                authors = authors
                    .Where(s => s.FirstName.Contains(searchString)
                    || s.LastName.Contains(searchString));

                return await authors.ToListAsync();
            }

            return await GetAuthors();
        }
    }
}