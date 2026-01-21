// File: Services/Interfaces/IRoleService.cs
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityRole<Guid>> GetByIdAsync(Guid id);
        Task<IEnumerable<IdentityRole<Guid>>> GetAllAsync();
        Task<IdentityRole<Guid>> CreateAsync(string roleName);
        Task<IdentityRole<Guid>> UpdateAsync(IdentityRole<Guid> role);
        Task<bool> DeleteAsync(Guid id);
    }
}
