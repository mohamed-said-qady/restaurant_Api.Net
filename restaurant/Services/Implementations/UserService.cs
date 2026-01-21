// File: Services/Implementation/UserService.cs
using Microsoft.AspNetCore.Identity;
using restaurant.Model;
using restaurant.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors));
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return _userManager.Users;
        }

        public async Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            var existing = await _userManager.FindByIdAsync(user.Id.ToString());
            if (existing == null) throw new Exception("User not found");

            existing.UserName = user.UserName;
            existing.Email = user.Email;

            var result = await _userManager.UpdateAsync(existing);
            if (!result.Succeeded) 
                throw new Exception(string.Join(", ", result.Errors));

            return existing;
        }
    }
}
