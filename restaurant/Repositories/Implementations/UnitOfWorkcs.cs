using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using restaurant.Data;
using restaurant.Repositories.Interfaces;

namespace restaurant.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        // بنحقن كل حاجة هنا يا محمد
        public IOrderRepository Orders { get; private set; }
        public IMenuItemRepository MenuItems { get; private set; }
        public IInventoryRepository Inventory { get; private set; }
        public IRolePermissionRepository RolePermissions { get; private set; }

        public UnitOfWork(
            AppDbContext context,
            IOrderRepository orders,
            IMenuItemRepository menuItems,
            IInventoryRepository inventory,
            IRolePermissionRepository rolePermissions)
        {
            _context = context;
            Orders = orders;
            MenuItems = menuItems;
            Inventory = inventory;
            RolePermissions = rolePermissions;
             
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        public Task CommitTransactionAsync()
        {

        }
        Task RollbackTransactionAsync() { }
    }
}