using AutoMapper;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using LMSService.Dto;
using LMSService.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class LibraryAssetService : ILibraryAssestService
    {
        private readonly ILibraryAssetRepository _libraryAssetRepo;
        private readonly ILibraryRepository _libraryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<LibraryAssetService> _logger;

        public LibraryAssetService(ILibraryRepository libraryRepo, IMapper mapper,
            ILibraryAssetRepository libraryAssetRepo, ILogger<LibraryAssetService> logger)
        {
            _libraryRepo = libraryRepo;
            _mapper = mapper;
            _libraryAssetRepo = libraryAssetRepo;
            _logger = logger;
        }

        public async Task<LibraryAssetForDetailedDto> AddAsset(LibraryAssetForCreationDto libraryAssetForCreation)
        {
            if (libraryAssetForCreation == null)
            {
                throw new NoValuesFoundException("The asset is null");
            }

            var asset = _mapper.Map<LibraryAsset>(libraryAssetForCreation);

            asset.StatusId = (int)EnumStatus.Available;
            asset.CopiesAvailable = asset.NumberOfCopies;

            _libraryRepo.Add(asset);

            if (!await _libraryRepo.SaveAll())
            {
                throw new Exception($"Creating {asset.Title} failed on save");
            }

            //var assetToReturn = _mapper.Map<LibraryAssetForListDto>(asset);

            var assetToReturn = await GetAsset(asset.Id);

            _logger.LogInformation($"added {libraryAssetForCreation.Title}");

            return assetToReturn;
        }

        public async Task DeleteAsset(int assetId)
        {
            var asset = await _libraryAssetRepo.GetAsset(assetId);

            _libraryRepo.Delete(asset);

            if (await _libraryRepo.SaveAll())
            {
                throw new Exception($"Deleting {asset.Title} failed on save");
            }

            return;
        }

        public async Task EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate)
        {
            var asset = await _libraryAssetRepo.GetAsset(libraryAssetForUpdate.Id);

            _mapper.Map(libraryAssetForUpdate, asset);

            if (asset.CopiesAvailable > 0)
            {
                asset.StatusId = (int)EnumStatus.Available;
            }

            if (!await _libraryRepo.SaveAll())
            {
                throw new Exception($"Updating {asset.Title} failed on save");
            }

            return;
        }

        public IQueryable<LibraryAssetForListDto> GetAll()
        {
            var assets = _libraryAssetRepo.GetAll();

            //var assetToReturn = assets.ProjectTo<LibraryAssetForListDto>();

            var assetToReturn = _mapper.ProjectTo<LibraryAssetForListDto>(assets);

            return assetToReturn;
        }

        public async Task<IEnumerable<LibraryAssetForListDto>> GetAllAssets()
        {
            var assets = await _libraryAssetRepo.GetLibraryAssets();

            var assetToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            return assetToReturn;
        }

        public async Task<PagedList<LibraryAsset>> GetAllAsync(PaginationParams paginationParams)
        {
            var assets = _libraryAssetRepo.GetAll();

            return await PagedList<LibraryAsset>.CreateAsync(assets, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<LibraryAssetForListDto>> GetAllAssets(PaginationParams paginationParams)
        {
            var assets = await _libraryAssetRepo.GetPagedLibraryAssetsAsync(paginationParams);

            var assetToDto = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            //var assetToReturn = _mapper.Map<PagedList<LibraryAssetForListDto>>(assets);

            var assetToReturn = (PagedList<LibraryAssetForListDto>)assetToDto;

            return assetToReturn;
        }

        public async Task<LibraryAssetForDetailedDto> GetAsset(int assetId)
        {
            var asset = await _libraryAssetRepo.GetAsset(assetId);

            var assetToReturn = _mapper.Map<LibraryAssetForDetailedDto>(asset);

            return assetToReturn;
        }

        public async Task<LibraryAssetForDetailedDto> GetAssetByIsbn(string isbn)
        {
            var asset = await _libraryAssetRepo.GetAssetByIsbn(isbn);

            var assetToReturn = _mapper.Map<LibraryAssetForDetailedDto>(asset);

            return assetToReturn;
        }

        public async Task<IEnumerable<LibraryAssetForListDto>> GetAssetsByAuthor(int authorId)
        {
            var assets = await _libraryAssetRepo.GetAssetsByAuthor(authorId);

            var assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            return assetsToReturn;
        }

        public async Task<IEnumerable<LibraryAssetForListDto>> SearchLibraryAsset(string searchString)
        {
            var assets = await _libraryAssetRepo.SearchLibraryAsset(searchString);

            var assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            return assetsToReturn;
        }

        public async Task<IEnumerable<LibraryAssetForListDto>> SearchLibraryAsset(SearchAssetDto searchAsset)
        {
            if (searchAsset == null)
            {
                return null;
            }
            var assets = await _libraryAssetRepo.SearchAssets(searchAsset);

            var assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            return assetsToReturn;
        }
    }
}