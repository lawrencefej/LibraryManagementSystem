using LMSRepository.Models;
using LMSService.Service;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManagementSystem.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly TestDataContextFactory _factory;

        public CategoryServiceTests()
        {
            _factory = new TestDataContextFactory();
        }

        private static Category GetCategory()
        {
            return new Category { Id = 4, Name = "Test4" };
        }

        [Fact]
        public async Task AddCategory_ValidCategory_ShouldAddNewCategory()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CategoryService(context);
                var category = GetCategory();

                // Act
                var actual = await service.AddCategory(category);
                var expected = category;

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Name, actual.Name);
            }
        }

        [Fact]
        public async Task DeleteCategory_ExistingCategory_ShouldDeleteCategory()
        {
            using (var context = _factory.UseInMemory())
            {
                // Arrange
                var service = new CategoryService(context);
                var category = GetCategory();
                context.Add(category);
                context.SaveChanges();
                category = context.Category.FirstOrDefault(x => x.Id == category.Id);

                // Act
                await service.DeleteCategory(category);
                var expected = context.Category.FirstOrDefault(x => x.Id == category.Id);

                // Assert
                Assert.Null(expected);
            }
        }

        [Fact]
        public async void GetCategory_ListCategory_ReturnsCategoryList()
        {
            using (var context = _factory.UseInMemory())
            {
                //Act
                var service = new CategoryService(context);
                var actual = await service.GetCategories();
                var expected = context.Category.Count();

                //Assert
                Assert.Equal(actual.Count(), expected);
            }
        }

        [Fact]
        public async void GetCategoryById_ExistingCategory_ReturnCategory()
        {
            using (var context = _factory.UseInMemory())
            {
                //Arrange
                var category = GetCategory();
                context.Add(category);
                context.SaveChanges();

                //Act
                var service = new CategoryService(context);
                var actual = await service.GetCategory(category.Id);
                var expected = category;

                //Assert
                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Name, actual.Name);
            }
        }
    }
}