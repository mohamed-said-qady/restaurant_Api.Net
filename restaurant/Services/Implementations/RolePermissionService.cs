using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Dtos;
using restaurant.Model;
using restaurant.Repositories.Interfaces;
using restaurant.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace restaurant.Repositories.Implementations
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _RolePermissionRepository ;
        
        
        public RolePermissionService( IRolePermissionRepository RolePermissionRepository)
        {
            _RolePermissionRepository = RolePermissionRepository;
        }

        public async Task<bool> CreateRoleWithPermission(CreateRoleWithPermissionDto dto)
        {
            
            return await _RolePermissionRepository.CreateRoleWithPermission(dto);

        }



        public async Task<List<string>> GetPermissionsByRoleAsync(string roleName)
        {
            return await _RolePermissionRepository.GetPermissionsByRoleAsync(roleName);
        }
    }
}
