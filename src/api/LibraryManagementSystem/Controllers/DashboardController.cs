using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSRepository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IDashboardService _dashboardService;

        public DashboardController(DataContext context, IDashboardService dashboardService)
        {
            _context = context;
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(DashboardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDashboardData()
        {

            DashboardResponse data = await _dashboardService.GetDashboardData();

            return Ok(data);
        }

        [HttpGet("test")]
        public async Task<IActionResult> GetDashboardTestData()
        {

            List<DataDto> data = await _context.LibraryAssetCategories
                .Include(s => s.Category)
                .GroupBy(t => t.Category.Name)
                .Select(d => new DataDto
                {
                    Count = d.Count(),
                    Name = d.Key.ToString()
                }).ToListAsync();


            ChartDto chartData = new()
            {
                Data = data,
                Label = "AssetDistribution"
            };

            return Ok(chartData);
        }
    }
}
