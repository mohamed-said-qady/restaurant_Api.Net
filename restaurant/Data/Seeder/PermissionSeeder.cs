using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using restaurant.Authorization;
using restaurant.Data;
using restaurant.Model;
using System.Data;

namespace restaurant.Data.Seeder
{
    public class PermissionSeeder
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole<Guid>> _Roles;
        public PermissionSeeder(AppDbContext context , RoleManager<IdentityRole<Guid>> roles )
        {
            _context = context;
            _Roles = roles;
        }

        public async Task SeedAsync()
        {
            // Admin Permissions
            await AddPermissionToRole(Roles.Admin, Permissions.OrderCreate);
            await AddPermissionToRole(Roles.Admin, Permissions.OrderUpdate);
            await AddPermissionToRole(Roles.Admin, Permissions.OrderView);
            await AddPermissionToRole(Roles.Admin, Permissions.OrderDelete);
            await AddPermissionToRole(Roles.Admin, Permissions.OrderViewByMenu);
            await AddPermissionToRole(Roles.Admin, Permissions.OrderViewAll);



            await AddPermissionToRole(Roles.Admin, Permissions.ProductCreate);
            await AddPermissionToRole(Roles.Admin, Permissions.ProductUpdate);
            await AddPermissionToRole(Roles.Admin, Permissions.ProductView);

            await AddPermissionToRole(Roles.Admin, Permissions.InventoryView);
            await AddPermissionToRole(Roles.Admin, Permissions.InventoryUpdate);
            await AddPermissionToRole(Roles.Admin, Permissions.InventoryCreate);
            await AddPermissionToRole(Roles.Admin, Permissions.InventoryViewByMenu);

            await AddPermissionToRole(Roles.Admin, Permissions.RolePermissionManage);




            // Chef Permissions
            await AddPermissionToRole(Roles.Chef, Permissions.OrderView);
            await AddPermissionToRole(Roles.Chef, Permissions.ProductView);

            await _context.SaveChangesAsync();
        }

        private async Task AddPermissionToRole(string roleName, string permissionCode)
        {

            //  هات الـ Role
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);


            if (role == null)
                return;

            //  هات أو أنشئ Permission
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

            // تأكد إن الربط مش موجود قبل كدا

            var exists = await _context.RolePermissions.AnyAsync(rp =>
                rp.RoleId == role.Id &&
                rp.PermissionId == permission.Id
            );

                if (exists)
                    return;

            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id,
                PermissionId = permission.Id
            });
        }
    }
}
