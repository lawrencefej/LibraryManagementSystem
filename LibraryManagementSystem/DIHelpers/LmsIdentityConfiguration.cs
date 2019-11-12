using LMSRepository.Data;
using LMSRepository.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PolicyRole = LibraryManagementSystem.API.Helpers.PolicyRole;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsIdentityConfiguration
    {
        public static void AddIdentityConfiguration(this IServiceCollection services, string token)
        {
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(LMSRepository.Models.Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
            builder.AddRoleValidator<RoleValidator<LMSRepository.Models.Role>>();
            builder.AddRoleManager<RoleManager<LMSRepository.Models.Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(Options =>
                {
                    Options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(token)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyRole.RequireAdminRole, policy => policy.RequireRole(PolicyRole.Admin));
                options.AddPolicy(PolicyRole.RequireLibrarianRole, policy => policy.RequireRole(PolicyRole.Admin, PolicyRole.Librarian));
                options.AddPolicy(PolicyRole.RequireMemberRole, policy => policy.RequireRole(PolicyRole.Admin, PolicyRole.Librarian, PolicyRole.Member));
            });
        }
    }
}