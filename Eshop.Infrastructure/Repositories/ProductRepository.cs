using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eshop.Core.DTO;
using Eshop.Core.Entities.Product;
using Eshop.Core.Interfaces;
using Eshop.Core.Services;
using Eshop.Core.Shared;
using Eshop.Infrastructure.Data;
using Eshop.Infrastructure.Repositories.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Eshop.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IImageManagmentService _imageManagmentService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagmentService imageManagmentService) : base(context)
        {
            _mapper = mapper;
            _context = context;
            _imageManagmentService = imageManagmentService;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(ProductParams productParams)
        {
            var query = _context.Products.Include(C => C.Category).Include(p => p.photos).AsNoTracking();

            //filtraing by word
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                //query = query.Where(w => w.Name.ToLower().Contains(productParams.Search.ToLower())
                //|| w.Description.ToLower().Contains(productParams.Search.ToLower()));
                var SearchWords = productParams.Search.Split(' ');
                query = query.Where(w => SearchWords.All(word =>
                
                    w.Name.ToLower().Contains(word.ToLower())
                    || w.Description.ToLower().Contains(word.ToLower())
                ));
            }

            //filtring by category id
            if(productParams.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == productParams.CategoryId);

            }
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAscending" => query.OrderBy(p => p.NewPrice),
                    "PriceDescending" => query.OrderByDescending(p => p.NewPrice),
                    _ => query.OrderBy(p => p.Name),
                };
            }

            //applying pagenation
            //productParams.PageNumber = productParams.PageNumber > 0 ? productParams.PageNumber : 1;
            //productParams.PageSize = productParams.PageSize > 0 ? productParams.PageSize : 3;
            query = query.Skip((productParams.PageSize) * (productParams.PageNumber - 1)).Take(productParams.PageSize);
            //maping to product dto
           var result = _mapper.Map<List<ProductDto>>(query);
            return result;
        }
        public async Task<bool> AddAsync(AddProductDto productDto)
        {
            if (productDto ==null) return false;
            var product = _mapper.Map<Product>(productDto);
           await _context.Products.AddAsync(product);
           await _context.SaveChangesAsync();
            var ImagePath = await _imageManagmentService.AddImageAsync(productDto.Photo, productDto.Name);
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id
            }).ToList();

            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;

        }



        public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto)
        {
            if (updateProductDto is null)
            {
                return false;
            }
            var findProduct = await _context.Products
                .Include(c => c.Category)
                .Include(p => p.photos)
                .FirstOrDefaultAsync(id => id.Id == updateProductDto.Id);

            if (findProduct is null)
            {
                return false;
            }

            _mapper.Map(updateProductDto, findProduct);
            var FindPhoto = await _context.Photos.Where(p => p.ProductId == updateProductDto.Id).ToListAsync();

            foreach (var item in FindPhoto)
            {
                _imageManagmentService.DeleteImageAsync(item.ImageName);
            }

            _context.Photos.RemoveRange(FindPhoto);
            //Adding new photos
            var ImagePath = await _imageManagmentService.AddImageAsync(updateProductDto.Photo, updateProductDto.Name);

            //mapping phoots
            var photos = ImagePath.Select(path=> new Photo{
                ImageName = path,
                ProductId  = updateProductDto.Id
            }).ToList();
            await _context.Photos.AddRangeAsync(photos);
            await _context.SaveChangesAsync();
            return true;

        }


        public async Task DeleteAsync(Product product)
        {
            var photos = await _context.Photos.Where(photo => photo.ProductId == product.Id).ToListAsync();
            foreach (var item in photos)
            {
                _imageManagmentService.DeleteImageAsync(item.ImageName);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

        }
    }
}
