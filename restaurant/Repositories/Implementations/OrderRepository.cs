using System.Threading.Tasks;
using System.Collections.Generic;
using restaurant.Model;
using restaurant.Data;
using Microsoft.EntityFrameworkCore;
using restaurant.Repositories.Interfaces;

namespace restaurant.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Order> SearchItemAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<IEnumerable<Order>> GetByMenuItemAsync(int menuItemId)
        {
            return await _context.Orders
                .Include(o => o.Details)
                .Where(o => o.Details.Any(d => d.MenuItemId == menuItemId))
                .ToListAsync();
        }




    }

}