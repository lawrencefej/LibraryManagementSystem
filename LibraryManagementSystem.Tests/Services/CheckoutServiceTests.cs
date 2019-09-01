using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Dto;
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
                new Checkout{Id = 1, LibraryAssetId = 1, LibraryCardId = 1, StatusId = (int)EnumStatus.Checkedout, IsReturned = false},
                new Checkout{Id = 2, LibraryAssetId = 1, LibraryCardId = 1, StatusId = (int)EnumStatus.Returned, IsReturned = true},
                new Checkout{Id = 3, LibraryAssetId = 2, LibraryCardId = 2, StatusId = (int)EnumStatus.Checkedout, IsReturned = false}
            };
        }

        private static Checkout GetCheckout()
        {
            return new Checkout { Id = 4, LibraryAssetId = 1, LibraryCardId = 1, StatusId = (int)EnumStatus.Checkedout, IsReturned = false };
        }

        private static LibraryAsset GetAsset()
        {
            return new LibraryAsset { Id = 1, Title = "Test", Year = 1992, NumberOfCopies = 10, CopiesAvailable = 10, AssetTypeId = 1, AuthorId = 1, CategoryId = 1, StatusId = 1 };
        }

        private static Author GetAuthor()
        {
            return new Author { Id = 1 };
        }

        private static LibraryCard GetLibraryCard()
        {
            return new LibraryCard { Id = 1, Fees = 0, UserId = 1 };
        }

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
        public async Task CheckInAsset_UnavailableAsset_ShouldMakeAssetAvailable()
        {
            using (var context = _factory.UseSqlite())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkout = GetCheckout();
                var asset = GetAsset();
                asset.CopiesAvailable = 0;
                asset.StatusId = (int)EnumStatus.Unavailable;
                context.Add(asset);
                context.Add(GetAuthor());
                context.Add(checkout);
                await context.SaveChangesAsync();
                await service.CheckInAsset(checkout.Id);
                var actual = context.Checkouts.Single();
                var actualAsset = context.LibraryAssets.Single();
                var expected = checkout;

                // Assert
                Assert.Equal(actualAsset.StatusId, (int)EnumStatus.Available);
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

        [Fact]
        public async Task CheckoutAsset_ValidCheckout_ShouldCheckoutAsset()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var asset = GetAsset();
                var librarycard = GetLibraryCard();
                context.Add(asset);
                context.Add(GetAuthor());
                context.Add(librarycard);
                await context.SaveChangesAsync();

                var checkoutForCreation = new CheckoutForCreationDto { LibraryAssetId = 1, userId = 1 };
                var actual = await service.CheckoutAsset(checkoutForCreation);
                var actualAsset = context.LibraryAssets.Single();

                // Assert
                Assert.Equal(nameof(EnumStatus.Checkedout), actual.Status);
                Assert.Equal(GetAsset().CopiesAvailable - 1, actualAsset.CopiesAvailable);
            }
        }

        [Fact]
        public async Task CheckoutAsset_LastAvailableAsset_ShouldMakeAssetUnavailabe()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var asset = GetAsset();
                asset.CopiesAvailable = 1;
                var librarycard = GetLibraryCard();
                context.Add(asset);
                context.Add(GetAuthor());
                context.Add(librarycard);
                await context.SaveChangesAsync();

                var checkoutForCreation = new CheckoutForCreationDto { LibraryAssetId = 1, userId = 1 };
                var actual = await service.CheckoutAsset(checkoutForCreation);
                var actualAsset = context.LibraryAssets.Single();

                // Assert
                Assert.Equal((int)EnumStatus.Unavailable, actualAsset.StatusId);
            }
        }

        [Fact]
        public async Task CheckoutAsset_InValidCard_ShouldThrowsNoValuesFoundException()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkoutForCreation = new CheckoutForCreationDto { LibraryAssetId = 1, userId = 1 };

                // Assert
                await Assert.ThrowsAsync<NoValuesFoundException>(() => service.CheckoutAsset(checkoutForCreation));
            }
        }

        [Fact]
        public async Task CheckoutAsset_CardWithFees_ShouldThrowsLMSValidationException()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var librarycard = GetLibraryCard();
                librarycard.Fees = 20;
                context.Add(librarycard);
                await context.SaveChangesAsync();
                var checkoutForCreation = new CheckoutForCreationDto { LibraryAssetId = 1, userId = 1 };

                // Assert
                await Assert.ThrowsAsync<LMSValidationException>(() => service.CheckoutAsset(checkoutForCreation));
            }
        }

        [Fact]
        public async Task CheckoutAsset_CheckedOutAsset_ShouldThrowsNoValuesFoundException()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkout = GetCheckout();
                var asset = GetAsset();
                var librarycard = GetLibraryCard();
                context.Add(asset);
                context.Add(GetAuthor());
                context.Add(librarycard);
                context.Add(checkout);
                await context.SaveChangesAsync();
                var checkoutForCreation = new CheckoutForCreationDto { LibraryAssetId = 1, userId = 1 };

                // Assert
                await Assert.ThrowsAsync<LMSValidationException>(() => service.CheckoutAsset(checkoutForCreation));
            }
        }

        [Fact]
        public async Task GetCheckoutsForAsset_ValidList_ShouldReturnValidList()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkouts = GetAllCheckouts();
                var asset = GetAsset();
                var librarycard = GetLibraryCard();
                context.Add(asset);
                context.AddRange(checkouts);
                context.Add(librarycard);
                await context.SaveChangesAsync();
                var actual = await service.GetCheckoutsForAsset(asset.Id);
                var expected = checkouts;

                // Assert
                Assert.Single(actual);
            }
        }

        [Fact]
        public async Task GetCheckoutsForMember_ValidList_ShouldReturnValidList()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkouts = GetAllCheckouts();
                var asset = GetAsset();
                var librarycard = GetLibraryCard();
                context.Add(asset);
                context.AddRange(checkouts);
                context.Add(librarycard);
                await context.SaveChangesAsync();
                var actual = await service.GetCheckoutsForMember(librarycard.UserId);
                var expected = checkouts;

                // Assert
                Assert.Equal(2, actual.Count());
            }
        }

        [Fact]
        public async Task GetAllCurrentCheckouts_ValidList_ShouldReturnOnlycheckoutList()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CheckoutService(context, _mapper, _logger);

                // Act
                var checkouts = GetAllCheckouts();
                var asset = GetAsset();
                context.Add(asset);
                context.Add(new LibraryAsset { Id = 2 });
                context.AddRange(checkouts);
                await context.SaveChangesAsync();
                var paginationParams = new PaginationParams();
                var actual = await service.GetAllCurrentCheckouts(paginationParams);
                var expected = checkouts;

                // Assert
                Assert.Equal(2, actual.Count());
            }
        }
    }
}