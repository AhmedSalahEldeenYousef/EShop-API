using AutoMapper;
using Eshop.API.Helpers;
using Eshop.Core.DTO;
using Eshop.Core.Entities.Product;
using Eshop.Core.Interfaces;
using Eshop.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.API.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> Get([FromQuery]ProductParams productParams )
        {
            try
            {
                var products = await _work.ProductRepository.GetAllAsync(productParams);

                //var products = await _work.ProductRepository.GetAllAsync(x=>x.Category, x=>x.photos);

                //var result = _mapper.Map<List<ProductDto>>(products);
                //if (products is null)
                //{
                //    return BadRequest(new ResponseAPI(400));
                //}
                //var TotalCount = await _work.ProductRepository.CoutAsync();
                return Ok(new Pagination<ProductDto>(productParams.PageSize, productParams.PageNumber, products.TotalCount, products.products));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id/{id}")]

        public async Task<IActionResult> GetById(int id)
        {

            try
            {
                var product = await _work.ProductRepository.GetByIdAsync(id, x=>x.Category, x=>x.photos);
                var result = _mapper.Map<ProductDto>(product);
                if (product is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-product")]
        public async Task<IActionResult> Add(AddProductDto productDto)
        {
            try
            {
                await _work.ProductRepository.AddAsync(productDto);
                return Ok( new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            try
            {
                await _work.ProductRepository.UpdateAsync(updateProductDto);
                return Ok( new ResponseAPI(200));

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Delete Category
        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {

            try
            {
                var product = await _work.ProductRepository.GetByIdAsync(id, p => p.photos, c => c.Category);
                await _work.ProductRepository.DeleteAsync(product);
                return Ok( new ResponseAPI(200));

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
