using LibraryManagementSystem.API.Helpers;
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
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<ILibraryAssetService, LibraryAssetService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAssetTypeService, AssetTypeService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<LogUserActivity>();
        }
    }
}