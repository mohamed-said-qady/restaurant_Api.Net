
using Microsoft.AspNetCore.Identity;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace restaurant.Services.Implementations
{

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IRolePermissionService _rolePermissionService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IRolePermissionService rolePermissionService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _rolePermissionService = rolePermissionService;
        }


        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            if (user == null)
                return BadRequest("user not found");

            if (!await _userManager.CheckPasswordAsync(user, password))
                return BadRequest("password error");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role == null) return null;

            var permissions = await _rolePermissionService.GetPermissionsByRoleAsync(role);

            return _jwtService.GenerateToken(user, role, permissions);
        }

        private string BadRequest(string v)
        {
            throw new Exception();
        }
    }
}
