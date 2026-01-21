
using Microsoft.AspNetCore.Mvc;
using restaurant.Dtos;
using restaurant.Services.Interfaces;
using System.Threading.Tasks;

namespace restaurant.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto.Username, dto.Password);
            if (token == null) return Unauthorized("اسم المستخدم أو كلمة المرور غير صحيحة");

            return Ok(token);
        }
    }
}
