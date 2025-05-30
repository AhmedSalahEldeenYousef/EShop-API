using AutoMapper;
using Eshop.Core.DTO;
using Eshop.Core.Entities.Product;

namespace Eshop.API.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x=>x.CategoryName, op=>op.MapFrom(src=>src.Category.Name)).ReverseMap();

            CreateMap<Photo, PhotoDto>().ReverseMap();
            CreateMap<AddProductDto, Product>().ForMember(x => x.photos, opt => opt.Ignore()).ReverseMap();
            CreateMap<UpdateProductDto, Product>().ForMember(x => x.photos, opt => opt.Ignore()).ReverseMap();


        }
    }
}
