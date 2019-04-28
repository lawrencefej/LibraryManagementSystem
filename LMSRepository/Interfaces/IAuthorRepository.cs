﻿using LMSRepository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> GetAuthor(int authorId);

        Task<IEnumerable<Author>> GetAuthors();

        Task<IEnumerable<Author>> SearchAuthor(string searchString);

        IQueryable<Author> GetAll();
    }
}