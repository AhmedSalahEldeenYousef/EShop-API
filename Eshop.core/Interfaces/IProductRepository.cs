using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Core.DTO;
using Eshop.Core.Entities.Product;

namespace Eshop.Core.Interfaces
{
    public interface IProductRepository :IGenericRepository<Product>
    {
        //TODO::Handle Product Adding Images Using product dto
         Task<IEnumerable<ProductDto>> GetAllAsync(string sort, int? categoryId);
        Task<bool> AddAsync(AddProductDto productDto);
        Task<bool> UpdateAsync(UpdateProductDto updateProductDto);
        Task DeleteAsync(Product product);
    }
}
