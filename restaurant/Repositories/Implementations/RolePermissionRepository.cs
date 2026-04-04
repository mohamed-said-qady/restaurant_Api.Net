using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq; // ضيف دي عشان الـ Where و الـ Select يشتغلوا

namespace restaurant.Repositories.Implementations
{
    // لاحظ التغيير هنا: استخدمنا IdentityRole<Guid>
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager; // تعديل النوع لـ Guid

        public RolePermissionRepository(AppDbContext context, RoleManager<IdentityRole<Guid>> roleManager) : base(context)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<List<string>> GetPermissionsByRoleAsync(string roleName)
        {
            return await _context.RolePermissions
                .Where(rp => rp.Role.Name == roleName)
                .Select(rp => rp.Permission.Code)
                .ToListAsync();
        }

        public async Task<bool> CreateRoleWithPermission(CreateRoleWithPermissionDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // البحث عن الـ Role
                var role = await _roleManager.FindByNameAsync(dto.RoleName);

                if (role == null)
                {
                    // إنشاء Role جديد مع تحديد الـ Guid
                    role = new IdentityRole<Guid>(dto.RoleName);
                    var result = await _roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed to create role");
                    }
                }

                foreach (var d in dto.PermissionIds)
                {
                    // بما إن الـ role.Id دلوقتِ Guid أصلاً مش محتاج Parse لو الموديل صح
                    // بس للتأكيد لو هو راجع كـ string من الكلاس الأساسي:
                    var roleIdGuid = role.Id;

                    var exists = await _context.RolePermissions
                        .AnyAsync(rp => rp.RoleId == roleIdGuid && rp.PermissionId == d);

                    if (!exists)
                    {
                        var rolePermission = new RolePermission
                        {
                            PermissionId = d,
                            RoleId = roleIdGuid
                        };
                        _context.RolePermissions.Add(rolePermission);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}