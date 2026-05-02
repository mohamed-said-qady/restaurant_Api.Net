
using Microsoft.AspNetCore.Identity;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using restaurant.Helper;
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


        public async Task<ServiceResult<string>> LoginAsync(string username, string password)
        {
            // البحث عن المستخدم
            var user = await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                return ServiceResult<string>.Failure("اسم المستخدم أو كلمة المرور غير صحيحة", 401);
            }

            // التأكد من كلمة المرور
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                return ServiceResult<string>.Failure("اسم المستخدم أو كلمة المرور غير صحيحة", 401);
            }

            // جلب الأدوار (Roles)
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role == null)
            {
                return ServiceResult<string>.Failure("المستخدم ليس لديه صلاحيات وصول", 403);
            }

            // جلب الصلاحيات وتوليد التوكن
            var permissions = await _rolePermissionService.GetPermissionsByRoleAsync(role);
            var token = _jwtService.GenerateToken(user, role, permissions);

            return ServiceResult<string>.Success(token, "تم تسجيل الدخول بنجاح");
        }
    }
}
