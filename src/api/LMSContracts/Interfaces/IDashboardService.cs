using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;

namespace LMSContracts.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponse> GetDashboardData();

        Task BroadcastDashboardData();
    }
}
