
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using restaurant.Dtos;
using restaurant.Services.Interfaces;
using System.Threading.Tasks;

namespace restaurant.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginDto dto)
        //{
        //    var token = await _authService.LoginAsync(dto.Username, dto.Password);
        //    if (token == null)
        //        return Unauthorized("اسم المستخدم أو كلمة المرور غير صحيحة");
        //    return Ok(token);
        //}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // بتبعت الـ Username والـ Password وبترجع النتيجة للـ HandleResult
            var result = await _authService.LoginAsync(dto.Username, dto.Password);

            return HandleResult(result);
        }
    }
}
