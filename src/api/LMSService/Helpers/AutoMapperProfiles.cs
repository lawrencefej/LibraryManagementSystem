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
            //  .ForMember(dest => dest.Age, opt =>
            //  {
            //      opt.MapFrom(d => d.DateOfBirth.CalculateAge());
            //  });
            CreateMap<AppUser, UserForDetailedDto>()
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
                     });
            // .ForMember(dest => dest.Age, opt =>
            // {
            //     opt.MapFrom(d => d.DateOfBirth.CalculateAge());
            // });
            CreateMap<UserForUpdateDto, AppUser>();
            CreateMap<UpdateAdminDto, AppUser>();
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
            //.ForMember(dest => dest.AssetType, opt =>
            //{
            //    opt.MapFrom(src => src.AssetType.Name);
            //})
            // .ForMember(dest => dest.Status, opt =>
            // {
            //     opt.MapFrom(src => src.Status.Name);
            // });
            // .ForMember(dest => dest.Category, opt =>
            // {
            //     opt.MapFrom(src => src.Category.Name);
            // })
            // .ForMember(dest => dest.AuthorName, opt =>
            // {
            //     opt.MapFrom(src => src.AssetAuthors.Select(t => t.Author.FullName));
            // });
            CreateMap<LibraryAsset, LibraryAssetForListDto>()
                .ForMember(dest => dest.AuthorName, opt =>
                {
                    opt.MapFrom(src => src.AssetAuthors.FirstOrDefault().Author.FullName);
                });
            // .ForMember(dest => dest.AssetType, opt =>
            // {
            //     opt.MapFrom(src => src.AssetType.Name);
            // })
            // .ForMember(dest => dest.AuthorName, opt =>
            // {
            //     opt.MapFrom(src => src.Author.FullName);
            // });
            CreateMap<LibraryAssetForCreationDto, LibraryAsset>();
            //   .ForMember(dest => dest.AuthorId, opt =>
            //   {
            //       opt.MapFrom(src => src.Author.Id);
            //   });
            // .ForMember(dest => dest.AssetTypeId, opt =>
            // {
            //     opt.MapFrom(src => src.AssetType.Id);
            // })
            //.ForMember(dest => dest.CategoryId, opt =>
            //{
            //    opt.MapFrom(src => src.Category.Id);
            //});
            CreateMap<AuthorDto, Author>().ReverseMap();
            // CreateMap<CheckoutForCreationDto, Checkout>().ReverseMap();
            CreateMap<Checkout, CheckoutForDetailedDto>();
            // CreateMap<CheckoutItem, CheckoutItemForReturn>();
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
                    opt.Condition(src => (src.Status == CheckoutStatus.Returned));
                });
            // .ForMember(dest => dest.ItemCount, opt =>
            // {
            //     opt.MapFrom(src => src.Items.Count);
            // });
            // CreateMap<Checkout, CheckoutForReturnDto>()
            //          .ForMember(dest => dest.Title, opt =>
            //          {
            //              opt.MapFrom(src => src.LibraryAsset.Title);
            //          })
            //          .ForMember(dest => dest.Status, opt =>
            //          {
            //              opt.MapFrom(src => src.Status.Name);
            //          })
            //          .ForMember(dest => dest.LibraryCardNumber, opt =>
            //          {
            //              opt.MapFrom(src => src.LibraryCardId);
            //          })
            //          .ForMember(dest => dest.Id, opt =>
            //          {
            //              opt.MapFrom(src => src.Id);
            //          });
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
            CreateMap<LibraryAssetAuthorDto, LibraryAssetAuthor>().ReverseMap();
            CreateMap<LibraryAssetCategoryDto, LibraryAssetCategory>().ReverseMap();
            CreateMap<LibraryCardForCreationDto, LibraryCard>();
            CreateMap<LibraryCard, LibraryCardForDetailedDto>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge());
                });
            CreateMap<LibraryCard, LibrarycardForListDto>().ReverseMap();
            CreateMap<LibraryCardForUpdate, LibraryCard>();
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<StateDto, State>().ReverseMap();
            // CreateMap<CheckoutForCreationDto, Checkout>();
            // CreateMap<CheckoutItemForCreationDto, CheckoutItem>();

        }
    }
}
