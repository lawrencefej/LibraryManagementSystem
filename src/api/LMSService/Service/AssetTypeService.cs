using System.Collections.Generic;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LMSService.Service
{
    // public class AssetTypeService : IAssetTypeService
    // {
    //     private readonly DataContext _context;

    //     public AssetTypeService(DataContext context)
    //     {
    //         _context = context;
    //     }

    //     public async Task<AssetType> AddAssetType(AssetType assetType)
    //     {
    //         _context.Add(assetType);
    //         await _context.SaveChangesAsync();

    //         return assetType;
    //     }

    //     public async Task DeleteAssetType(AssetType assetType)
    //     {
    //         _context.Remove(assetType);
    //         await _context.SaveChangesAsync();
    //     }

    //     public async Task<AssetType> GetAssetType(int assetTypeId)
    //     {
    //         var assetType = await _context.AssetTypes.FirstOrDefaultAsync(x => x.Id == assetTypeId);

    //         return assetType;
    //     }

    //     public async Task<IEnumerable<AssetType>> GetAssetTypes()
    //     {
    //         var assetTypes = await _context.AssetTypes.AsNoTracking().ToListAsync();

    //         return assetTypes;
    //     }
    // }
}
