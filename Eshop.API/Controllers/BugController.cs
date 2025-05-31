using AutoMapper;
using Eshop.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.API.Controllers
{

    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }


        [HttpGet("not-found")]
        public async Task<ActionResult> GetNotFound()
        {
            var category = await _work.CategoryRepository.GetByIdAsync(100);
            if (category is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(category);
            }

        }

        [HttpGet("server-error")]
        public async Task<ActionResult> GetServerError()
        {
            var category = await _work.CategoryRepository.GetByIdAsync(100);
            category.Name = "";  //throw Exception
            return Ok(category);
            
        }

        [HttpGet("bad-request/{id}")]
        public async Task<ActionResult> GetBadRequest(int id)
        {
            return Ok();
        }


        [HttpGet("bad-request")]
        public async Task<ActionResult> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
