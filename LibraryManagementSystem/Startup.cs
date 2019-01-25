using AutoMapper;
using System.Net;
using System.Text;
using LibraryManagement.API.Helpers;
using LMSLibrary.Data;
using LMSLibrary.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using LMSLibrary.Helpers;
using FluentValidation.AspNetCore;
using LMSLibrary.Services;
using LMSLibrary.Validators;
using LMSLibrary.Dto;
using FluentValidation;
using LMSService.Interfaces;
using LMSService.Service;
using LMSRepository.DataAccess;
using LMSRepository.Interfaces;
using LMSService.Exceptions;
using Serilog;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.
                UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(Options =>
                {
                    Options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireLibrarianRole", policy => policy.RequireRole("Admin", "Librarian"));
                options.AddPolicy("RequireMemberRole", policy => policy.RequireRole("Member", "Admin", "Librarian"));
            });

            services.AddMvc(Options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                Options.Filters.Add(new AuthorizeFilter(policy));
                Options.Filters.Add(typeof(ValidateModelAttribute));

            })
            //.AddFluentValidation()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CheckoutForCreationDtoValidator>());
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddCors();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            //Mapper.Reset();
            services.AddAutoMapper();
            services.AddTransient<Seed>();
            //services.AddScoped<IValidator<CheckoutForCreationDto>, CheckoutForCreationDtoValidator>();
            services.AddScoped<ILibraryCardRepository, LibraryCardRepository>();
            services.AddScoped<ILibraryAssetRepository, LibraryAssetRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILibraryCardRepository, LibraryCardRepository>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<ICheckoutRepository, CheckoutRepository>();
            services.AddScoped<IReserveRepository, ReserveRepository>();
            services.AddScoped<LogUserActivity>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));
                //app.UseDeveloperExceptionPage();
                //app.ConfigureCustomExceptionMiddleware();

            }
            else
            {
                //app.ConfigureCustomExceptionMiddleware();
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));
                //app.UseExceptionHandler(builder => {
                //    builder.Run(async context => {
                //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //        var error = context.Features.Get<IExceptionHandlerFeature>();
                //        if (error != null)
                //        {
                //            context.Response.AddApplicationError(error.Error.Message);
                //            await context.Response.WriteAsync(error.Error.Message);
                //        }

                //    });
                //});
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            loggerFactory.AddSerilog();
            seeder.SeedUsers();
            seeder.SeedBooks();
            seeder.SeedMedia();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
