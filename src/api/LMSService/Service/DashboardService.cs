using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LMSService.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly DataContext _context;

        public DashboardService(DataContext context)
        {
            _context = context;
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
                TypeDistribution = await GetTypeDistribution()
            };

            return dashboardData;
        }

        // private async Task<ChartDto> GetCategoryDistributionData()
        // {
        //     List<DataDto> data = await _context.LibraryAssets.AsNoTracking()
        //         .GroupBy(c => c.AssetCategories)
        //         .Select(d => new DataDto
        //         {
        //             Count = d.Count(),
        //             Name = d.Key
        //         }).ToListAsync();

        //     return new ChartDto
        //     {
        //         Data = data,
        //         Label = "CategoryDistribution"
        //     };
        // }

        // private async Task<TotalsReport> GetTotals()
        // {
        //     return new TotalsReport
        //     {
        //         TotalAuthors = await _context.Authors.AsNoTracking().CountAsync(),
        //         TotalCheckouts = await _context.Checkouts.AsNoTracking().CountAsync(),
        //         TotalItems = await _context.LibraryAssets.AsNoTracking().CountAsync(),
        //         TotalMembers = await _context.LibraryCards.AsNoTracking().CountAsync()
        //     };
        // }

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

            // ChartDto chartData = new()
            // {
            //     Data = data,
            //     Label = "Item Type Distribution"
            // };

            return new ChartDto
            {
                Data = data,
                Label = "Item Type Distribution"
            };
        }
    }
}
