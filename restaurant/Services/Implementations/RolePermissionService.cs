using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using restaurant.Services.Interfaces;
using restaurant.Data;
using restaurant.Model;

namespace restaurant.Services.Implementations
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly AppDbContext _context;

        public RolePermissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetPermissionsByRoleAsync(string roleName)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .Where(rp => rp.Role.Name == roleName)
                .Select(rp => rp.Permission.Code)
                .ToListAsync();
        }
    }
}