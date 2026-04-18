using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurant.Authorization;
using restaurant.Dtos;
using restaurant.Services.Interfaces;
using System.Security.Claims;

namespace restaurant.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Order")]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Permission(Permissions.OrderCreate)]
        public async Task<IActionResult> Create(OrderCreateDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized("Invalid user id");

            // بنستخدم HandleResult عشان نرجع الـ ServiceResult موحد
            return HandleResult(await _orderService.CreateAsync(userId, dto));
        }

        [HttpGet("{id}")]
        [Permission(Permissions.OrderView)]
        public async Task<IActionResult> GetById(int id)
            => HandleResult(await _orderService.GetByIdAsync(id));

        [HttpDelete("{id}")]
        [Permission(Permissions.OrderDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            // الـ Validation البسيط ده ممكن تخليه في الـ Service برضه لو عاوز الكنترولر "أبيض"
            if (id <= 0) return BadRequest("Invalid ID");

            return HandleResult(await _orderService.DeleteAsync(id));
        }

        [HttpPut("{id}")]
        [Permission(Permissions.OrderUpdate)]
        public async Task<IActionResult> Update(int id, OrderUpdateDto dto)
            => HandleResult(await _orderService.UpdateAsync(id, dto));

        [HttpGet]
        [Permission(Permissions.OrderView)]
        public async Task<IActionResult> GetAll([FromQuery] OrderSpecParams dto) // فرضاً إن عندك Params للفلترة
            => HandleResult(await _orderService.GetAllAsync(dto));
    }
}