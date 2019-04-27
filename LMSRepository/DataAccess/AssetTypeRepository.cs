using LMSRepository.Data;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    public class AssetTypeRepository : IAssetTypeRepository
    {
        private readonly DataContext _context;

        public AssetTypeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AssetType> Get(int assetTypeId)
        {
            var assetType = await _context.AssetTypes.FirstOrDefaultAsync();

            return assetType;
        }

        public async Task<IEnumerable<AssetType>> GetAll()
        {
            var assetTypes = await _context.AssetTypes.ToListAsync();

            return assetTypes;
        }
    }
}