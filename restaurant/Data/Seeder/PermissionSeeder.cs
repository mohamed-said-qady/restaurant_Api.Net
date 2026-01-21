using restaurant.Authorization;
using restaurant.Data;
using restaurant.Model;
using Microsoft.EntityFrameworkCore;

namespace restaurant.Data.Seeders
{
    public class RolePermissionSeeder
    {
        private readonly AppDbContext _context;

        public RolePermissionSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Admin Permissions
            await AddPermissionToRole(Roles.Admin, Permissions.OrderCreate);
            await AddPermissionToRole(Roles.Admin, Permissions.OrderUpdate);
            await AddPermissionToRole(Roles.Admin, Permissions.OrderView);

            await AddPermissionToRole(Roles.Admin, Permissions.ProductCreate);
            await AddPermissionToRole(Roles.Admin, Permissions.ProductUpdate);
            await AddPermissionToRole(Roles.Admin, Permissions.ProductView);

            // Chef Permissions
            await AddPermissionToRole(Roles.Chef, Permissions.OrderView);
            await AddPermissionToRole(Roles.Chef, Permissions.ProductView);

            await _context.SaveChangesAsync();
        }

        private async Task AddPermissionToRole(string roleName, string permissionCode)
        {
            // 1️⃣ هات الـ Role
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName);

            if (role == null)
                return;

            // 2️⃣ هات أو أنشئ Permission
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Code == permissionCode);

            if (permission == null)
            {
                permission = new Permission
                {
                    Code = permissionCode
                };

                _context.Permissions.Add(permission);
                await _context.SaveChangesAsync();
            }

            // 3️⃣ تأكد إن الربط مش موجود قبل كدا
            var exists = await _context.RolePermissions.AnyAsync(rp =>
                rp.RoleId == role.Id &&
                rp.PermissionId == permission.Id
            );

            if (exists)
                return;

            // 4️⃣ اعمل الربط
            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id,
                PermissionId = permission.Id
            });
        }
    }
}