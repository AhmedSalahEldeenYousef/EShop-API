using AutoMapper;
using Eshop.API.Helpers;
using Eshop.Core.Entities.Basket;
using Eshop.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.API.Controllers
{

    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> get(string id)
        {
            var result = await _work.CustomerBasket.GetBasketAsync(id);
            if (result is null)
            {
                return Ok(new CustomerBasket());
            }
            return Ok(result);
        }

        [HttpPost("update-basket")]

        public async Task<IActionResult> add(CustomerBasket basket)
        {
            var result = await _work.CustomerBasket.UpdateBasketAsync(basket);
            return Ok(result);
        }

        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
         var result =    await _work.CustomerBasket.DeleteBasketAsync(id);
            return result ? Ok(new ResponseAPI(200,"Item deleted")) : BadRequest(new ResponseAPI(400));
        }
    }
}
