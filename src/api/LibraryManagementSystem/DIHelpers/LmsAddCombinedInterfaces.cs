using LibraryManagementSystem.Helpers;
using LMSContracts.Interfaces;
using LMSRepository.Data;
using LMSService.Service;
using LMSService.Validators.services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsAddCombinedInterfaces
    {
        public static void AddCombinedInterfaces(this IServiceCollection services)
        {
            // services.AddScoped<LogUserActivity>();
            // services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<DevOnlyActionFilter>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ILibraryAssetService, LibraryAssetService>();
            services.AddScoped<ILibraryCardService, LibraryCardService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IValidatorService, ValidatorService>();
            services.AddScoped<Seed>();
        }
    }
}
