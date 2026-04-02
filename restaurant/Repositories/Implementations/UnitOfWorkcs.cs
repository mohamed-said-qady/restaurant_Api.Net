using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using restaurant.Data;
using restaurant.Repositories.Interfaces;

namespace restaurant.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction; 

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

        // 1. بدأ العملية
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        // 2. حفظ التغييرات وتأكيد العملية
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync(); // لازم نحفظ الداتا الأول
                if (_transaction != null)
                {
                    await _transaction.CommitAsync(); // نأكد العملية في الداتا بيز
                }
            }
            catch
            {
                await RollbackTransactionAsync(); // لو الحفظ فشل، ارجع في كلامك فوراً
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        // 3. إلغاء كل اللي حصل لو فيه مشكلة
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}