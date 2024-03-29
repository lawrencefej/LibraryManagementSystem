﻿using System;
using System.Text;
using System.Threading.Tasks;
using LMSEntities.Configuration;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Role = LibraryManagementSystem.API.Helpers.Role;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsIdentityConfiguration
    {
        public static void AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            JwtSettings jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

            IdentityBuilder builder = services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(AppRole), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
            builder.AddRoleValidator<RoleValidator<AppRole>>();
            builder.AddRoleManager<RoleManager<AppRole>>();
            builder.AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // TODO validate issue
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            StringValues accessToken = context.Request.Query["access_token"];

                            PathString path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
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
