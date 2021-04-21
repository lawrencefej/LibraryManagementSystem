using System.Text;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Role = LibraryManagementSystem.API.Helpers.Role;

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

            builder = new IdentityBuilder(builder.UserType, typeof(LMSEntities.Models.Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
            builder.AddRoleValidator<RoleValidator<LMSEntities.Models.Role>>();
            // TODO: Fix role naming issue
            builder.AddRoleManager<RoleManager<LMSEntities.Models.Role>>();
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
                // TODO: Fix role naming issue
                options.AddPolicy(Role.RequireAdminRole, policy => policy.RequireRole(Role.Admin));
                options.AddPolicy(Role.RequireLibrarianRole, policy => policy.RequireRole(Role.Admin, Role.Librarian));
                options.AddPolicy(Role.RequireMemberRole, policy => policy.RequireRole(Role.Admin, Role.Librarian, Role.Member));
            });
        }
    }
}
