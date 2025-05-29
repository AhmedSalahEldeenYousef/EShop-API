using AutoMapper;
using Eshop.Core.DTO;
using Eshop.Core.Entities.Product;

namespace Eshop.API.Mapping
{
    public class CategoryMapping :Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
        }
    }
}
