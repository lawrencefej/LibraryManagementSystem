using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Http;

namespace LMSContracts.Interfaces
{
    public interface IPhotoService
    {
        Task<LmsResponseHandler<PhotoResponseDto>> AddPhotoForCard(IFormFile file, int cardId);

        Task<LmsResponseHandler<PhotoResponseDto>> AddPhotoForAsset(IFormFile file, int assetId);

        Task<LmsResponseHandler<PhotoResponseDto>> AddPhotoForUser(IFormFile file, int userId);
    }
}
