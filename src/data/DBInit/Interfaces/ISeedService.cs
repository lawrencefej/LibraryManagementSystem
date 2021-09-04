using System.Threading.Tasks;

namespace DBInit.Interfaces
{
    public interface ISeedService
    {
        Task SeedLibraryCard();

        Task SeedUsers();

        Task SeedAuthors();

        Task SeedPastCheckout();

        Task SeedCurrentCheckout();

        Task SeedBooksAsset();

        Task SeedMediaAsset();

        Task SeedOtherAsset();

        Task SeedAssets();
    }
}
