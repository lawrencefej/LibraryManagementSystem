using LMSRepository.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryManagementSystem.Tests.ControllerTests
{
    public class CatalogTests : TestBase
    {
        public void GetLibraryAsset_ExistingId_ReturnsAsset()
        {
            //// Arrange
            //var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            //// Act
            //var okResult = _controller.Get(testGuid).Result as OkObjectResult;

            //// Assert
            //Assert.IsType<ShoppingItem>(okResult.Value);
            //Assert.Equal(testGuid, (okResult.Value as ShoppingItem).Id);

            using (var context = InitAndGetDbContext())
            {
                // Arrange
                //var command = new CatalogController();

                //// Act
                //int reportTo = 2;
                //command.Execute(new EmployeeUnderManagerModel
                //{
                //    EmployeeId = 1,
                //    ManagerId = reportTo
                //});

                //// Assert
                //Assert.Equal(1, );
            }
        }

        private DbContext InitAndGetDbContext()
        {
            var context = GetDbContext();

            context.Add(new LibraryAsset
            {
                Id = 1,
                Title = "",
                //LastName = ""
            });
            context.Add(new LibraryAsset
            {
                Id = 2,
                Title = "",
                //LastName = ""
            });

            context.SaveChanges();
            return context;
        }
    }
}