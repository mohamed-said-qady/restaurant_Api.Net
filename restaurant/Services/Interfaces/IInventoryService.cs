using restaurant.Model;
using restaurant.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryItem>> GetAllAsync();
        Task<InventoryItem?> GetByMenuItemIdAsync(int menuItemId);
        Task<InventoryItem> CreateAsync(InventoryCreateDto dto);
        Task<bool> UpdateQuantityAsync(int menuItemId, int quantity);

    }
}
