using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Core.DTO;
using Eshop.Core.Entities.Product;
using Eshop.Core.Shared;

namespace Eshop.Core.Interfaces
{
    public interface IProductRepository :IGenericRepository<Product>
    {
        //TODO::Handle Product Adding Images Using product dto
         Task<RetunProductDto> GetAllAsync(ProductParams productParams);
        Task<bool> AddAsync(AddProductDto productDto);
        Task<bool> UpdateAsync(UpdateProductDto updateProductDto);
        Task DeleteAsync(Product product);
    }
}
