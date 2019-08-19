using AutoMapper;
using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using LMSService.Helpers;
using LMSService.Interfaces;
using PhotoLibrary;
using PhotoLibrary.Configuration;
using PhotoLibrary.Model;
using System;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class PhotoService : IPhotoService
    {
        private readonly DataContext _context;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IPhotoLibraryService _photoLibrary;
        private readonly IUserRepository _userRepository;
        private readonly ILibraryAssetRepository _libraryAssetRepository;
        private readonly IPhotoConfiguration _photoConfiguration;
        private readonly IMapper _mapper;

        public PhotoService(DataContext context ,ILibraryRepository libraryRepository, IPhotoLibraryService photoLibrary,
            IUserRepository userRepository, ILibraryAssetRepository libraryAssetRepository,
            IPhotoConfiguration photoConfiguration, IMapper mapper)
        {
            _context = context;
            _libraryRepository = libraryRepository;
            _photoLibrary = photoLibrary;
            _userRepository = userRepository;
            _libraryAssetRepository = libraryAssetRepository;
            _photoConfiguration = photoConfiguration;
            _mapper = mapper;
        }

        public async Task<ResponseHandler> AddPhotoForAsset(AssetPhotoDto assetPhotoDto)
        {
            var asset = await _libraryAssetRepository.GetAsset(assetPhotoDto.LibraryAssetId);

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

            if (await _libraryRepository.SaveAll())
            {
                var photoToreturn = _mapper.Map<PhotoForReturnDto>(photo);

                return new ResponseHandler(photoToreturn, photo.Id);
            }

            throw new Exception($"Adding Photo failed on save");
        }

        public async Task<ResponseHandler> AddPhotoForUser(UserPhotoDto userPhotoDto)
        {
            var user = await _userRepository.GetUser(userPhotoDto.UserId);

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

            if (await _libraryRepository.SaveAll())
            {
                var photoToreturn = _mapper.Map<PhotoForReturnDto>(photo);

                return new ResponseHandler(photoToreturn, photo.Id);
            }

            throw new Exception("Adding Photo failed on save");
        }

        private async Task<bool> DeletePhoto(PhotoSettings settings, Photo photo)
        {
            if (_photoLibrary.DeletePhoto(settings, photo.PublicId))
            {
                _libraryRepository.Delete(photo);

                if (await _libraryRepository.SaveAll())
                {
                    return true;
                }
                throw new Exception("Deleting Photo failed on save");
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