using restaurant.Model;
using restaurant.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using restaurant.Helper;
namespace restaurant.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<ServiceResult<IEnumerable<InventoryItem>>> GetAllAsync(invetorySpecParams dto);
        Task<ServiceResult<InventoryItem?>> GetByMenuItemIdAsync(int menuItemId);
        Task<ServiceResult<InventoryItem>> CreateAsync(InventoryCreateDto dto);
        Task<ServiceResult<bool>> UpdateQuantityAsync(int menuItemId, int quantity);

    }
}
