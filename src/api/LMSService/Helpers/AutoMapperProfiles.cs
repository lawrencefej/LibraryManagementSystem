using System.Linq;
using AutoMapper;
using LMSEntities.DataTransferObjects;
using LMSEntities.Models;

namespace LibraryManagementSystem.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserForListDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                     {
                         opt.MapFrom(src => src.ProfilePicture.Url);
                     })
                     .ForMember(dest => dest.LibraryCardNumber, opt =>
                     {
                         opt.MapFrom(src => src.LibraryCard.Id);
                     });
            CreateMap<AppUser, UserForDetailedDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                     {
                         opt.MapFrom(src => src.ProfilePicture.Url);
                     });
            CreateMap<AppUser, LoginUserDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                    {
                        opt.MapFrom(src => src.ProfilePicture.Url);
                    })
                    .ForMember(dest => dest.Role, opt =>
                    {
                        opt.MapFrom(src => src.UserRoles.FirstOrDefault().Role.Name);
                    });
            CreateMap<UserForUpdateDto, AppUser>();
            CreateMap<UpdateAdminRoleDto, AppUser>();
            CreateMap<UserForRegisterDto, AppUser>();
            CreateMap<AddAdminDto, AppUser>();
            CreateMap<MemberForCreation, AppUser>();
            CreateMap<LibraryCardForCreationDto, LibraryCard>();
            CreateMap<Photo, PhotoForDetailedDto>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<UserPhotoDto, UserProfilePhoto>();
            CreateMap<AssetPhotoDto, AssetPhoto>();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<LibraryAssetForUpdateDto, LibraryAsset>();
            CreateMap<LibraryAsset, LibraryAssetForDetailedDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                    {
                        opt.MapFrom(src => src.Photo.Url);
                    })
                    .ForMember(dest => dest.Authors, opt =>
                    {
                        opt.MapFrom(src => src.AssetAuthors.Select(t => t.Author));
                    })
                    .ForMember(dest => dest.Categories, opt =>
                    {
                        opt.MapFrom(src => src.AssetCategories.Select(t => t.Category));
                    });
            CreateMap<LibraryAsset, LibraryAssetForListDto>()
                .ForMember(dest => dest.AuthorName, opt =>
                {
                    opt.MapFrom(src => src.AssetAuthors.FirstOrDefault().Author.FullName);
                });
            CreateMap<AppUser, AdminUserForListDto>()
                .ForMember(dest => dest.Role, opt =>
                {
                    opt.MapFrom(src => src.UserRoles.FirstOrDefault().Role);
                });
            CreateMap<LibraryAssetForCreationDto, LibraryAsset>();
            CreateMap<AuthorDto, Author>().ReverseMap();
            CreateMap<Checkout, CheckoutForDetailedDto>();
            CreateMap<Checkout, CheckoutForListDto>()
                .ForMember(dest => dest.CardNumber, opt =>
                {
                    opt.MapFrom(src => src.LibraryCard.CardNumber);
                })
                .ForMember(dest => dest.Title, opt =>
                {
                    opt.MapFrom(src => src.LibraryAsset.Title);
                })
                .ForMember(dest => dest.DateReturned, opt =>
                {
                    opt.Condition(src => src.Status == CheckoutStatus.Returned);
                });
            CreateMap<ReserveForCreationDto, ReserveAsset>().ReverseMap();
            CreateMap<ReserveAsset, ReserveForReturnDto>()
                     .ForMember(dest => dest.Id, opt =>
                     {
                         opt.MapFrom(src => src.Id);
                     })
                    .ForMember(dest => dest.Title, opt =>
                     {
                         opt.MapFrom(src => src.LibraryAsset.Title);
                     });
            CreateMap<AppUserRole, UserRoleDto>()
                    .ForMember(dest => dest.Id, opt =>
                    {
                        opt.MapFrom(src => src.Role.Id);
                    })
                    .ForMember(dest => dest.Name, opt =>
                    {
                        opt.MapFrom(src => src.Role.Name);
                    })
                    .ForMember(dest => dest.NormalizedName, opt =>
                    {
                        opt.MapFrom(src => src.Role.NormalizedName);
                    });
            CreateMap<RoleDto, AppRole>();
            CreateMap<AppRole, UserRoleDto>();
            CreateMap<LibraryAssetAuthorDto, LibraryAssetAuthor>().ReverseMap();
            CreateMap<LibraryAssetCategoryDto, LibraryAssetCategory>().ReverseMap();
            CreateMap<LibraryCardForCreationDto, LibraryCard>();
            CreateMap<LibraryCard, LibraryCardForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.LibraryCardPhoto.Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge());
                });
            CreateMap<LibraryCard, LibrarycardForListDto>().ReverseMap();
            CreateMap<LibraryCardForUpdate, LibraryCard>();
            CreateMap<Address, AddressDto>();
            CreateMap<AddAddressDto, Address>();
            CreateMap<StateDto, State>().ReverseMap();

        }
    }
}
