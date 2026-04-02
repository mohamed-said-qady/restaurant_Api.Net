using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace restaurant.Repositories.Implementations
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManger;
        public RolePermissionRepository(AppDbContext context, RoleManager<IdentityRole> roleManger) : base(context)
        {
            _context=context;
            _roleManger = roleManger;
        }
        public Task<List<string>> GetPermissionsByRoleAsync(string roleName) {
        return _context.RolePermissions
            .Where(rp => rp.Role.Name == roleName)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();
        }
        public async Task<bool> CreateRoleWithPermission(CreateRoleWithPermissionDto dto) {
            using var transaction = await _context.Database.BeginTransactionAsync();
            // find the role by its name, if it doesn't exist, create it.
            var role = await _roleManger.FindByNameAsync(dto.RoleName);
            try
            {
                if (role == null)
                {
                    role = new IdentityRole(dto.RoleName);
                    var result = await _roleManger.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception();
                    }
                }

                else
                {
                    // the role already exists
                    Console.WriteLine($"Role '{dto.RoleName}' already exists.");
                }


                foreach (var d in dto.PermissionIds)//public List<int>PermissionIds{get;set;}=new List<int>();
                {
                    //افحص لو فيه علاقه في RolePermission سيبها وضيف اللي مكتوب بس من permission جديد 
                    var exists = await _context.RolePermissions.AnyAsync(rp => rp.RoleId == Guid.Parse(role.Id) && rp.PermissionId == d);
                    if (!exists)
                    {
                        var rolePermission = new RolePermission
                        {
                            PermissionId = d,
                            RoleId = Guid.Parse(role.Id)
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