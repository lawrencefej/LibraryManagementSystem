using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LMSLibrary.Models;

namespace LMSLibrary.Services
{
    /// <summary>
    /// Logic to reserve an item
    /// </summary>
    public class ReserveService : IReserveService
    {
        public ReserveService()
        {

        }
        public Task<ReserveAsset> CancelReserveAsset(ReserveAsset reserve)
        {
            throw new NotImplementedException();
        }

        public Task<ReserveAsset> CreateReserveAsset(int userId, int assetId)
        {
            throw new NotImplementedException();
        }

        public Task<ReserveAsset> ExpireReserveAsset(ReserveAsset reserve)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReserveAsset>> GetAllReservedAssets()
        {
            throw new NotImplementedException();
        }

        public Task<ReserveAsset> GetReserveAsset(int id)
        {
            throw new NotImplementedException();
        }
    }
}
