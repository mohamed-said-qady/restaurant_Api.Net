// File: Services/Implementation/RoleService.cs
using Microsoft.AspNetCore.Identity;
using restaurant.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RoleService(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityRole<Guid>> CreateAsync(string roleName)
        {
            var role = new IdentityRole<Guid> { Name = roleName };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors));

            return role;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<IEnumerable<IdentityRole<Guid>>> GetAllAsync()
        {
            return _roleManager.Roles;
        }

        public async Task<IdentityRole<Guid>> GetByIdAsync(Guid id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<IdentityRole<Guid>> UpdateAsync(IdentityRole<Guid> role)
        {
            var existing = await _roleManager.FindByIdAsync(role.Id.ToString());
            if (existing == null) throw new Exception("Role not found");

            existing.Name = role.Name;
            var result = await _roleManager.UpdateAsync(existing);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors));

            return existing;
        }
    }
}
