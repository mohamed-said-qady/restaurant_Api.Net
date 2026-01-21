
using Microsoft.AspNetCore.Identity;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;

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
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return null;

            if (!await _userManager.CheckPasswordAsync(user, password))
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role == null) return null;

            var permissions = await _rolePermissionService.GetPermissionsByRoleAsync(role);

            return _jwtService.GenerateToken(user, role, permissions);
        }
    }
}
