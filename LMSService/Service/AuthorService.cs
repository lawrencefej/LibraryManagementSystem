using AutoMapper;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using LMSService.Interfacees;
using LMSService.Exceptions;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthorService> _logger;
        private readonly ILibraryAssetRepository _libraryAssetRepository;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper, ILibraryRepository libraryRepository,
            ILogger<AuthorService> logger, ILibraryAssetRepository libraryAssetRepository)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _libraryRepository = libraryRepository;
            _logger = logger;
            _libraryAssetRepository = libraryAssetRepository;
        }

        public async Task<AuthorDto> AddAuthor(AuthorDto authorDto)
        {
            //TODO add unique logic
            if (authorDto == null)
            {
                throw new NoValuesFoundException("The author is null");
            }

            var author = _mapper.Map<Author>(authorDto);

            _libraryRepository.Add(author);

            if (!await _libraryRepository.SaveAll())
            {
                throw new Exception($"Creating {author.FullName} failed on save");
            }

            var authorToReturn = _mapper.Map<AuthorDto>(author);

            _logger.LogInformation($"added {author.FullName}");

            return authorToReturn;
        }

        public async Task DeleteAuthor(int authorId)
        {
            //TODO add logs
            var author = await _authorRepository.GetAuthor(authorId);

            _libraryRepository.Delete(author);

            if (await _libraryRepository.SaveAll())
            {
                return;
            }

            throw new Exception($"Deleting {author.FullName} failed on save");
        }

        public async Task EditAuthor(AuthorDto authorDto)
        {
            var author = await _authorRepository.GetAuthor(authorDto.Id);

            _mapper.Map(authorDto, author);

            if (!await _libraryRepository.SaveAll())
            {
                throw new Exception($"Updating {author.FullName} failed on save");
            }

            return;
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

        public async Task<IEnumerable<AuthorDto>> SearchAuthors(string searchString)
        {
            var authors = await _authorRepository.SearchAuthor(searchString);

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return authorsToReturn;
        }

        public async Task<PagedList<Author>> GetAllAsync(PaginationParams paginationParams)
        {
            var authors = _authorRepository.GetAll();

            return await PagedList<Author>.CreateAsync(authors, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}