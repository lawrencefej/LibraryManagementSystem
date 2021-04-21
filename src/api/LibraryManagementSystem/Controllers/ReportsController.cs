using System.Threading.Tasks;
using LMSContracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("byMonth")]
        public async Task<IActionResult> CheckoutsByMonth()
        {
            var chartData = await _reportService.GetCheckoutsByMonthReport();

            return Ok(chartData);
        }

        [HttpGet("AssetDistribution")]
        public async Task<IActionResult> GetAssetDistribution()
        {
            var chartData = await _reportService.GetAssetsDistributionReport();

            return Ok(chartData);
        }

        [HttpGet("CategoryDistribution")]
        public async Task<IActionResult> GetCategoryDistribution()
        {
            var chartData = await _reportService.GetCategoryDistributionReport();

            return Ok(chartData);
        }

        [HttpGet("byDay")]
        public async Task<IActionResult> GetCheckoutsByDay()
        {
            var chartData = await _reportService.GetCheckoutsByDayReport();

            return Ok(chartData);
        }

        [HttpGet("returns")]
        public async Task<IActionResult> GetReturnsByDay()
        {
            var chartData = await _reportService.GetReturnsByDayReport();

            return Ok(chartData);
        }

        [HttpGet("totals")]
        public async Task<IActionResult> GetTotals()
        {
            var totals = await _reportService.GetTotalsReport();

            return Ok(totals);
        }

        [HttpGet("returnsByMonth")]
        public async Task<IActionResult> ReturnsByMonth()
        {
            var chartData = await _reportService.GetReturnsByMonthReport();

            return Ok(chartData);
        }
    }
}
