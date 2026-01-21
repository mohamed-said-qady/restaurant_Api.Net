using System.Threading.Tasks;
using System.Collections.Generic;
using restaurant.Model;
using restaurant.Repositories.Interfaces;

namespace restaurant.Repositories.Interfaces{
    public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order> SearchItemAsync(int id);
    Task<IEnumerable<Order>> GetByMenuItemAsync(int menuItemId);
    }
}