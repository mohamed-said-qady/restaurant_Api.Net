
using restaurant.Model;
using System.Collections.Generic;

namespace restaurant.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, string role, List<string> permissions);
    }
}