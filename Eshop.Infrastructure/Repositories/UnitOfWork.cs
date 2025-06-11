using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eshop.Core.Interfaces;
using Eshop.Core.Services;
using Eshop.Infrastructure.Data;
using StackExchange.Redis;

namespace Eshop.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagmentService _imageManagmentService;
        private readonly IConnectionMultiplexer _redis;

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public ICustomerBasketRepository CustomerBasket { get; }

        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagmentService imageManagmentService,IConnectionMultiplexer redis)
        {
            _context = context;
            _mapper = mapper;
           _imageManagmentService = imageManagmentService;
            _redis = redis;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context,_mapper,_imageManagmentService);
            PhotoRepository = new PhotoRepository(_context);
            CustomerBasket = new CustomerBasketRepository(_redis);


        }
    }
}
