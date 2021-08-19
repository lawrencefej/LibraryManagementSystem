using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LMSContracts.Interfaces;
using LMSEntities.Configuration;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LMSService.Service
{
    public class PhotoService : IPhotoService
    {
        private readonly DataContext _context;
        private readonly Cloudinary _cloudinary;

        public PhotoService(DataContext context, IOptions<CloudinarySettings> cloudinarySettings)
        {
            Account acc = new(cloudinarySettings.Value.Name,
                              cloudinarySettings.Value.ApiKey,
                              cloudinarySettings.Value.ApiSecret);
            _context = context;
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<LmsResponseHandler<PhotoResponseDto>> AddPhotoForCard(IFormFile file, int cardId)
        {
            if (file.Length > 0)
            {
                LibraryCard card = await _context.LibraryCards.Include(x => x.LibraryCardPhoto)
                .FirstOrDefaultAsync(x => x.Id == cardId);

                if (card.LibraryCardPhoto != null)
                {
                    await DeletePhoto(card.LibraryCardPhoto.PublicId);
                }

                using Stream stream = file.OpenReadStream();

                ImageUploadParams uploadParams = new()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500)
                    .Width(500)
                    .Crop("fill")
                    .Gravity("face"),
                    Folder = "LMS/LibraryCards"
                };

                ImageUploadResult result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error != null)
                {
                    // TODO log error and return generic message
                    return LmsResponseHandler<PhotoResponseDto>.Failed(result.Error.Message);
                }

                LibraryCardPhoto photo = new()
                {
                    PublicId = result.PublicId,
                    Url = result.SecureUrl.AbsoluteUri
                };

                card.LibraryCardPhoto = photo;

                await _context.SaveChangesAsync();

                return LmsResponseHandler<PhotoResponseDto>.Successful(new PhotoResponseDto { Id = photo.Id, Url = photo.Url });
            }

            return LmsResponseHandler<PhotoResponseDto>.Failed("");
        }

        public async Task<LmsResponseHandler<PhotoResponseDto>> AddPhotoForUser(IFormFile file, int userId)
        {
            if (file.Length > 0)
            {
                AppUser user = await _context.Users.Include(x => x.ProfilePicture)
                .FirstOrDefaultAsync(x => x.Id == userId);

                if (user.ProfilePicture != null)
                {
                    await DeletePhoto(user.ProfilePicture.PublicId);
                }

                using Stream stream = file.OpenReadStream();

                ImageUploadParams uploadParams = new()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500)
                    .Width(500)
                    .Crop("fill")
                    .Gravity("face"),
                    Folder = "LMS/Users"
                };

                ImageUploadResult result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error != null)
                {
                    // TODO log error and return generic message
                    return LmsResponseHandler<PhotoResponseDto>.Failed(result.Error.Message);
                }

                UserProfilePhoto photo = new()
                {
                    PublicId = result.PublicId,
                    Url = result.SecureUrl.AbsoluteUri
                };

                user.ProfilePicture = photo;

                await _context.SaveChangesAsync();

                return LmsResponseHandler<PhotoResponseDto>.Successful(new PhotoResponseDto { Id = photo.Id, Url = photo.Url });
            }

            return LmsResponseHandler<PhotoResponseDto>.Failed("");
        }

        public async Task<LmsResponseHandler<PhotoResponseDto>> AddPhotoForAsset(IFormFile file, int assetId)
        {
            if (file.Length > 0)
            {
                LibraryAsset asset = await _context.LibraryAssets.Include(x => x.Photo)
                .FirstOrDefaultAsync(x => x.Id == assetId);

                if (asset.Photo != null)
                {
                    await DeletePhoto(asset.Photo.PublicId);
                }

                using Stream stream = file.OpenReadStream();

                ImageUploadParams uploadParams = new()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500)
                    .Width(500)
                    .Crop("fill"),
                    Folder = "LMS/Assets"
                };

                ImageUploadResult result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error != null)
                {
                    // TODO log error and return generic message
                    return LmsResponseHandler<PhotoResponseDto>.Failed(result.Error.Message);
                }

                AssetPhoto photo = new()
                {
                    PublicId = result.PublicId,
                    Url = result.SecureUrl.AbsoluteUri
                };

                asset.Photo = photo;

                await _context.SaveChangesAsync();

                return LmsResponseHandler<PhotoResponseDto>.Successful(new PhotoResponseDto { Id = photo.Id, Url = photo.Url });
            }

            return LmsResponseHandler<PhotoResponseDto>.Failed("");
        }

        private async Task<DeletionResult> DeletePhoto(string publicId)
        {
            DeletionParams deleteParams = new(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
