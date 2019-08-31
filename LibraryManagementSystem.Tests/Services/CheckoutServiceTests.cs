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
    public class CheckoutServiceTests
    {
        private readonly ILogger<CheckoutService> _logger;
        private readonly Mapper _mapper;
        private readonly TestDataContextFactory _factory;

        public CheckoutServiceTests()
        {
            _logger = new NullLogger<CheckoutService>();
            _mapper = new Mapper(new MapperConfiguration(cfg
                => cfg.AddProfile(new AutoMapperProfiles())));
            _factory = new TestDataContextFactory();
        }

        private static IEnumerable<Checkout> GetAllCheckouts()
        {
            return new List<Checkout>
            {
                new Checkout{Id = 1, LibraryAssetId = 1, LibraryCardId = 1, StatusId = 1, IsReturned = false},
                new Checkout{Id = 2, LibraryAssetId = 1, LibraryCardId = 1, StatusId = 1, IsReturned = true},
                new Checkout{Id = 3, LibraryAssetId = 1, LibraryCardId = 1, StatusId = 1, IsReturned = false}
            };
        }

        private static Checkout GetCheckout()
        {
            return new Checkout { Id = 4, LibraryAssetId = 1, LibraryCardId = 1, StatusId = 1, IsReturned = false };
        }

        private static LibraryAsset GetAsset()
        {
            return new LibraryAsset { Id = 1, Title = "Test", Year = 1992, NumberOfCopies = 10, CopiesAvailable = 10, AssetTypeId = 1, AuthorId = 1, CategoryId = 1, StatusId = 1 };
        }

        private static Author GetAuthor()
        {
            return new Author { Id = 1 };
        }

        //private static LibraryCard GetLibraryCard()
        //{
        //    return new LibraryCard { Id = 1, Fees = 0, UserId = 1 };
        //}

        [Fact]
        public async Task CheckInAsset_ValidAsset_ShouldCheckInAsset()
        {
            using (var context = _factory.UseSqlite())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkout = GetCheckout();
                var asset = GetAsset();
                context.Add(asset);
                context.Add(GetAuthor());
                context.Add(checkout);
                await context.SaveChangesAsync();
                await service.CheckInAsset(checkout.Id);
                var actual = context.Checkouts.Single();
                var actualAsset = context.LibraryAssets.Single();
                var expected = checkout;

                // Assert
                Assert.Equal(expected.StatusId, (int)EnumStatus.Returned);
                Assert.True(actual.IsReturned);
                Assert.NotNull(actual.DateReturned);
                Assert.Equal(asset.CopiesAvailable, actualAsset.CopiesAvailable);
            }
        }

        [Fact]
        public async Task CheckInAsset_InValidAsset_ShouldThrowNoValuesfoundException()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkout = GetCheckout();
                context.Add(checkout);
                await context.SaveChangesAsync();

                // Assert
                await Assert.ThrowsAsync<NoValuesFoundException>(() => service.CheckInAsset(checkout.Id));
            }
        }

        [Fact]
        public async Task CheckInAsset_ReturnedAsset_ShouldThrowLMSValidationException()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkout = GetCheckout();
                checkout.IsReturned = true;
                checkout.StatusId = (int)EnumStatus.Returned;
                var asset = GetAsset();
                context.Add(asset);
                context.Add(checkout);
                await context.SaveChangesAsync();

                // Assert
                await Assert.ThrowsAsync<LMSValidationException>(() => service.CheckInAsset(checkout.Id));
            }
        }

        [Fact]
        public async Task CheckInAsset_InValidCheckout_ShouldThrowNoValuesfoundException()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkout = GetCheckout();

                // Assert
                await Assert.ThrowsAsync<NoValuesFoundException>(() => service.CheckInAsset(checkout.Id));
            }
        }

        //[Fact]
        //public async Task CheckoutAsset_ValidCheckout_ShouldChekoutAsset()
        //{
        //    using (var context = _factory.UseInMemory())
        //    {
        //    }
        //}
    }
}