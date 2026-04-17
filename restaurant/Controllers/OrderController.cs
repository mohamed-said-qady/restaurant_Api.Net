
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
    [Permission("OrderCreate")]
    public async Task<IActionResult> Create(OrderCreateDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized("Invalid user id");

        var result = await _orderService.CreateAsync(userId, dto);
        return Ok(result);
    }


    //[HttpGet]
    //[Permission("Order.View")]
    //public async Task<IActionResult> GetAll()
    //    => Ok(await _orderService.GetAllAsync());


    [HttpGet("{id}")]
    [Permission("OrderView")]
    public async Task<IActionResult> GetById(int id)

        => Ok(await _orderService.GetByIdAsync(id));

    //[HttpGet]
    //[Permission("Order.View")]
    //public async Task<IActionResult> ()
    //{

    //}
    [HttpDelete("{id}")]
    [Permission("OrderDelete")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        // 1. Validation سريع قبل ما نشغل الـ Service ونستهلك موارد
        if (id <= 0) 
            return BadRequest("Invalid ID");

        // 2. منادي الـ Service اللي إنت تعبت فيها
        var success = await _orderService.DeleteAsync(id);

        // 3. نترجم الـ bool لـ Status Code صح
         return NotFound($"Order {id} not found");

        
    }

    [HttpPut("{id}")]
    [Permission("OrderUpdate")]
    public async Task<IActionResult> UpdateAsync([FromRoute]int id ,[FromBody] OrderUpdateDto dto)
    {
        var result = await _orderService.UpdateAsync(id,dto);
        if (!result.IsSuccess)
        {
            return result.StatusCode == 404 ? NotFound(result.Message) : BadRequest(result.Message);
        }

        return NoContent();
    }

  }
