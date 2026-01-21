

using restaurant.Dtos;
using System.Threading.Tasks;

namespace restaurant.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
    }
}
