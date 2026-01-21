// File: Services/Interfaces/IUserService.cs
using restaurant.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetByIdAsync(Guid id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);
        Task<ApplicationUser> UpdateAsync(ApplicationUser user);
        Task<bool> DeleteAsync(Guid id);
    }
}
