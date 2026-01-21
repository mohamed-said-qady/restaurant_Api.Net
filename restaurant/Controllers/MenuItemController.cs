using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurant.Dtos;
using restaurant.Services.Interfaces;

[ApiController]
[Route("api/menu-items")]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuItemController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    // أي حد يشوف المينيو
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
        => Ok(await _menuItemService.GetAllAsync());

    // Admin يضيف صنف
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(MenuItemCreateDto dto)
        => Ok(await _menuItemService.CreateAsync(dto));

    // Admin يعدل
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, MenuItemUpdateDto dto)
        => Ok(await _menuItemService.UpdateAsync(id, dto));

    // Admin يمسح
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _menuItemService.DeleteAsync(id);
        return NoContent();
    }
}
