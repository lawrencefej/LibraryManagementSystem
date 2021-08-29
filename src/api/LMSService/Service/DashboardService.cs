using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSRepository.Data;
using LMSService.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LMSService.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly DataContext _context;
        private readonly IHubContext<DashboardHub, IDashboardHub> _hub;

        public DashboardService(DataContext context, IHubContext<DashboardHub, IDashboardHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<DashboardResponse> GetDashboardData()
        {
            DashboardResponse dashboardData = new()
            {
                TotalAuthors = await _context.Authors.AsNoTracking().CountAsync(),
                TotalCards = await _context.LibraryCards.AsNoTracking().CountAsync(),
                TotalCheckouts = await _context.Checkouts.AsNoTracking().CountAsync(),
                TotalItems = await _context.LibraryAssets.AsNoTracking().CountAsync(),
                CategoryDistribution = await GetCategoryDistributionData(),
                TypeDistribution = await GetTypeDistribution(),
                ReturnsByMonth = await GetReturnsByMonthReportData(),
                ReturnsByDay = await GetReturnsByDayReport(),
                CheckoutsByDay = await GetCheckoutsByDayReport(),
                CheckoutsByMonth = await GetCheckoutsByMonthReport(),
            };

            return dashboardData;
        }

        public async Task BroadcastDashboardData()
        {
            await _hub.Clients.All.BroadcastChartData(await GetDashboardData());
        }

        private async Task<ChartDto> GetCategoryDistributionData()
        {
            List<DataDto> data = await _context.LibraryAssetCategories
                .AsNoTracking()
                .Include(s => s.Category)
                .GroupBy(t => t.Category.Name)
                .Select(d => new DataDto
                {
                    Count = d.Count(),
                    Name = d.Key.ToString()
                }).ToListAsync();

            return new ChartDto
            {
                Data = data,
                Label = "CategoryDistribution"
            };
        }

        private async Task<ChartDto> GetTypeDistribution()
        {
            List<DataDto> data = await _context.LibraryAssets
                .AsNoTracking()
                .GroupBy(t => t.AssetType)
                .Select(d => new DataDto
                {
                    Count = d.Count(),
                    Name = d.Key.ToString()
                })
                .ToListAsync();

            return new ChartDto
            {
                Data = data,
                Label = "Item Type Distribution"
            };
        }

        private async Task<ChartDto> GetCheckoutsByDayReport()
        {
            List<DataDto> data = await _context.Checkouts.AsNoTracking()
               .Where(d => d.CheckoutDate > DateTime.Today.AddDays(-7))
               .GroupBy(d => d.CheckoutDate.Date)
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Date = x.Key.Date,
                   Day = x.Key.Date.DayOfWeek,
                   Name = x.Key.Date.ToString("ddd")
               })
               .ToListAsync();

            List<DateTime> days = GetDays(30);

            data = ParseData(7, data);

            return new ChartDto()
            {
                Data = data,
                Label = "Checkouts"
            };
        }

        private async Task<ChartDto> GetCheckoutsByMonthReport()
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

            return new ChartDto()
            {
                Data = result,
                Label = "checkouts"
            };
        }

        private async Task<ChartDto> GetReturnsByMonthReportData()
        {
            List<DataDto> data = await _context.Checkouts.AsNoTracking()
               .Where(d => d.DateReturned > DateTime.Today.AddMonths(-12))
               .GroupBy(d => d.DateReturned.Date.Month)
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Month = x.Key,
                   Date = DateTime.Today,
                   Name = GetMonthName(x.Key)
               })
               .ToListAsync();

            List<DataDto> result = ParseData(data);

            return new ChartDto()
            {
                Data = result,
                Label = "Returns"
            };
        }

        private async Task<ChartDto> GetReturnsByDayReport()
        {
            List<DataDto> data = await _context.Checkouts.AsNoTracking()
                .Where(d => d.DateReturned > DateTime.Today.AddDays(-7))
               .GroupBy(d => d.DateReturned.Date)
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Date = x.Key.Date,
                   Day = x.Key.Date.DayOfWeek,
                   Name = x.Key.Date.ToString("ddd")
               })
               .ToListAsync();

            List<DateTime> days = GetDays(30);
            data = ParseData(7, data);

            return new ChartDto()
            {
                Data = data,
                Label = "Returns"
            };
        }

        private static string GetMonthName(int month)
        {
            DateTime date = new(DateTime.Today.Year, month, 1);
            return date.ToString("MMMM");
        }

        private static List<DateTime> GetDays(int days)
        {
            DateTime startDate = DateTime.Today.AddDays(-days);

            List<DateTime> daysToReturn = Enumerable.Range(0, days)
                .Select(i => startDate.AddDays(i))
                .ToList();

            return daysToReturn;
        }

        private static List<DataDto> ParseData(int days, List<DataDto> dataDtos)
        {
            DateTime startDate = DateTime.Today.AddDays(-days);

            IEnumerable<DataDto> emptyData = Enumerable.Range(1, days).Select(i =>
                new DataDto
                {
                    Count = 0,
                    Date = startDate.AddDays(i),
                    Day = startDate.AddDays(i).DayOfWeek,
                    Name = startDate.AddDays(i).ToString("ddd")
                });

            List<DataDto> result = dataDtos.Union(
                emptyData.Where(e => !dataDtos
                    .Select(x => x.Date).Contains(e.Date)))
                    .OrderBy(s => s.Date)
                .ToList();

            return result;
        }

        private static List<DataDto> ParseData(List<DataDto> dataDtos)
        {
            DateTime startDate = DateTime.Today.AddMonths(-12);

            List<DataDto> emptyData = Enumerable.Range(1, 12).Select(i =>
                new DataDto
                {
                    Count = 0,
                    Month = DateTime.Today.AddMonths(i - 12).Month,
                    Name = GetMonthName(DateTime.Today.AddMonths(i - 12).Month),
                    Date = DateTime.Today.AddMonths(i - 12)
                }).ToList();

            List<DataDto> result = dataDtos.Union(
                emptyData.Where(e => !dataDtos
                    .Select(x => x.Month).Contains(e.Month)))
                    .OrderBy(s => s.Date)
                .ToList();

            return result;
        }
    }
}
