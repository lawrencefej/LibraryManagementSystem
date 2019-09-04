using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManagementSystem.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly TestDataContextFactory _factory;

        public AuthorServiceTests()
        {
            _factory = new TestDataContextFactory();
        }

        private static IEnumerable<Author> GetAllAuthors()
        {
            return new List<Author>
            {
                new Author
                {
                    Id = 1,
                    FirstName = "Test1",
                    LastName = "name1"
                },
                new Author{Id = 2, FirstName = "Test2", LastName = "name2"},
                new Author{Id = 3, FirstName = "Test3", LastName = "name3"}
            };
        }

        private static Author GetAuthor()
        {
            return new Author { Id = 4, FirstName = "Lebron", LastName = "James" };
        }

        [Fact]
        public async Task AddAuthor_ValidAuthor_ShouldAddNewAuthor()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new AuthorService(context);

                // Act
                var author = GetAuthor();
                var expected = await service.AddAuthor(author);
                var actual = author;

                // Assert
                Assert.Equal(expected.FirstName, actual.FirstName);
                Assert.Equal(expected.LastName, actual.LastName);
                Assert.Equal(expected.Id, actual.Id);
            }
        }

        [Fact]
        public async Task AddAuthor_ExistingAuthor_ShouldDeleteAuthor()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var author = GetAuthor();
                var service = new AuthorService(context);
                context.Add(author);
                context.SaveChanges();

                // Act
                await service.DeleteAuthor(author);

                // Assert
                Assert.Equal(0, context.Authors.Count());
            }
        }

        [Fact]
        public async Task EditAuthor_ValidAuthor_ShouldEditAuthor()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new AuthorService(context);
                var author = GetAuthor();
                context.Add(author);
                context.SaveChanges();
                author.FirstName = "testedit";

                // Act
                await service.EditAuthor(author);
                var actual = context.Authors.Single();
                var expected = author;

                // Assert
                Assert.Equal(expected.FirstName, actual.FirstName);
            }
        }

        [Fact]
        public async void GetPaginatedAuthors_PaginatedList_ReturnsPaginatedList()
        {
            using (var context = _factory.UseSqlite())
            {
                //Arrange
                context.AddRange(GetAllAuthors());
                context.SaveChanges();

                //Act
                var service = new AuthorService(context);
                var paginationParams = new PaginationParams();
                var actual = await service.GetPaginatedAuthors(paginationParams);
                var authors = GetAllAuthors().ToList();

                //Assert
                Assert.Equal(actual.Count, authors.Count);
            }
        }

        [Fact]
        public async void GetAuthor_ExistingAsset_ReturnAsset()
        {
            using (var context = _factory.UseInMemory())
            {
                //Arrange
                var author = GetAuthor();
                context.Add(author);
                context.SaveChanges();

                // Act
                var service = new AuthorService(context);
                var actual = await service.GetAuthor(author.Id);
                var expected = author;

                //Assert
                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.FirstName, actual.FirstName);
                Assert.Equal(expected.LastName, actual.LastName);
            }
        }

        [Fact]
        public async void GetAssetsByAuthor_ValidAuthor_ReturnsAllAssetsByAuthor()
        {
            using (var context = _factory.UseSqlite())
            {
                //Arrange
                context.AddRange(GetAllAuthors());
                context.Add(GetAuthor());
                context.SaveChanges();

                //Act
                var service = new AuthorService(context);
                var actual = await service.SearchAuthors("Test");

                //Assert
                Assert.Equal(3, actual.ToList().Count);
            }
        }
    }
}