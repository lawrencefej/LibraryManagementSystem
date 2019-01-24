using LMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LMSLibrary.Services
{
    public interface IReserveService
    {
        Task<IEnumerable<ReserveAsset>> GetAllReservedAssets();
        Task<ReserveAsset> GetReserveAsset(int id);
        Task<ReserveAsset> CreateReserveAsset(int userId, int assetId);
        Task<ReserveAsset> CancelReserveAsset(ReserveAsset reserve);
        Task<ReserveAsset> ExpireReserveAsset(ReserveAsset reserve);

    }
}
