using LibraryManagementSystem.Helpers;
using LMSContracts.Interfaces;
using LMSRepository.Data;
using LMSService.Helpers;
using LMSService.Service;
using LMSService.Validators.services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsAddCombinedInterfaces
    {
        public static void AddCombinedInterfaces(this IServiceCollection services)
        {
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<ILibraryAssetService, LibraryAssetService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILibraryCardService, LibraryCardService>();
            services.AddScoped<IValidatorService, ValidatorService>();
            // services.AddScoped<LogUserActivity>();
            services.AddScoped<DevOnlyActionFilter>();
            services.AddScoped<Seed>();
        }
    }
}
