using System.Collections.Generic;
using System.Threading.Tasks;
using restaurant.Model;
using restaurant.Repositories.Interfaces;
using restaurant.Dtos;
namespace restaurant.Repositories.Interfaces
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
       Task<List<string>> GetPermissionsByRoleAsync(string roleName);
       Task<bool> CreateRoleWithPermission(CreateRoleWithPermissionDto dto);
    }
}
 
