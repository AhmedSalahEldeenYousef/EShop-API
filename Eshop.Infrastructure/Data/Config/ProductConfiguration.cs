using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.HasData(new Product
            {
            Id = 1, 
            Name  ="test product",
            CategoryId = 1, Description="test product description",
            Price= 12,

            });
        }
    }
}
