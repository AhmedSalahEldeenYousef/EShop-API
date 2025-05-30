using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Core.Interfaces;
using Eshop.Core.Services;
using Eshop.Infrastructure.Data;
using Eshop.Infrastructure.Repositories;
using Eshop.Infrastructure.Repositories.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Eshop.Infrastructure
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // services.AddScoped<ICategoryRepository, CategoryRepository>();
            //  services.AddScoped<IProductRepository, ProductRepository>();
            //  services.AddScoped<IPhotoRepository, PhotoRepository>();


            //Applying Unite Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IImageManagmentService, ImageManagmentService>();

            services.AddSingleton<IFileProvider>(
             new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            //Applying DbContext 

            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("eshopconnectionstring"));
            });
            return services;
        }
    }
}
