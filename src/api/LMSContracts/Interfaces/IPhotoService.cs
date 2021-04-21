using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;

namespace LMSContracts.Interfaces
{
    public interface IPhotoService
    {
        Task<ResponseHandler> AddPhotoForUser(UserPhotoDto userPhotoDto);

        Task<ResponseHandler> AddPhotoForAsset(AssetPhotoDto assetPhotoDto);
    }
}
