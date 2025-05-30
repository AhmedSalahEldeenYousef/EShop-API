using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eshop.Core.Interfaces;
using Eshop.Core.Services;
using Eshop.Infrastructure.Data;

namespace Eshop.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagmentService _imageManagmentService;
        
        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagmentService imageManagmentService)
        {
            _context = context;
            _mapper = mapper;
           _imageManagmentService = imageManagmentService;
      
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context,_mapper,_imageManagmentService);
            PhotoRepository = new PhotoRepository(_context);
   
        }
    }
}
