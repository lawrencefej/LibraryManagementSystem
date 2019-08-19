using AutoMapper;
using LibraryManagementSystem.API.Controllers;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManagementSystem.Tests.ControllerTests
{
    public class CatalogTests 
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly Mapper _mapper;

        public CatalogTests()
        {
            _logger = new NullLogger<CatalogController>();
            _mapper = new Mapper(new MapperConfiguration(cfg
                => cfg.AddProfile(new AutoMapperProfiles())));
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

        private static ControllerContext SetClaimsUser()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }));

            return new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user}
            };
        }

        [Fact]
        public async Task DeleteLibraryAsset_ValidAsset_ReturnsNoContent()
        {
            // Arrange
            var asset = GetAsset();
            var mock = new Mock<ILibraryAssetService>();
            mock.Setup(x => x.DeleteAsset(asset.Id));
            var controller = new CatalogController(mock.Object, _logger, _mapper);

            // Act
            var result = await controller.DeleteLibraryAsset(asset.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task EditAssett_ExistingAsset_ReturnsNoContent()
        {
            // Arrange
            var assetForUpdate = new LibraryAssetForUpdateDto { Id = 4, Title = "Test", Year = 1992, NumberOfCopies = 10, AssetTypeId = 1, AuthorId = 2, CategoryId = 1 };
            var asset = GetAsset();
            var mock = new Mock<ILibraryAssetService>();
            mock.Setup(x => x.GetAsset(asset.Id)).ReturnsAsync(asset);
            mock.Setup(x => x.EditAsset(asset));
            var controller = new CatalogController(mock.Object, _logger, _mapper)
            {
                ControllerContext = SetClaimsUser()
            };

            // Act
            var result = await controller.EditAsset(assetForUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task EditAssett_NonExistingAsset_ReturnsNoContent()
        {
            // Arrange
            var assetForUpdate = new LibraryAssetForUpdateDto { Id = 4, Title = "Test", Year = 1992, NumberOfCopies = 10, AssetTypeId = 1, AuthorId = 2, CategoryId = 1 };
            var asset = GetAsset();
            var mock = new Mock<ILibraryAssetService>();
            mock.Setup(x => x.EditAsset(asset));
            var controller = new CatalogController(mock.Object, _logger, _mapper)
            {
                ControllerContext = SetClaimsUser()
            };

            // Act
            var result = await controller.EditAsset(assetForUpdate);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetLibraryAsset_ExistingAsset_ReturnsOk()
        {
            // Arrange
            var asset = GetAsset();
            var mock = new Mock<ILibraryAssetService>();
            mock.Setup(x => x.GetAsset(asset.Id)).ReturnsAsync(asset);
            var controller = new CatalogController(mock.Object, _logger, _mapper)
            {
                ControllerContext = SetClaimsUser()
            };

            // Act
            var result = await controller.GetLibraryAsset(asset.Id);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsType<LibraryAssetForDetailedDto>(okResult.Value);
            Assert.Equal(asset.Id, (okResult.Value as LibraryAssetForDetailedDto).Id);
        }

        [Fact]
        public async Task GetLibraryAsset_NonExistingAsset_ReturnsNoContent()
        {
            // Arrange
            var asset = GetAsset();
            var mock = new Mock<ILibraryAssetService>();
            mock.Setup(x => x.GetAsset(100)).ReturnsAsync(asset);
            var controller = new CatalogController(mock.Object, _logger, _mapper)
            {
                ControllerContext = SetClaimsUser()
            };

            // Act
            var result = await controller.GetLibraryAsset(asset.Id);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.Null(okResult);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetLibraryAssets_ExistingAssets_ReturnsOk()
        {
            // Arrange
            var assets = GetAllAssets().ToList();
            var pagedAssets = new PagedList<LibraryAsset>(assets, 1, 1, 1);
            var paginationParams = new PaginationParams();
            var mock = new Mock<ILibraryAssetService>();
            mock.Setup(x => x.GetAllAsync(paginationParams)).ReturnsAsync(pagedAssets);
            var controller = new CatalogController(mock.Object, _logger, _mapper)
            {
                ControllerContext = SetClaimsUser()
            };

            // Act
            var result = await controller.GetLibraryAssets(paginationParams);
            var okResult = result as OkObjectResult;
            var list = okResult.Value as IEnumerable<LibraryAssetForListDto>;
            var actual = list.ToList().Count;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(assets.Count, actual);
        }

        //[Fact]
        //public async Task AddLibraryAsset_ValidObject_ReturnsCreatedResponse()
        //{
        //    // Arrange
        //    var asset = new LibraryAsset
        //    {
        //        Id = 40,
        //        NumberOfCopies = 20,
        //        Title = "Test",
        //        Year = 1992,
        //    };
        //    var mock = new Mock<ILibraryAssetService>();
        //    mock.Setup(x => x.AddAsset(asset)).ReturnsAsync(asset);
        //    var controller = new CatalogController(mock.Object, _logger, _mapper);

        //    // Act
        //    var assetDto = new LibraryAssetForCreationDto
        //    {
        //        NumberOfCopies = 20,
        //        Title = "Test",
        //        Year = 1992,
        //    };
        //    var result = await controller.AddLibraryAsset(assetDto);
        //    var okResult = result as OkObjectResult;
        //    var actual = okResult.Value as LibraryAssetForListDto;

        //    // Assert
        //    Assert.Null(okResult);
        //    Assert.IsType<CreatedAtRouteResult>(result);
        //    Assert.IsType<LibraryAssetForListDto>(actual);
        //    Assert.Equal(assetDto.Title, actual.Title);
        //}
    }
}