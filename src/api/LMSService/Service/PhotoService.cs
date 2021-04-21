using System;
using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using PhotoLibrary;
using PhotoLibrary.Configuration;
using PhotoLibrary.Model;

namespace LMSService.Service
{
    public class PhotoService : IPhotoService
    {
        private readonly DataContext _context;

        private readonly IPhotoLibraryService _photoLibrary;
        private readonly IPhotoConfiguration _photoConfiguration;
        private readonly IMapper _mapper;

        public PhotoService(DataContext context, IPhotoLibraryService photoLibrary,
            IPhotoConfiguration photoConfiguration, IMapper mapper)
        {
            _context = context;
            _photoLibrary = photoLibrary;
            _photoConfiguration = photoConfiguration;
            _mapper = mapper;
        }

        public async Task<ResponseHandler> AddPhotoForAsset(AssetPhotoDto assetPhotoDto)
        {
            var asset = await _context.LibraryAssets.Include(x => x.Photo).FirstOrDefaultAsync(x => x.Id == assetPhotoDto.LibraryAssetId);

            PhotoSettings settings = CloudinarySettings();

            if (asset.Photo != null)
            {
                await DeletePhoto(settings, asset.Photo);
            }

            var photoModel = new PhotoModel(assetPhotoDto.File)
            {
                Folder = "LMS/Assets",
                Width = 500,
                Height = 500,
                Crop = "fill"
            };

            var result = _photoLibrary.UploadPhoto(settings, photoModel);

            assetPhotoDto.Url = result.Url;
            assetPhotoDto.PublicId = result.PublicId;

            var photo = _mapper.Map<AssetPhoto>(assetPhotoDto);

            asset.Photo = photo;

            await _context.SaveChangesAsync();

            var photoToreturn = _mapper.Map<PhotoForReturnDto>(photo);

            return new ResponseHandler(photoToreturn, photo.Id);
        }

        public async Task<ResponseHandler> AddPhotoForUser(UserPhotoDto userPhotoDto)
        {
            var user = await _context.Users.Include(x => x.ProfilePicture).FirstOrDefaultAsync(x => x.Id == userPhotoDto.UserId);

            PhotoSettings settings = CloudinarySettings();

            if (user.ProfilePicture != null)
            {
                await DeletePhoto(settings, user.ProfilePicture);
            }

            var photoModel = new PhotoModel(userPhotoDto.File)
            {
                Folder = "LMS/Members",
                Width = 500,
                Height = 500,
                Gravity = "face",
                Crop = "fill"
            };

            var result = _photoLibrary.UploadPhoto(settings, photoModel);

            userPhotoDto.Url = result.Url;
            userPhotoDto.PublicId = result.PublicId;

            var photo = _mapper.Map<UserProfilePhoto>(userPhotoDto);

            user.ProfilePicture = photo;

            await _context.SaveChangesAsync();

            var photoToreturn = _mapper.Map<PhotoForReturnDto>(photo);
            return new ResponseHandler(photoToreturn, photo.Id);
        }

        private async Task DeletePhoto(PhotoSettings settings, Photo photo)
        {
            if (_photoLibrary.DeletePhoto(settings, photo.PublicId))
            {
                _context.Remove(photo);
                await _context.SaveChangesAsync();
                return;
            }

            throw new Exception($"Cloud delete failed, please try again later");
        }

        private PhotoSettings CloudinarySettings()
        {
            return new PhotoSettings
            {
                Name = _photoConfiguration.Name,
                ApiSecret = _photoConfiguration.ApiSecret,
                ApiKey = _photoConfiguration.ApiKey
            };
        }
    }
}
