using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LMSService.Service
{
    public class ReportService : IReportService
    {
        private readonly DataContext _context;

        public ReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<ChartDto> GetAssetsDistributionReport()
        {
            var data = await _context.LibraryAssets.AsNoTracking()
               .GroupBy(d => new { d.AssetType.Name })
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Name = x.Key.Name
               })
               .ToListAsync();

            var chartData = new ChartDto()
            {
                Data = data,
                Label = "AssetDistribution"
            };

            return chartData;
        }

        public async Task<ChartDto> GetCategoryDistributionReport()
        {
            var data = await _context.LibraryAssets.AsNoTracking()
               .GroupBy(d => new { d.Category.Name })
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Name = x.Key.Name
               })
               .ToListAsync();

            var chartData = new ChartDto()
            {
                Data = data,
                Label = "CategoryDistribution"
            };

            return chartData;
        }

        public async Task<ChartDto> GetCheckoutsByDayReport()
        {
            var data = await _context.Checkouts.AsNoTracking()
               .Where(d => d.Since > DateTime.Today.AddDays(-7))
               .GroupBy(d => new { Date = d.Since })
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Date = x.Key.Date,
                   Day = x.Key.Date.DayOfWeek,
                   Name = x.Key.Date.ToString("ddd")
               })
               .ToListAsync();

            var days = GetDays(30);

            data = ParseData(7, data);

            var chartData = new ChartDto()
            {
                Data = data,
                Label = "Checkouts"
            };

            return chartData;
        }

        public async Task<ChartDto> GetCheckoutsByMonthReport()
        {
            var data = await _context.Checkouts.AsNoTracking()
               .Where(d => d.Since > DateTime.Today.AddMonths(-12))
               .GroupBy(d => new { d.Since.Month })
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Month = x.Key.Month,
                   Name = GetMonthName(x.Key.Month)
               })
               .ToListAsync();

            var result = ParseData(data);

            var chartData = new ChartDto()
            {
                Data = result,
                Label = "checkouts"
            };

            return chartData;
        }

        public async Task<ChartDto> GetReturnsByDayReport()
        {
            var data = await _context.Checkouts.AsNoTracking()
                .Where(d => d.DateReturned > DateTime.Today.AddDays(-7))
               .GroupBy(d => new { Date = d.DateReturned })
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Date = x.Key.Date,
                   Day = x.Key.Date.Value.DayOfWeek,
                   Name = x.Key.Date.Value.ToString("ddd")
               })
               .ToListAsync();

            var days = GetDays(30);
            data = ParseData(7, data);

            var chartData = new ChartDto()
            {
                Data = data,
                Label = "Returns"
            };

            return chartData;
        }

        public async Task<ChartDto> GetReturnsByMonthReport()
        {
            var data = await _context.Checkouts.AsNoTracking()
               .Where(d => d.DateReturned > DateTime.Today.AddMonths(-12))
               .GroupBy(d => new { d.DateReturned })
               .Select(x => new DataDto
               {
                   Count = x.Count(),
                   Month = x.Key.DateReturned.Value.Month,
                   Name = GetMonthName(x.Key.DateReturned.Value.Month)
               })
               .ToListAsync();

            var result = ParseData(data);

            var chartData = new ChartDto()
            {
                Data = result,
                Label = "Returns"
            };

            return chartData;
        }

        public async Task<TotalsReport> GetTotalsReport()
        {
            var Totals = new TotalsReport
            {
                TotalAuthors = await _context.Authors.AsNoTracking().CountAsync(),
                TotalCheckouts = await _context.Checkouts.AsNoTracking().CountAsync(),
                TotalItems = await _context.LibraryAssets.AsNoTracking().CountAsync(),
                TotalMembers = await _context.LibraryCards.AsNoTracking().CountAsync()
            };

            return Totals;
        }

        private static string GetMonthName(int month)
        {
            DateTime date = new DateTime(DateTime.Today.Year, month, 1);
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
            var startDate = DateTime.Today.AddMonths(-12);

            var emptyData = Enumerable.Range(1, 12).Select(i =>
                new DataDto
                {
                    Count = 0,
                    Month = DateTime.Today.AddMonths(i - 12).Month,
                    Name = GetMonthName(DateTime.Today.AddMonths(i - 12).Month),
                    Date = DateTime.Today.AddMonths(i - 12)
                }).ToList();

            var result = dataDtos.Union(
                emptyData.Where(e => !dataDtos
                    .Select(x => x.Month).Contains(e.Month)))
                .ToList();

            return result;
        }
    }
}
