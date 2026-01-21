using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Model;
using restaurant.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Repositories.Implementations
{
    public class InventoryRepository : GenericRepository<InventoryItem>, IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryItem>> GetAllAsync()
            => await _context.InventoryItems
                             .Include(i => i.MenuItem)
                             .ToListAsync();

        public async Task<InventoryItem?> GetByIdAsync(int id)
            => await _context.InventoryItems
                             .Include(i => i.MenuItem)
                             .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<InventoryItem?> GetByMenuItemIdAsync(int menuItemId)
            => await _context.InventoryItems
                             .Include(i => i.MenuItem)
                             .FirstOrDefaultAsync(x => x.MenuItemId == menuItemId);

        public async Task DecreaseAsync(int menuItemId, decimal quantity)
        {
            var item = await GetByMenuItemIdAsync(menuItemId);

            if (item == null)
                throw new InvalidOperationException("Inventory item not found.");

            if (item.Quantity < quantity)
                throw new InvalidOperationException("Insufficient stock.");

            item.Quantity -= quantity;
            await _context.SaveChangesAsync();
        }
    }
}
