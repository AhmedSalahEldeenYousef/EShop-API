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
using Eshop.Infrastructure.Data;
using Eshop.Infrastructure.Repositories.Service;
using Microsoft.EntityFrameworkCore;

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
