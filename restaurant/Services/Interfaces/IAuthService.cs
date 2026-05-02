

using restaurant.Dtos;
using System.Threading.Tasks;
using restaurant.Helper;
namespace restaurant.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<string>> LoginAsync(string username, string password);
    }
}
