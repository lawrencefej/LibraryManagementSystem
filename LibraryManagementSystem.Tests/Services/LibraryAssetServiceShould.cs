using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Exceptions;
using LMSService.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManagementSystem.Tests.Services
{
    public class LibraryAssetServiceShould
    {
        private readonly ILogger<LibraryAssetService> _logger;
        private readonly Mapper _mapper;
        private TestDataContextFactory _factory;

        public LibraryAssetServiceShould()
        {
            _logger = new NullLogger<LibraryAssetService>();
            _mapper = new Mapper(new MapperConfiguration(cfg
                => cfg.AddProfile(new AutoMapperProfiles())));
            _factory = new TestDataContextFactory();
        }

        private static IEnumerable<LibraryAsset> GetAllAssets()
        {
            return new List<LibraryAsset>
            {
                new LibraryAsset
                {
                    Id = 1,
                    Title = "Test",
                    Year = 1992,
                    StatusId = 1,
                    NumberOfCopies = 10,
                    CopiesAvailable = 10,
                    AssetTypeId = 1,
                    AuthorId = 1,
                    CategoryId = 1
                },
                new LibraryAsset{Id = 2, AuthorId = 1, StatusId = 1, AssetTypeId = 1, CategoryId = 1},
                new LibraryAsset{Id = 3, AuthorId = 1, StatusId = 1, AssetTypeId = 1, CategoryId = 1}
            };
        }

        private static LibraryAsset GetAsset()
        {
            return new LibraryAsset { Id = 4, Title = "Test", Year = 1992, NumberOfCopies = 10, AssetTypeId = 1, AuthorId = 2, CategoryId = 1 };
        }

        private static Author GetAuthor()
        {
            return new Author { Id = 1 };
        }

        [Fact]
        public async Task AddAsset_ValidAsset_ShouldAddNewAsset()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new LibraryAssetService(context, _logger);

                // Act
                await service.AddAsset(GetAsset());
                var actual = context.LibraryAssets.Single();
                var expected = GetAsset();
                expected.StatusId = (int)EnumStatus.Available;

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(expected.NumberOfCopies, actual.CopiesAvailable);
                Assert.Equal(expected.StatusId, actual.StatusId);
                Assert.Equal(expected.Id, actual.Id);
                Assert.Single(context.LibraryAssets.ToList());
            }
        }

        [Fact]
        public async Task AddAsset_ExistingAsset_ShouldDeleteAsset()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new LibraryAssetService(context, _logger);
                context.Add(GetAsset());
                context.SaveChanges();

                // Act
                await service.DeleteAsset(GetAsset().Id);

                // Assert
                Assert.Equal(0, context.LibraryAssets.Count());
            }
        }

        [Fact]
        public async Task AddAsset_NonExistingAsset_ShouldDeleteAsset()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new LibraryAssetService(context, _logger);

                // Assert
                await Assert.ThrowsAsync<NoValuesFoundException>(() => service.DeleteAsset(GetAsset().Id));
            }
        }

        [Fact]
        public async Task EditAsset_ValidAsset_ShouldEditAsset()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new LibraryAssetService(context, _logger);
                var asset = GetAsset();
                context.Add(asset);
                context.SaveChanges();
                asset.Year = 2000;

                // Act
                await service.EditAsset(asset);
                var actual = context.LibraryAssets.Single();
                var expected = asset;

                // Assert
                Assert.Equal(expected.Year, actual.Year);
            }
        }

        [Fact]
        public async Task EditAsset_UnAvailableAsset_ShouldMakeAssetVailable()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new LibraryAssetService(context, _logger);
                var asset = GetAsset();
                asset.StatusId = (int)EnumStatus.Unavailable;
                asset.CopiesAvailable = 0;
                context.Add(asset);
                context.SaveChanges();
                asset.CopiesAvailable = 10;

                // Act
                await service.EditAsset(asset);
                var actual = context.LibraryAssets.Single();
                var expected = asset;

                // Assert
                Assert.Equal(expected.StatusId, actual.StatusId);
                Assert.Equal(asset.CopiesAvailable, actual.CopiesAvailable);
            }
        }

        [Fact]
        public async void GetAllAsync_PaginatedList_ReturnsPaginatedList()
        {
            using (var context = _factory.UseSqlite())
            {
                ILogger<LibraryAssetService> logger = new NullLogger<LibraryAssetService>();

                context.AddRange(GetAllAssets());
                context.Add(GetAuthor());
                context.SaveChanges();

                var service = new LibraryAssetService(context, _logger);
                var paginationParams = new PaginationParams();
                var actual = await service.GetAllAsync(paginationParams);
                var assets = GetAllAssets().ToList();

                Assert.Equal(actual.Count, assets.Count);
            }
        }

        [Fact]
        public async void GetAssetById_ExistingAsset_ReturnAsset()
        {
            using (var context = _factory.UseSqlite())
            {
                ILogger<LibraryAssetService> logger = new NullLogger<LibraryAssetService>();

                context.AddRange(GetAllAssets());
                context.Add(GetAuthor());
                context.SaveChanges();

                var service = new LibraryAssetService(context, _logger);
                var actual = await service.GetAsset(1);
                var expected = GetAllAssets().FirstOrDefault();

                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(GetAuthor().Id, actual.Author.Id);
                Assert.Equal(expected.Title, actual.Title);
            }
        }

        [Fact]
        public async void GetAssetsByAuthor_ValidAuthor_ReturnsAllAssetsByAuthor()
        {
            using (var context = _factory.UseSqlite())
            {
                ILogger<LibraryAssetService> logger = new NullLogger<LibraryAssetService>();

                context.AddRange(GetAllAssets());
                context.Add(GetAsset());
                context.Add(GetAuthor());
                context.Add(new Author { Id = 2 });
                context.SaveChanges();

                var service = new LibraryAssetService(context, _logger);
                var actual = await service.GetAssetsByAuthor(GetAuthor().Id);
                var assets = GetAllAssets().ToList();

                Assert.Equal(3, actual.ToList().Count);
            }
        }
    }
}