using Microsoft.AspNetCore.Identity;
using restaurant.Authorization;
using System;
using System.Threading.Tasks;

namespace restaurant.Data.Seeders
{
    public class RoleSeeder
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(
                    new IdentityRole<Guid>(Roles.Admin));

            if (!await _roleManager.RoleExistsAsync(Roles.Chef))
                await _roleManager.CreateAsync(
                    new IdentityRole<Guid>(Roles.Chef));
        }
    }
}
