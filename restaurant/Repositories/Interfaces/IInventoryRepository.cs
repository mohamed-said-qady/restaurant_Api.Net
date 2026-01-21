using restaurant.Model;
using restaurant.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace restaurant.Repositories.Implementations { 

public interface IInventoryRepository : IGenericRepository<InventoryItem>
{

    Task<InventoryItem?> GetByMenuItemIdAsync(int menuItemId);

    Task DecreaseAsync(int menuItemId, decimal quantity); // خفض عند إنشاء order
}
}