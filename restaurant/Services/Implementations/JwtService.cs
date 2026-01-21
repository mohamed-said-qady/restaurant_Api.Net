
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using restaurant.Services.Interfaces;
using System.Security.Claims;
using System.Text;
using restaurant.Model;

namespace restaurant.Services.Implementations
{

    public class JwtService : IJwtService
    {
        private const string SecretKey = "SUPER_SECRET_KEY"; // ممكن تحطه في appsettings.json

        public string GenerateToken(ApplicationUser user, string role, List<string> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };

            foreach (var p in permissions)
                claims.Add(new Claim("permission", p)); // Claim مخصص للصلاحيات

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}