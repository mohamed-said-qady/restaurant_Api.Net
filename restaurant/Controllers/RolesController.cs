// File: Controllers/RolesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using restaurant.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace restaurant.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string roleName)
        {
            var created = await _roleService.CreateAsync(roleName);
            return Ok(created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] IdentityRole<Guid> role)
        {
            var updated = await _roleService.UpdateAsync(role);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _roleService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok();
        }
    }
}
