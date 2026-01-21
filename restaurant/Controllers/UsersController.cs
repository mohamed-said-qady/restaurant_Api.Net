// File: Controllers/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using restaurant.Model;
using restaurant.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace restaurant.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ApplicationUser user, string password)
        {
            var created = await _userService.CreateAsync(user, password);
            return Ok(created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ApplicationUser user)
        {
            var updated = await _userService.UpdateAsync(user);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok();
        }
    }
}
