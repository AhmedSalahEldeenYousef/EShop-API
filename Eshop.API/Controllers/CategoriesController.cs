using Eshop.Core.DTO;
using Eshop.Core.Entities.Product;
using Eshop.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.API.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work) : base(work)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            try
            {
                var categories = await _work.CategoryRepository.GetAllAsync();
                if(categories is null)
                {
                    return BadRequest();
                }else
                {
                    return Ok(categories);
                }
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
                var category = await _work.CategoryRepository.GetByIdAsync(id);
                if (category is null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(category);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> Add(CategoryDto categoryDto)
        {
            try
            {
                var category = new Category()
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description
                };
                 await _work.CategoryRepository.AddAsync(category);
                return Ok(new { message = "Item added Successfully!"});
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Updating Category
        [HttpPut("update-category")]

        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto categoryDto)
        {
            try
            {
                var category = new Category()
                {
                    Description = categoryDto.Description,
                    Name = categoryDto.Name,    
                    Id  = categoryDto.id
                };
                 await _work.CategoryRepository.UpdateAsync(category);
                 return Ok(new { message = "Category Updated Successfully!" });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Delete Category
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                 await _work.CategoryRepository.DeleteAsync(id);
                 return Ok(new { messgae = "Category deleted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
