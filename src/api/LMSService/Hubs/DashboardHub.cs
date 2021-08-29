using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LMSService.Hubs
{
    [Authorize]
    public class DashboardHub : Hub<IDashboardHub>
    {
        public async Task BroadcastChartData(DashboardResponse dashboardResponse)
        {
            await Clients.All.BroadcastChartData(dashboardResponse);
        }
    }
}
