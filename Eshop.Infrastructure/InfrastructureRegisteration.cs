using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Core.Interfaces;
using Eshop.Infrastructure.Data;
using Eshop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            //Applying DbContext 

            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("eshopconnectionstring"));
            });
            return services;
        }
    }
}
