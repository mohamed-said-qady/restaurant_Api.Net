using restaurant.Model;
using restaurant.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem?> GetByIdAsync(int id);
        Task<MenuItem> CreateAsync(MenuItemCreateDto dto);
        Task<MenuItem?> UpdateAsync(int id, MenuItemUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
