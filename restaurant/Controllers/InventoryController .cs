using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurant.Authorization;
using restaurant.Controllers;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Services.Interfaces;

[ApiController]
[Authorize]
[Route("api/inventory")]
public class InventoryController : BaseApiController
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    [Permission(Permissions.InventoryView)]
    public async Task<IActionResult> GetAll(invetorySpecParams dto)
        => HandleResult(await _inventoryService.GetAllAsync(dto));


}
