using AutoMapper;
using LMSLibrary.Dto;
using LMSLibrary.Models;
using LMSRepository.Dto;

namespace LibraryManagementSystem.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                     {
                         opt.MapFrom(src => src.ProfilePicture.Url);
                     })
                     .ForMember(dest => dest.LibraryCardNumber, opt =>
                     {
                         opt.MapFrom(src => src.LibraryCard.Id);
                     })
                     .ForMember(dest => dest.Age, opt =>
                     {
                         opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                     });
            CreateMap<User, UserForDetailedDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                     {
                         opt.MapFrom(src => src.ProfilePicture.Url);
                     })
                     .ForMember(dest => dest.LibraryCardNumber, opt =>
                     {
                         opt.MapFrom(src => src.LibraryCard.Id);
                     })
                     .ForMember(dest => dest.Fees, opt =>
                     {
                         opt.MapFrom(src => src.LibraryCard.Fees);
                     })
                    .ForMember(dest => dest.Age, opt =>
                    {
                        opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                    });
            CreateMap<UserForUpdateDto, User>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<LibraryCardForCreationDto, LibraryCard>();
            CreateMap<Photo, PhotoForDetailedDto>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<Status, StatusToReturnDto>();
            CreateMap<AssetType, AssetTypeForReturn>();
            CreateMap<LibraryAssetForUpdateDto, LibraryAsset>();
            CreateMap<LibraryAsset, LibraryAssetForDetailedDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                    {
                        opt.MapFrom(src => src.Photo.Url);
                    })
                    .ForMember(dest => dest.AssetType, opt =>
                    {
                        opt.MapFrom(src => src.AssetType.Name);
                    })
                    .ForMember(dest => dest.Status, opt =>
                    {
                        opt.MapFrom(src => src.Status.Name);
                    })
                    .ForMember(dest => dest.AuthorName, opt =>
                    {
                        opt.MapFrom(src => src.Author.FullName);
                    });
            CreateMap<LibraryAsset, LibraryAssetForListDto>()
                    .ForMember(dest => dest.AssetType, opt =>
                    {
                        opt.MapFrom(src => src.AssetType.Name);
                    })
                    .ForMember(dest => dest.AuthorName, opt =>
                    {
                        opt.MapFrom(src => src.Author.FullName);
                    });
            CreateMap<LibraryAssetForCreationDto, LibraryAsset>().ReverseMap();
            CreateMap<CheckoutForCreationDto, Checkout>().ReverseMap();
            CreateMap<Checkout, CheckoutForReturnDto>()
                     .ForMember(dest => dest.Title, opt =>
                     {
                         opt.MapFrom(src => src.LibraryAsset.Title);
                     })
                     .ForMember(dest => dest.Status, opt =>
                     {
                         opt.MapFrom(src => src.Status.Name);
                     })
                     .ForMember(dest => dest.LibraryCardNumber, opt =>
                     {
                         opt.MapFrom(src => src.LibraryCardId);
                     })
                     .ForMember(dest => dest.Id, opt =>
                     {
                         opt.MapFrom(src => src.Id);
                     });
            CreateMap<ReserveForCreationDto, ReserveAsset>().ReverseMap();
            CreateMap<ReserveAsset, ReserveForReturnDto>()
                     .ForMember(dest => dest.Id, opt =>
                     {
                         opt.MapFrom(src => src.Id);
                     })
                     .ForMember(dest => dest.Status, opt =>
                     {
                         opt.MapFrom(src => src.Status.Name);
                     })
                    .ForMember(dest => dest.Title, opt =>
                     {
                         opt.MapFrom(src => src.LibraryAsset.Title);
                     });
        }
    }
}