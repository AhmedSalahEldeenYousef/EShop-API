using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Core.Entities.Product;
using Microsoft.AspNetCore.Http;

namespace Eshop.Core.DTO
{
    public record ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public string CategoryName { get; set; }
        public virtual List<PhotoDto> photos { get; set; }
    }

    public record PhotoDto
    {
        public int ProductId { get; set; }
        public string ImageName { get; set; }
    }
    public record AddProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public int CategoryId { get; set; }
        public IFormFileCollection Photo { get; set; }
    }

    public record UpdateProductDto : AddProductDto
    {
        public int Id { get; set; }
    }
}
