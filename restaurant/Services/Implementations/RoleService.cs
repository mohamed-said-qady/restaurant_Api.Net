using Microsoft.AspNetCore.Identity;
using restaurant.Services.Interfaces;
using restaurant.Helper; // اتأكد من النيم سبايس بتاعك
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace restaurant.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RoleService(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ServiceResult<IdentityRole<Guid>>> CreateAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return ServiceResult<IdentityRole<Guid>>.Failure("اسم الدور مطلوب", 400);

            var role = new IdentityRole<Guid> { Name = roleName };
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ServiceResult<IdentityRole<Guid>>.Failure(errors, 400);
            }

            return ServiceResult<IdentityRole<Guid>>.Success(role, "تم إنشاء الدور بنجاح", 201);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return ServiceResult<bool>.Failure("عفواً، الدور غير موجود", 404);

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return ServiceResult<bool>.Failure("فشل حذف الدور", 400);

            return ServiceResult<bool>.Success(true, "تم حذف الدور بنجاح", 200);
        }

        public async Task<ServiceResult<IEnumerable<IdentityRole<Guid>>>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return ServiceResult<IEnumerable<IdentityRole<Guid>>>.Success(roles, "تم جلب الأدوار بنجاح");
        }

        public async Task<ServiceResult<IdentityRole<Guid>>> GetByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return ServiceResult<IdentityRole<Guid>>.Failure("الدور غير موجود", 404);

            return ServiceResult<IdentityRole<Guid>>.Success(role);
        }

        public async Task<ServiceResult<IdentityRole<Guid>>> UpdateAsync(Guid id, string newName)
        {
            var existing = await _roleManager.FindByIdAsync(id.ToString());
            if (existing == null)
                return ServiceResult<IdentityRole<Guid>>.Failure("الدور غير موجود", 404);

            existing.Name = newName;
            var result = await _roleManager.UpdateAsync(existing);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ServiceResult<IdentityRole<Guid>>.Failure(errors, 400);
            }

            return ServiceResult<IdentityRole<Guid>>.Success(existing, "تم تحديث الدور بنجاح");
        }
    }
}