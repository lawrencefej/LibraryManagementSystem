using AutoMapper;
using FluentValidation.AspNetCore;
using LibraryManagementSystem.API.Helpers;
using LibraryManagementSystem.Models;
using LMSRepository.Data;
using LMSRepository.DataAccess;
using LMSRepository.Interfaces;
using LMSRepository.Interfaces.DataAccess;
using LMSRepository.Interfaces.Helpers;
using LMSRepository.Interfaces.Models;
using LMSService.Exceptions;
using LMSService.Dto;
using LMSService.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;
using Role = LibraryManagementSystem.API.Helpers.Role;
using Microsoft.AspNetCore.Identity.UI.Services;
using LMSService.Helpers;
using Microsoft.IdentityModel.Logging;

namespace LibraryManagementSystem.API
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
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<DataContext>(x => x.
                UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(LMSRepository.Interfaces.Models.Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
            builder.AddRoleValidator<RoleValidator<LMSRepository.Interfaces.Models.Role>>();
            builder.AddRoleManager<RoleManager<LMSRepository.Interfaces.Models.Role>>();
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
                options.AddPolicy(Role.RequireAdminRole, policy => policy.RequireRole(Role.Admin));
                options.AddPolicy(Role.RequireLibrarianRole, policy => policy.RequireRole(Role.Admin, Role.Librarian));
                options.AddPolicy(Role.RequireMemberRole, policy => policy.RequireRole(Role.Admin, Role.Librarian, Role.Member));
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
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddTransient<IEmailSender, EmailSender>();
            //Mapper.Reset();
            services.AddAutoMapper();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Library Management System", Version = "V1" });
            });
            services.AddTransient<Seed>();
            //services.AddScoped<IValidator<CheckoutForCreationDto>, CheckoutForCreationDtoValidator>();
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

            services.AddScoped<LogUserActivity>();

            //services.AddDbContext<LibraryManagementSystemContext>(options =>
            //        options.UseSqlServer(Configuration.GetConnectionString("LibraryManagementSystemContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));
                app.UseDeveloperExceptionPage();
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
            seeder.SeedAuthors();
            seeder.SeedAssets();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management System V1");
            });
        }
    }
}