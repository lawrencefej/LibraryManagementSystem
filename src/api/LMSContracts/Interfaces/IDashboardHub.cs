using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;

namespace LMSContracts.Interfaces
{
    public interface IDashboardHub
    {
        Task BroadcastChartData(DashboardResponse dashboardResponse);
    }
}
