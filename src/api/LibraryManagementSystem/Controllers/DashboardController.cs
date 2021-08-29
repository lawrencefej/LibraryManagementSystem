using System;
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
            List<DataDto> data = await _context.Checkouts.AsNoTracking()
               .Where(d => d.CheckoutDate > DateTime.Today.AddMonths(-12))
               .GroupBy(d => d.CheckoutDate.Month)
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Month = x.Key,
                   Date = DateTime.Today,
                   Name = GetMonthName(x.Key)
               })
               .ToListAsync();

            List<DataDto> result = ParseData(data);


            ChartDto chartData = new()
            {
                Data = result,
                Label = "checkouts"
            };

            return Ok(chartData);
        }

        [HttpGet("test2")]
        public async Task<IActionResult> GetDashboardTest2Data()
        {
            await _dashboardService.BroadcastDashboardData();

            return NoContent();
        }

        private static string GetMonthName(int month)
        {
            DateTime date = new(DateTime.Today.Year, month, 1);
            return date.ToString("MMMM");
        }

        private List<DateTime> GetDays(int days)
        {
            var startDate = DateTime.Today.AddDays(-days);

            var daysToReturn = Enumerable.Range(0, days)
                .Select(i => startDate.AddDays(i))
                .ToList();

            return daysToReturn;
        }

        private List<DataDto> ParseData(int days, List<DataDto> dataDtos)
        {
            var startDate = DateTime.Today.AddDays(-days);

            var emptyData = Enumerable.Range(1, days).Select(i =>
                new DataDto
                {
                    Count = 0,
                    Date = startDate.AddDays(i),
                    Day = startDate.AddDays(i).DayOfWeek,
                    Name = startDate.AddDays(i).ToString("ddd")
                });

            var result = dataDtos.Union(
                emptyData.Where(e => !dataDtos
                    .Select(x => x.Date).Contains(e.Date)))
                .ToList();

            return result;
        }

        private List<DataDto> ParseData(List<DataDto> dataDtos)
        {
            DateTime startDate = DateTime.Today.AddMonths(-12);

            List<DataDto> emptyData = Enumerable.Range(1, 12).Select(i =>
                new DataDto
                {
                    Count = 0,
                    // Month = DateTime.Today.AddMonths(i - 12).Month,
                    Month = startDate.AddMonths(-i).Month,
                    // Name = GetMonthName(DateTime.Today.AddMonths(i - 12).Month),
                    Name = GetMonthName(startDate.AddMonths(-i).Month),
                    // Date = DateTime.Today.AddMonths(i - 12)
                    Date = startDate.AddMonths(-i)
                }).ToList();

            List<DataDto> result = dataDtos.Union(
                emptyData.Where(e => !dataDtos
                    .Select(x => x.Month).Contains(e.Month)))
                .OrderBy(s => s.Date)
                .ToList();

            return result;

            // return emptyData;
        }
    }
}
