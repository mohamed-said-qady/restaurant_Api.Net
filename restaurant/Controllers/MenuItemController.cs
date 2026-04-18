using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurant.Authorization;
using restaurant.Dtos;
using restaurant.Services.Interfaces;

namespace restaurant.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/MenuItem")]

    public class MenuItemController : BaseApiController
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        // أي حد يشوف المينيو
        [HttpGet]
        [Permission(Permissions.MenuItemViewAll)]
        public async Task<IActionResult> GetAll([FromQuery] MenuItemSpecParams dto)
            => HandleResult(await _menuItemService.GetAllAsync(dto));


        [HttpGet("{id}")]
        [Permission(Permissions.MenuItemView)]
        public async Task<IActionResult> GetById(int id)
            => HandleResult(await _menuItemService.GetByIdAsync(id));

        // Admin يضيف صنف
        [HttpPost]
        [Permission(Permissions.MenuItemCreate)]
        public async Task<IActionResult> Create(MenuItemCreateDto dto)
            => HandleResult(await _menuItemService.CreateAsync(dto));


        // Admin يعدل
        [HttpPut("{id}")]
        [Permission(Permissions.MenuItemUpdate)]
        public async Task<IActionResult> Update(int id, MenuItemUpdateDto dto)
            => HandleResult(await _menuItemService.UpdateAsync(id, dto));

        // Admin يمسح
        [HttpDelete("{id}")]
        [Permission(Permissions.MenuItemDelete)]
        public async Task<IActionResult> Delete(int id)
            => HandleResult(await _menuItemService.DeleteAsync(id));
    }
}