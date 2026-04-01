using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using restaurant.Services.Interfaces;
using restaurant.Data;
using restaurant.Model;
using restaurant.Dtos;

namespace restaurant.Services.Implementations
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly AppDbContext _context;

        public RolePermissionService(AppDbContext context)
        {
            _context = context;
        }
        //محتاج دالة تنشأ علاقة بين Role و Permission
        //public async Task<RolePermission> AssignPermissionToRoleAsync(string roleName, string permissionCode)
        //{
        //    var role = await _context.Roles
        //        .FirstOrDefaultAsync(r => r.Name == roleName);

        //    if (role == null)
        //        throw new ArgumentException($"Role '{roleName}' not found.");

        //    var permission = await _context.Permissions
        //        .FirstOrDefaultAsync(p => p.Code == permissionCode);

        //    if (permission == null)
        //        throw new ArgumentException($"Permission '{permissionCode}' not found.");

        //    var existingRelationship = await _context.RolePermissions
        //        .FirstOrDefaultAsync(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id);

        //    if (existingRelationship != null)
        //        throw new InvalidOperationException($"Permission '{permissionCode}' is already assigned to role '{roleName}'.");

        //    var rolePermission = new RolePermission
        //    {
        //        RoleId = role.Id,
        //        PermissionId = permission.Id
        //    };

        //    _context.RolePermissions.Add(rolePermission);
        //    await _context.SaveChangesAsync();

        //    return rolePermission;
        //}
        public async Task<RolePermission> CreateRoleWithPermission( CreateRoleWithPermissionDto dto)
        {
            // Implementation goes here.
            // This method should create a new role and assign the specified permissions to it.
            // You would typically start by creating the role, then loop through the list of permission codes.
            // find each permission by its code, and create a RolePermission entry for each one.   
        }



        public async Task<List<string>> GetPermissionsByRoleAsync(string roleName)
        {
            return await _context.RolePermissions.
                Include(RP => RP.Role)
                .Include(RP => RP.Permission)
                .Where(RP => RP.Role.Name == roleName)
                .Select(RP => RP.Permission.Code)
                .ToListAsync();
        }
    }
}
//return await _context.RolePermissions
//    .Include(rp => rp.Role)
//    .Include(rp => rp.Permission)
//    .Where(rp => rp.Role.Name == roleName)
//    .Select(rp => rp.Permission.Code)
//    .ToListAsync();