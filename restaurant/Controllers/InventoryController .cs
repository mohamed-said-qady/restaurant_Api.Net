using Microsoft.AspNetCore.Mvc;
using restaurant.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurant.Services.Interfaces;
using restaurant.Dtos;

[ApiController]
[Route("api/inventory")]
[Authorize(Roles = "Admin")] // المخزن Admin فقط
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }



    // عرض المخزون
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _inventoryService.GetAllAsync());
}
