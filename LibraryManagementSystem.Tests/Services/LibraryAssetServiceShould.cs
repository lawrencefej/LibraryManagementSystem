using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using LMSService.Exceptions;
using LMSService.Interfacees;
using LMSService.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Linq;
using Xunit;

namespace LibraryManagementSystem.Tests.Services
{
    public class LibraryAssetServiceShould : TestBase
    {
        private readonly ILogger<LibraryAssetService> _logger;
        private readonly Mapper _mapper;

        public LibraryAssetServiceShould()
        {
            _logger = new NullLogger<LibraryAssetService>();
            _mapper = new Mapper(new MapperConfiguration(cfg
                => cfg.AddProfile(new AutoMapperProfiles())));
        }

        [Fact]
        public async void AddASSet_AddNewAsset_ShouldAddNewAsset()
        {
            using (var context = GetDbContext())
            {
                var service = new LibraryAssetService(context, _logger);

                var asset = await service.AddAsset(new LibraryAsset
                {
                    Id = 40,
                    NumberOfCopies = 20
                });

                var actual = context.LibraryAssets.ToList();

                Assert.Equal(40, actual.Single().Id);
                Assert.Equal(1, actual.Single().StatusId);
                Assert.Equal(asset.NumberOfCopies, actual.Single().CopiesAvailable);
                Assert.Single(actual);
            }
        }

        //[Fact]
        //public void AddAsset_NullAsset_ShouldThrowAnException()
        //{
        //    var service = new LibraryAssetService();
        //    LibraryAssetForCreationDto asset = null;
        //    Assert.ThrowsAsync<NoValuesFoundException>(() => service.AddAsset(asset));
        //}

        //[Fact]
        //public void AddAsset_NullAsset_ShouldThrowAnException()
        //{
        //    var service = new LibraryAssetService();
        //    LibraryAssetForCreationDto asset = null;
        //    Assert.ThrowsAsync<NoValuesFoundException>(() => service.AddAsset(asset));
        //}

        //[Fact]
        //public async void GetAssetById_ExistingAsset_ReturnAsset()
        //{
        //    ILogger<LibraryAssetService> logger = new NullLogger<LibraryAssetService>();
        //    var asset = new LibraryAsset
        //    {
        //        Id = 1,
        //        Title = "Test Title",
        //        Year = 1992
        //    };

        //    const int id = 1;
        //    //var asset = new LibraryAss
        //    var mock = new Mock<ILibraryAssetRepository>();
        //    var mock2 = new Mock<ILibraryRepository>();

        //    mock.Setup(x => x.GetAsset(id))
        //        .ReturnsAsync(asset);

        //    var cls = new LibraryAssetService(mock2.Object, _mapper, mock.Object, logger);
        //    var actual = await cls.GetAsset(1);
        //    Assert.Equal(asset.Id, actual.Id);
        //}

        private DbContext InitAndGetDbContext()
        {
            var context = GetDbContext();

            context.Add(new LibraryAsset
            {
                Id = 1,
                Title = "",
                Year = 1992
            });
            context.Add(new LibraryAsset
            {
                Id = 2,
                Title = "",
                Year = 2016
            });

            context.SaveChanges();
            return context;
        }
    }
}