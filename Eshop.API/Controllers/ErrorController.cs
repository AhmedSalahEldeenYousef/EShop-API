using Eshop.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.API.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public  IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ResponseAPI(statusCode));

        }
    }
}
