
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Interfaces
{
    public interface IRolePermissionService
    {
        Task<List<string>> GetPermissionsByRoleAsync(string roleName);
    }
}
