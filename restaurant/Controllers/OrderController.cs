using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using restaurant.Services.Interfaces;
using restaurant.Dtos;

[ApiController]
[Route("api/orders")]
[Authorize] // أي يوزر لازم يكون عامل Login
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // User يعمل Order
    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Create(OrderCreateDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized("Invalid user id");

        var result = await _orderService.CreateAsync(userId, dto);
        return Ok(result);
    }

    // Admin يشوف كل الطلبات
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
        => Ok(await _orderService.GetAllAsync());

    // User يشوف Order بتاعه
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _orderService.GetByIdAsync(id));
}
