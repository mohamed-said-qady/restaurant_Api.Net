
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using restaurant.Services.Interfaces;
using restaurant.Dtos;
using restaurant.Authorization;


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
    [Permission("Order.Create")]
    public async Task<IActionResult> Create(OrderCreateDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized("Invalid user id");

        var result = await _orderService.CreateAsync(userId, dto);
                return Ok(result);
    }


    [HttpGet]
    [Permission("Order.View")]
    public async Task<IActionResult> GetAll()
        => Ok(await _orderService.GetAllAsync());


    [HttpGet("{id}")]
    [Permission("Order.View")]
    public async Task<IActionResult> GetById(int id)

        => Ok(await _orderService.GetByIdAsync(id));

}
