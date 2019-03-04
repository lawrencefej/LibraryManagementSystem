using AutoMapper;
using LMSRepository.Dto;
using LMSRepository.Interfaces;
using LMSService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILibraryRepository _libraryRepository;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper, ILibraryRepository libraryRepository)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _libraryRepository = libraryRepository;
        }

        public Task AddAuthor(AuthorDto authorDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAuthor(int authorId)
        {
            throw new NotImplementedException();
        }

        public Task EditAuthor(AuthorDto authorDto)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthorDto> GetAuthor(int authorId)
        {
            var author = await _authorRepository.GetAuthor(authorId);

            var authorToReturn = _mapper.Map<AuthorDto>(author);

            return authorToReturn;
        }

        public async Task<IEnumerable<AuthorDto>> GetAuthors()
        {
            var authors = await _authorRepository.GetAuthors();

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return authorsToReturn;
        }

        public async Task<IEnumerable<AuthorDto>> SearchLibraryAsset(string searchString)
        {
            var authors = await _authorRepository.SearchAuthor(searchString);

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return authorsToReturn;
        }
    }
}