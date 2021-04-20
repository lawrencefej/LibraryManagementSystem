using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;

namespace LMSContracts.Interfaces
{
    public interface IReportService
    {
        Task<ChartDto> GetCheckoutsByMonthReport();

        Task<ChartDto> GetReturnsByMonthReport();

        Task<ChartDto> GetAssetsDistributionReport();

        Task<ChartDto> GetCategoryDistributionReport();

        Task<ChartDto> GetCheckoutsByDayReport();

        Task<ChartDto> GetReturnsByDayReport();

        Task<TotalsReport> GetTotalsReport();
    }
}
