using LMSRepository.Models;
using LMSService.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManagementSystem.Tests.Services
{
    public class AssetTypeServiceTests
    {
        private readonly TestDataContextFactory _factory;

        public AssetTypeServiceTests()
        {
            _factory = new TestDataContextFactory();
        }

        private static AssetType GetAssetType()
        {
            return new AssetType { Id = 4, Name = "Test3" };
        }

        [Fact]
        public async Task AddAssetType_ValidAssetType_ShouldAddNewAssetType()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new AssetTypeService(context);

                // Act
                var actual = await service.AddAssetType(GetAssetType());
                var expected = GetAssetType();

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Name, actual.Name);
            }
        }

        [Fact]
        public async Task DeleteAssetType_ExistingAsset_ShouldDeleteAssetType()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new AssetTypeService(context);
                context.Add(GetAssetType());
                context.SaveChanges();
                var assetType = context.AssetTypes.FirstOrDefault(x => x.Id == GetAssetType().Id);

                // Act
                await service.DeleteAssetType(assetType);
                var expected = context.AssetTypes.FirstOrDefault(x => x.Id == GetAssetType().Id);

                // Assert
                Assert.Null(expected);
            }
        }

        [Fact]
        public async void GetAssetTypes_List_ReturnsAssetTypesList()
        {
            using (var context = _factory.UseInMemory())
            {
                //Act
                var service = new AssetTypeService(context);
                var actual = await service.GetAssetTypes();
                var expected = context.AssetTypes.Count();

                //Assert
                Assert.Equal(actual.Count(), expected);
            }
        }

        [Fact]
        public async void GetAssetById_ExistingAsset_ReturnAsset()
        {
            using (var context = _factory.UseInMemory())
            {
                //Arrange
                var assetType = GetAssetType();
                context.Add(assetType);
                context.SaveChanges();

                //Act
                var service = new AssetTypeService(context);
                var actual = await service.GetAssetType(assetType.Id);
                var expected = assetType;

                //Assert
                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Name, actual.Name);
            }
        }
    }
}