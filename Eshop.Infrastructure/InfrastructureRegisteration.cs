using Eshop.Core.Interfaces;
using Eshop.Core.Services;
using Microsoft.IdentityModel.Tokens;
using Eshop.Infrastructure.Data;
using Eshop.Infrastructure.Repositories;
using Eshop.Infrastructure.Repositories.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Eshop.Core.Entities.Auth;

namespace Eshop.Infrastructure
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // services.AddScoped<ICategoryRepository, CategoryRepository>();
            //  services.AddScoped<IProductRepository, ProductRepository>();
            //  services.AddScoped<IPhotoRepository, PhotoRepository>();


            //Applying Unite Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IImageManagmentService, ImageManagmentService>();

            //register Email Sender
            services.AddScoped<IEmailService, EmailService>();

            //Adding Token Service
            services.AddScoped<IGenerateToken, GenerateToken>();
            //Adding Auth Service
            services.AddScoped<IAuth, AuthRepository>();
            //Adding File Provider for Static Files
            services.AddSingleton<IFileProvider>(
             new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            //Adding Redies Connection
            services.AddSingleton<IConnectionMultiplexer>(i =>
            {
                var config = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
                return ConnectionMultiplexer.Connect(config);
            });
            //Applying DbContext 

            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("eshopconnectionstring"));
            });


            //Adding Identity

            //services.AddIdentity<AppUser, IdentityRole>(op =>
            //{
            //    op.Password.RequireDigit = false;
            //    op.Password.RequiredLength = 6;
            //    op.Password.RequireLowercase = false;
            //    op.Password.RequireUppercase = false;
            //    op.Password.RequireNonAlphanumeric = false;
            //});

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })

           .AddCookie(o =>
            {
                o.Cookie.Name = "token";
                o.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return Task.CompletedTask;
                };
            })

           .AddJwtBearer(op =>
            {
                op.RequireHttpsMetadata = false;
                op.SaveToken = true;
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Secret"])),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Token:Issuer"],
                    //ValidAudience = configuration["Token:Audience"],
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // Disable the default 5 minute clock skew
                };
                op.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }
    }
}
