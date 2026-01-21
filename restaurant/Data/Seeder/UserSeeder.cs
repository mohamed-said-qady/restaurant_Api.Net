using Microsoft.AspNetCore.Identity;
using restaurant.Authorization;
using restaurant.Model;
using System.Threading.Tasks;

namespace restaurant.Data.Seeders
{
    public class UserSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await CreateUserIfNotExists(
                email: "admin@restaurant.com",
                password: "Admin@123",
                role: Roles.Admin
            );

            await CreateUserIfNotExists(
                email: "chef@restaurant.com",
                password: "Chef@123",
                role: Roles.Chef
            );
        }

        private async Task CreateUserIfNotExists(
            string email,
            string password,
            string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                return;

            var newUser = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, role);
            }
        }
    }
}
