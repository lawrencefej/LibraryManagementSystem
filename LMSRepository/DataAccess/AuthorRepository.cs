using LMSRepository.Interfaces.DataAccess;
using LMSRepository.Interfaces.Models;
using LMSRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSRepository.Data;

namespace LMSRepository.DataAccess
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
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