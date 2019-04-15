using EmailService;
using EmailService.Services;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.DataAccess;
using LMSRepository.Interfaces;
using LMSRepository.Interfaces.DataAccess;
using LMSService.Dto;
using LMSService.Helpers;
using LMSService.Interfaces;
using LMSService.Service;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsAddCombinedInterfaces
    {
        public static void AddCombinedInterfaces(this IServiceCollection services)
        {
            services.AddScoped<ILibraryCardRepository, LibraryCardRepository>();
            services.AddScoped<ILibraryAssetRepository, LibraryAssetRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILibraryCardRepository, LibraryCardRepository>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IReserveService, ReserveService>();
            services.AddScoped<ICheckoutRepository, CheckoutRepository>();
            services.AddScoped<IReserveRepository, ReserveRepository>();
            services.AddScoped<ILibraryAssestService, LibraryAssetService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAssetTypeService, AssetTypeService>();
            services.AddScoped<IAssetTypeRepository, AssetTypeRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IEmailService, MailtrapService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<LogUserActivity>();
        }
    }
}