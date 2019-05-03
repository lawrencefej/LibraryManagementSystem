using LMSRepository.Dto;
using LMSService.Helpers;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IPhotoService
    {
        Task<ResponseHandler> AddPhotoForUser(UserPhotoDto userPhotoDto);

        Task<ResponseHandler> AddPhotoForAsset(AssetPhotoDto assetPhotoDto);
    }
}