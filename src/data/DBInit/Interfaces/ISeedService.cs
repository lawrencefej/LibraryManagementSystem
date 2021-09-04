using System.Threading.Tasks;

namespace DBInit.Interfaces
{
    public interface ISeedService
    {
        Task SeedDatabase();
    }
}
