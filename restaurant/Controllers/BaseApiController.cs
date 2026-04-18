using Microsoft.AspNetCore.Mvc;
using restaurant.Helper;
//using restaurant.Core.General;
namespace restaurant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        // ميثود بتاخد باراميتر واحد وبترجعه زي ما هو بالـ Code بتاعه
        protected IActionResult HandleResult<T>(ServiceResult<T> result)
        {

            return StatusCode(result.StatusCode, result);
        }
    }
}