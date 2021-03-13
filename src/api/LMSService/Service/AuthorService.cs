using LMSRepository.Data;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly DataContext _context;

        public AuthorService(DataContext context)
        {
            _context = context;
        }

        public async Task<Author> AddAuthor(Author author)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();

            return author;
        }

        public async Task DeleteAuthor(Author author)
        {
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
            var authors = _context.Authors.AsNoTracking().AsQueryable();
            // TODO make this case insensitive
            authors = authors.Where(s => s.FirstName.Contains(searchString)
                    || s.LastName.Contains(searchString));

            return await authors.ToListAsync();
        }

        public async Task<PagedList<Author>> GetPaginatedAuthors(PaginationParams paginationParams)
        {
            var authors = _context.Authors.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(paginationParams.SearchString))
            {
                authors = authors
                    .Where(x => x.FirstName.Contains(paginationParams.SearchString)
                    || x.LastName.Contains(paginationParams.SearchString));
            }

            if (paginationParams.SortDirection == "asc")
            {
                if (string.Equals(paginationParams.OrderBy, "firstname", StringComparison.CurrentCultureIgnoreCase))
                {
                    authors = authors.OrderBy(x => x.FirstName);
                }
                else if (string.Equals(paginationParams.OrderBy, "lastname", StringComparison.CurrentCultureIgnoreCase))
                {
                    authors = authors.OrderBy(x => x.LastName);
                }
            }
            else if (paginationParams.SortDirection == "desc")
            {
                if (string.Equals(paginationParams.OrderBy, "firstname", StringComparison.CurrentCultureIgnoreCase))
                {
                    authors = authors.OrderByDescending(x => x.FirstName);
                }
                else if (string.Equals(paginationParams.OrderBy, "lastname", StringComparison.CurrentCultureIgnoreCase))
                {
                    authors = authors.OrderByDescending(x => x.LastName);
                }
            }
            else
            {
                authors = authors.OrderBy(x => x.LastName);
            }

            return await PagedList<Author>.CreateAsync(authors, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}